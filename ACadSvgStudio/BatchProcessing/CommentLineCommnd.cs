#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


using ACadSvg;

namespace ACadSvgStudio.BatchProcessing {

    internal class CommentLineCommand : CommandBase {

        public CommentLineCommand(string commandLine) {
            _commandLine = commandLine;
            _parseError = string.Empty;
        }


        public override string ToCommandLine() {
            return _commandLine;
        }


        public override void Execute(ConversionContext conversionContext, out string msg) {
            msg = _commandLine;
            return;
        }
    }
}
