using ACadSvg;
using ACadSvgStudio.CommandLine;
using ACadSvgStudio.Defs;

namespace ACadSvgStudio {

	internal class AppContext {

		public CommandLineParser CommandLineParser { get; }

		public ConversionContext ConversionContext { get; private set; }

		public SvgProperties SvgProperties { get; private set; }


		public AppContext() {
			CommandLineParser = new CommandLineParser(this);
			SvgProperties = new SvgProperties(null);
		}


		public void CreateConversionContext()
		{
			ConversionContext = new ConversionContext()
			{
				ConversionOptions = SvgProperties.GetConversionOptions(),
				ViewboxData = SvgProperties.GetViewbox(),
				GlobalAttributeData = SvgProperties.GetGlobalAttributeData()
			};
		}


		public void ExportDefsFromDwgFile(string fileName, IEnumerable<string> selectedDefsIds, bool resolveDefs, string outputFileName) {
			DocumentSvg docSvg = ACadLoader.LoadDwg(fileName, ConversionContext);
			string svgText = docSvg.ToSvg();

			DefsExporter exporter = new DefsExporter(svgText, selectedDefsIds.ToArray(), resolveDefs);
			exporter.Export(outputFileName, selectedDefsIds.Count() == 0);
		}
	}
}
