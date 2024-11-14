#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio.BatchProcessing {

    internal class EmptyLineCommand : CommandBase {

        public EmptyLineCommand() {
            _commandLine = string.Empty;
            _parseError = string.Empty;
        }


        public override string ToCommandLine() {
            return string.Empty;
        }
    }
}
