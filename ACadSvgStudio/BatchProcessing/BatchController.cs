#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio.BatchProcessing {

    internal static class BatchController {

        private static Batch _currentBatch;


        public static Batch CurrentBatch { get { return _currentBatch; } }

            
        internal static Batch CreateBatch(string batchPath) {
            _currentBatch = new Batch(batchPath);
            return _currentBatch;
        }


        internal static Batch LoadBatch(string path) {
            _currentBatch = Batch.FromFile(path);
            return _currentBatch;
        }
    }
}
