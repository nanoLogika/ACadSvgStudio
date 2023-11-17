#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using ScintillaNET;

namespace ACadSvgStudio {

	public static class ScintillaExtensions {

		public static void CollapseChildren(this ScintillaNET.Scintilla scintilla) {
			scintilla.DirectMessage(NativeMethods.SCI_FOLDCHILDREN, IntPtr.Zero, IntPtr.Zero);
		}

		public static void ExpandChildren(this ScintillaNET.Scintilla scintilla) {
			scintilla.DirectMessage(NativeMethods.SCI_EXPANDCHILDREN, IntPtr.Zero, IntPtr.Zero);
		}
	}
}
