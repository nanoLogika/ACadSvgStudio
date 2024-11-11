#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using ACadSvg;


namespace ACadSvgStudio.BatchProcessing {

    internal abstract class CommandBase {


        public abstract string ToCommandLine();


        public abstract void Execute(ConversionContext conversionContext, out string msg);
    }
}
