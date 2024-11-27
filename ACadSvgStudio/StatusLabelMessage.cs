#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using ScintillaNET;

namespace ACadSvgStudio {

	internal class StatusLabelMessage {

		private Scintilla _scintillaSvgGroupEditor;
		private ToolStripStatusLabel _statusLabel;


		public StatusLabelMessage(Scintilla scintillaSvgGroupEditor, ToolStripStatusLabel statusLabel) {
			_scintillaSvgGroupEditor = scintillaSvgGroupEditor;
			_statusLabel = statusLabel;
			_statusLabel.DoubleClickEnabled = true;
			_statusLabel.DoubleClick += _statusLabel_DoubleClick;
		}


		public string Message { get; private set; }

		public int Line { get; private set; } = -1;


		public void ClearMessage()
		{
			Message = string.Empty;
			Line = -1;

			_statusLabel.Text = string.Empty;
		}

		public void SetMessage(string message, int line = -1) {
			Message = message;
			Line = line;

			_statusLabel.Text = message;
		}


		private void _statusLabel_DoubleClick(object? sender, EventArgs e) {
			if (Line < 0 || Line > _scintillaSvgGroupEditor.Lines.Count) {
				return;
			}

			Line line = _scintillaSvgGroupEditor.Lines[Line - 1];
			_scintillaSvgGroupEditor.SetSelection(line.Position, line.Position + line.Length);
			_scintillaSvgGroupEditor.ScrollCaret();
		}
	}
}
