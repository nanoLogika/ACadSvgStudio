#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using CommandLine;


namespace ACadSvgStudio.BatchProcessing {

    internal static class CommandLineParser {

        public static ExportOptions ParseCommandLine(string commandLine) {
            string[] args = splitArguments(commandLine);

            ParserResult<ExportOptions> result = Parser.Default.ParseArguments<ExportOptions>(args);
            if (result.Tag == ParserResultType.NotParsed) {
                throw new InvalidDataException("Invalid command line: " + commandLine);
            }

            return result.Value;
        }


        private static string[] splitArguments(string commandLine) {
            var result = new List<string>();
            bool insideQuotes = false;
            string currentArg = "";

            for (int i = 0; i < commandLine.Length; i++) {
                char c = commandLine[i];

                if (c == '"') {
                    insideQuotes = !insideQuotes;
                }
                else if (char.IsWhiteSpace(c) && !insideQuotes) {
                    if (!string.IsNullOrEmpty(currentArg)) {
                        result.Add(currentArg);
                        currentArg = "";
                    }
                }
                else {
                    currentArg += c;
                }
            }

            if (!string.IsNullOrEmpty(currentArg)) {
                result.Add(currentArg);
            }

            return result.ToArray();
        }
    }
}
