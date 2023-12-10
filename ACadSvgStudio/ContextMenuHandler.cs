#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using CefSharp;
using CefSharp.Handler;


namespace ACadSvgStudio {

    internal class MyContextMenuHandler : ContextMenuHandler {
        private MainForm _mainForm;

        public MyContextMenuHandler(MainForm mainForm) {
            _mainForm = mainForm;
        }


        protected override void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model) {
            base.OnBeforeContextMenu(chromiumWebBrowser, browser, frame, parameters, model);

            model.RemoveAt(0);
            model.RemoveAt(0);
            model.RemoveAt(0);
            model.RemoveAt(0);

            model.AddSeparator();

            model.AddItem((CefMenuCommand)101, "Center to Fit");
        }


        protected override bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags) {
            if (commandId == (CefMenuCommand)101) {
                _mainForm.centerToFit();
                return true;
            }

            return false;
        }
    }
}