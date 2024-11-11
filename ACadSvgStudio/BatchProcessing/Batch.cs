#region copyright LGPL nanoLogika
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


        public Batch(string path) {
            _path = path;
        }

        public bool HasChanges {
            get { return _hasChanges; }
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


        public void AddCommand(string commandLine) {

            string verb = commandLine.Substring(0, commandLine.IndexOf(' ')).ToLower();

            CommandBase command;
            switch (verb) {
            case "export":
                command = ExportCommand.FromCommandLine(commandLine);
                break;
            default:
                throw new InvalidOperationException($"Command {verb} not supported.");
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


        public void Execute(ConversionContext conversionContext, out string msg) {
            StringBuilder sb = new StringBuilder();
            foreach (CommandBase command in _commands) {
                command.Execute(conversionContext, out string cmdMsg);
                sb.Append(cmdMsg);
            }
            msg = sb.ToString();
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
	}
}
