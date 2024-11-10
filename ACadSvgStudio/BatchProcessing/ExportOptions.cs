#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using CommandLine;

namespace ACadSvgStudio.BatchProcessing {

    [Verb("export", HelpText = "Exports .svg or .g.svg file with selected defs.")]
    public class ExportOptions {

        [Option(
            'i', "input",
            Required = true,
            HelpText = "Input files like .dwg or .dxf.")]
        public string Input { get; set; }

        [Option(
            'o', "output",
            Required = true,
            HelpText = "Output files like .svg or .g.svg.")]
        public string Output { get; set; }


        [Option(
            'd', "defs-groups",
            Required = true,
            Separator = ' ',
            HelpText = "Selected groups in defs to be included.")]
        public IEnumerable<string>? IncludedDefs { get; set; }


        [Option(
            'r', "resolve-defs",
            Required = false,
            Default = false,
            HelpText = "Should defs be resolved.")]
        public bool ResolveDefs { get; set; }
    }
}
