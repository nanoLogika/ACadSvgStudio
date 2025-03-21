#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using ACadSvg;


namespace ACadSvgStudio.BatchProcessing {

    internal abstract class CommandBase {

        protected string _commandLine;
        protected string _parseError;
        protected string _message;


        public abstract string ToCommandLine();


        public string CommandLine {
            get { return _commandLine; }
        }


        public string Message {
            get { return _message; }
        }


        public string ParseError {
            get { return _parseError; }
        }


        public bool HasParseError {
            get { return !string.IsNullOrEmpty(_parseError); }
        }


        public virtual void Execute(ConversionContext conversionContext) {
            return;
        }
    }
}
