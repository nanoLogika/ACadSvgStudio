#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using System.Text;

using ACadSvg;

using ACadSvgStudio.Defs;

namespace ACadSvgStudio.BatchProcessing {

    internal class ExportCommand : CommandBase {

        private string _inputPath;
        private string _outputPath;
        private string[] _defsGroupIds;
        private bool _resolveDefs;
        private bool _removeDevsGroupAttributes;


        public ExportCommand(string commandLine, string message) {
            _commandLine = commandLine;
            _parseError = message;
        }


        public ExportCommand(string inputPath, string outputPath, bool resolveDefs, bool removeDevsGroupAttributes, string[] defsGroupIds) {
            _inputPath = inputPath ?? throw new ArgumentNullException(nameof(inputPath));
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));

            _inputPath = getRelativePath(_inputPath, Settings.Default.BatchACadLoadBaseDirectory);
            _outputPath = getRelativePath(_outputPath, Settings.Default.BatchSvgExportBaseDirectory);

            _resolveDefs = resolveDefs;
            _removeDevsGroupAttributes = removeDevsGroupAttributes;
            _defsGroupIds = defsGroupIds ?? throw new ArgumentNullException(nameof(defsGroupIds));
            _parseError = string.Empty;
        }


        private string getRelativePath(string fullPath, string basePath) {
            if (!string.IsNullOrEmpty(basePath) && Directory.Exists(basePath)) {
                return fullPath.Replace(basePath, "").TrimStart('\\');
            }
            else {
                return fullPath;
            }
        }


        public override void Execute(ConversionContext conversionContext, out string msg) {
            string inputPath = getFullPath(_inputPath, Settings.Default.BatchACadLoadBaseDirectory);
            string outputPath = getFullPath(_outputPath, Settings.Default.BatchSvgExportBaseDirectory);

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir)) {
                Directory.CreateDirectory(outputDir);
            }

            DocumentSvg docSvg = ACadLoader.LoadDwg(inputPath, conversionContext);
            string svgText = docSvg.ToSvg();
            DefsExporter exporter = new DefsExporter(svgText, _resolveDefs);
            exporter.Export(outputPath, _defsGroupIds);

            msg = $"Exporting '{_outputPath}' finished.";
        }


        private string getFullPath(string path, string baseDirectory) {
            string fullPath = path;
            if (!Path.IsPathFullyQualified(path)) {
                if (string.IsNullOrEmpty(baseDirectory)) {
                    throw new InvalidOperationException($"{path} is a ralative path, plrase specify a base path in the batch-processing settings.");
                }
                fullPath = Path.Combine(baseDirectory, path);
            }
            return fullPath;
        }


        public static ExportCommand FromCommandLine(string commandLine) {
            try {
                ExportOptions exportOptions = CommandLineParser.ParseCommandLine(commandLine, out string errorInfo);

                if (string.IsNullOrEmpty(errorInfo)) {
                    return new ExportCommand(
                        exportOptions.Input, exportOptions.Output,
                        exportOptions.ResolveDefs, false,
                        exportOptions.IncludedDefs.ToArray<string>());
                }

                return new ExportCommand(commandLine, errorInfo);
            }
            catch (Exception ex) {
                return new ExportCommand(commandLine, ex.Message);
            }
        }


        public override string ToCommandLine() {
            if (string.IsNullOrEmpty(_parseError)) {

                string rd = _resolveDefs ? " -r+" : string.Empty;
                string rga = _removeDevsGroupAttributes ? " -a+" : string.Empty;

                StringBuilder commandLineSb = new StringBuilder("EXPORT");
                commandLineSb.Append(" -o \"").Append(_outputPath).Append('"');
                commandLineSb.Append(" -i \"").Append(_inputPath).Append('"');

                commandLineSb.Append(rd);
                commandLineSb.Append(rga);
                commandLineSb.Append(" -d");
                foreach (string gId in _defsGroupIds) {
                    commandLineSb.Append(' ').Append(gId);
                }

                return commandLineSb.ToString();
            }
            else {
                return _commandLine;
            }
        }


        public override string ToString() {
            string rd = _resolveDefs ? " -r+" : string.Empty;
            string rga = _removeDevsGroupAttributes ? " -a+" : string.Empty;

            StringBuilder commandLineSb = new StringBuilder("EXPORT");
            commandLineSb.Append(" -o ").Append(Path.GetFileNameWithoutExtension(_outputPath));
            commandLineSb.Append(" -i ").Append(Path.GetFileNameWithoutExtension(_inputPath));
            commandLineSb.Append(rd);
            commandLineSb.Append(rga);
            commandLineSb.Append(" -d");
            foreach (string gId in _defsGroupIds) {
                commandLineSb.Append(' ').Append(gId);
            }

            return commandLineSb.ToString();
        }


        public override bool Equals(object? obj) {
            if (obj == null) {
                return false;
            }
            if (!(obj is ExportCommand otherExportCommand)) {
                return false;
            }

            return otherExportCommand._inputPath == _inputPath &&
                otherExportCommand._outputPath == _outputPath;
        }
    }
}
