namespace ACadSvgStudio.CommandLine {

	internal partial class CommandLineForm : Form {

		private AppContext _appContext;


		public CommandLineForm()
		{
			InitializeComponent();
		}


		public CommandLineForm(AppContext appContext) : this()
		{
			_appContext = appContext;

			_appContext.CommandLineParser.OutputWritten += CommandLineParser_OutputWritten;
		}


		private List<string> splitArguments(string input)
		{
			var result = new List<string>();
			bool insideQuotes = false;
			string currentArg = "";

			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];

				if (c == '"')
				{
					insideQuotes = !insideQuotes;
				}
				else if (char.IsWhiteSpace(c) && !insideQuotes)
				{
					if (!string.IsNullOrEmpty(currentArg))
					{
						result.Add(currentArg);
						currentArg = "";
					}
				}
				else
				{
					currentArg += c;
				}
			}

			if (!string.IsNullOrEmpty(currentArg))
			{
				result.Add(currentArg);
			}

			return result;
		}


		private void CommandLineParser_OutputWritten(object? sender, string e)
		{
			outputTextBox.AppendText($"{e}{Environment.NewLine}");
			outputTextBox.SelectionStart = outputTextBox.Text.Length;
			outputTextBox.ScrollToCaret();
		}

		private void executeButton_Click(object sender, EventArgs e)
		{
			string[] args = splitArguments(commandTextBox.Text).ToArray();

			_appContext.CommandLineParser.Parse(args);
			commandTextBox.Text = string.Empty;
		}

		private void commandTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				executeButton.PerformClick();
			}
		}
	}
}
