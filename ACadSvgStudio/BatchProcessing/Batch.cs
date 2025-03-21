﻿#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using ACadSvg;

using System.Text;

namespace ACadSvgStudio.BatchProcessing {

    internal class Batch {

        private string _path;
        private IList<CommandBase> _commands = new List<CommandBase>();
        private bool _hasChanges = false;
        private string _log;


        public string Log {
            get { return _log; }
        }


        public Batch(string path) {
            _path = path;
        }

        public bool HasChanges {
            get { return _hasChanges; }
        }


        public bool IsEmpty {
            get { return _commands.Count == 0; }
        }


        public string Name {
            get { return System.IO.Path.GetFileNameWithoutExtension(_path ?? "*unnamed*"); }
        }


        public string Path {
            get { return _path; }
        }


        public static Batch FromFile(string path) {
            Batch batch = new Batch(path);

            foreach (string commandLine in File.ReadAllLines(path)) {
                batch.AddCommand(commandLine);
            }

            batch._hasChanges = false;
            return batch;
        }


        public bool HasErrors(out string errors) {
            StringBuilder sb = new StringBuilder();

            foreach (CommandBase command in _commands) {
                if (command.HasParseError) {
                    sb.AppendLine($"Parse error: {command.ParseError}");
                }
                //else if (command is ExportCommand exportCommand) {
                //    if (!File.Exists(exportCommand.InputFile)) {
                //        sb.AppendLine($"File \"{exportCommand.InputFile}\" does not exist");
                //    }
                //}
            }

            errors = sb.ToString();
            return sb.Length > 0;
        }


        public void AddCommand(string commandLine) {
            if (string.IsNullOrWhiteSpace(commandLine)) {
                AddCommand(new EmptyLineCommand());
                return;
            }
            if (commandLine.Trim().StartsWith("#")) {
                AddCommand(new CommentLineCommand(commandLine));
                return;
            }

            int lengthOfVerb = commandLine.IndexOf(' ');
            if (lengthOfVerb == -1) {
                lengthOfVerb = commandLine.Length;
            }
            string verb = commandLine.Substring(0, lengthOfVerb).ToLower();

            CommandBase command;
            switch (verb) {
            case "export":
                command = ExportCommand.FromCommandLine(commandLine);
                break;
            default:
                command = new UnknownVerbCommand(verb, commandLine);
                break;
            }
            AddCommand(command);

            _hasChanges = true;
        }


        public void AddCommand(CommandBase command) {
            //  Check for duplicate commands by verb, key arguments (see Equals!)
            int existingIndex = _commands.IndexOf(command);
            if (existingIndex >= 0) {
                _commands[existingIndex] = command;
                _hasChanges = true;
            }
            else {
                _commands.Add(command);
            }
        }


        public void Execute(ConversionContext conversionContext) {
            StringBuilder sb = new StringBuilder();
            foreach (CommandBase command in _commands) {
                command.Execute(conversionContext);
                sb.AppendLine(command.Message);
            }
            _log = sb.ToString();
        }


        public void Save() {
            if (string.IsNullOrEmpty(_path)) {
                throw new InvalidOperationException("Path is not set, call SaveAs()");
            }
            save();
        }


        public void SaveAs(string path) {
            _path = path ?? throw new ArgumentException(nameof(path));
            save();
        }


        private void save() {
            try {
                using (StreamWriter outputBatch = File.CreateText(_path)) {
                    foreach (CommandBase command in _commands) {
                        outputBatch.WriteLine(command.ToCommandLine());
                    }
                    outputBatch.Flush();
                    outputBatch.Close();
                }
            }
            catch (Exception ex) {
                throw;
            }
            _hasChanges = false;
        }


        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (CommandBase command in _commands) {
                sb.AppendLine(command.ToCommandLine());
            }
            return sb.ToString();
        }


        public IList<int> GetErrorLines() {
            IList<int> lines = new List<int>();
            int i = 0;
            foreach (CommandBase command in _commands) {
                if (command.HasParseError) {
                    lines.Add(i);
                }
                i++;
            }
            return lines;
        }


        public string GetParseErrorInfos() {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (CommandBase command in _commands) {
                if (command.HasParseError) {
                    sb.Append(i).Append(": ").AppendLine(command.ParseError);
                }
            }
            return sb.ToString();
        }
    }
}
