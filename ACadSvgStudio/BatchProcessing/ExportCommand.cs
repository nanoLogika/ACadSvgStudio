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

        public ExportCommand() { }


        public ExportCommand(string inputPath, string outputPath, bool resolveDefs, bool removeDevsGroupAttributes, string[] defsGroupIds) {
            InputPath = inputPath ?? throw new ArgumentNullException(nameof(inputPath));
            OutputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            ResolveDefs = resolveDefs;
            RemoveDevsGroupAttributes = removeDevsGroupAttributes;
            DefsGroupIds = defsGroupIds ?? throw new ArgumentNullException(nameof(defsGroupIds));
        }


        public string InputPath { get; set; } = string.Empty;


        public string OutputPath { get; set; } = string.Empty;


        public string[] DefsGroupIds { get; set; }


        public bool ResolveDefs { get; set; }


        public bool RemoveDevsGroupAttributes { get; set; }


        public override void Execute(ConversionContext conversionContext) {
            DocumentSvg docSvg = ACadLoader.LoadDwg(InputPath, conversionContext);
            string svgText = docSvg.ToSvg();
            DefsExporter exporter = new DefsExporter(svgText, DefsGroupIds, ResolveDefs);
            exporter.Export(OutputPath);
        }


        public static ExportCommand FromCommandLine(string commandLine) {
            ExportOptions exportOptions = CommandLineParser.ParseCommandLine(commandLine);

            return new ExportCommand(
                exportOptions.Input, exportOptions.Output,
                exportOptions.ResolveDefs, false,
                exportOptions.IncludedDefs.ToArray<string>());
        }


        public override string ToCommandLine() {
            string rd = ResolveDefs ? " -r+" : string.Empty;
            string rga = RemoveDevsGroupAttributes ? " -rga+" : string.Empty;

            StringBuilder commandLineSb = new StringBuilder("EXPORT");
            commandLineSb.Append(" -o \"").Append(OutputPath).Append('"');
            commandLineSb.Append(" -i \"").Append(InputPath).Append('"');
            ;
            commandLineSb.Append(rd);
            commandLineSb.Append(rga);
            commandLineSb.Append(" -d");
            foreach (string gId in DefsGroupIds) {
                commandLineSb.Append(' ').Append(gId);
            }

            return commandLineSb.ToString();
        }


        public override string ToString() {
            string rd = ResolveDefs ? " -r+" : string.Empty;
            string rga = RemoveDevsGroupAttributes ? " -rga+" : string.Empty;

            StringBuilder commandLineSb = new StringBuilder("EXPORT");
            commandLineSb.Append(" -o ").Append(Path.GetFileNameWithoutExtension(OutputPath));
            commandLineSb.Append(" -i ").Append(Path.GetFileNameWithoutExtension(InputPath));
            commandLineSb.Append(rd);
            commandLineSb.Append(rga);
            commandLineSb.Append(" -d");
            foreach (string gId in DefsGroupIds) {
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

            return otherExportCommand.InputPath == this.InputPath &&
                otherExportCommand.OutputPath == this.OutputPath;
        }
    }
}
