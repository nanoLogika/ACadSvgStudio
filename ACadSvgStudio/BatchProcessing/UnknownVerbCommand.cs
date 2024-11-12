using System;
#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio.BatchProcessing {

    internal class UnknownVerbCommand : CommandBase {

        public UnknownVerbCommand(string verb, string commandLine) {
            _commandLine = commandLine;
            _parseError = $"Command {verb} not supported.";
        }


        public override string ToCommandLine() {
            return _commandLine;
        }
    }
}
