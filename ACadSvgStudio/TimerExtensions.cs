#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio {

    internal static class TimerExtensions {

        public static void Restart(this System.Windows.Forms.Timer timer) {
            timer.Stop();
            timer.Start();
        }
    }
}
