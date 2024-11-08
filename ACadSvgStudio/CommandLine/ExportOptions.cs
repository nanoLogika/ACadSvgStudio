using CommandLine;

namespace ACadSvgStudio.CommandLine {

	[Verb("export", HelpText = "Exports .svg or .g.svg file with selected defs.")]
	public class ExportOptions {

		[Option('i', "input", Required = true, HelpText = "Input files like .dwg or .dxf.")]
		public string Input { get; set; }

		[Option('o', "output", Required = true, HelpText = "Output files like .svg or .g.svg.")]
		public string Output { get; set; }


		[Option('s', "select-def", Required = false, HelpText = "Select defs to be included.")]
		public IEnumerable<string>? IncludedDefs { get; set; }


		[Option('d', "resolve-defs", Required = false, Default = false, HelpText = "Should defs be resolved.")]
		public bool ResolveDefs { get; set; }
	}
}
