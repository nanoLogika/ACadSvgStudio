using CommandLine;
using System.Text;

namespace ACadSvgStudio.CommandLine {

    internal class CommandLineParser {

        private AppContext _appContext;

        public event EventHandler<string> OutputWritten;


        public CommandLineParser(AppContext appContext) {
            _appContext = appContext;
        }


        public void Parse(string[] args) {
            Parser.Default.ParseArguments<ExportOptions>(args)
                .WithParsed(ParseExportOptions)
                .WithNotParsed(HandleParseError);
        }


        private void ParseExportOptions(ExportOptions options) {
            if (!File.Exists(options.Input)) {
                OnOutputWritten($"File {options.Input} does not exist!");
                return;
            }

			OnOutputWritten($"Exporting \"{options.Input}\" started...");

			_appContext.ExportDefsFromDwgFile(options.Input, options.IncludedDefs, options.ResolveDefs, options.Output);

            OnOutputWritten($"File \"{options.Output}\" exported!");
		}

        private void HandleParseError(IEnumerable<Error> errors) {
            StringBuilder sb = new StringBuilder();
            foreach (Error error in errors) {
                sb.AppendLine($"{error.Tag}");
            }
            OnOutputWritten(sb.ToString());
        }


        protected virtual void OnOutputWritten(string message) {
            OutputWritten?.Invoke(this, message);
        }
    }
}
