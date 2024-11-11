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


        internal static Batch LoadOrCreateBatch(string batchPath) {
            if (File.Exists(batchPath)) {
                _currentBatch = Batch.FromFile(batchPath);
            }
            else {
                _currentBatch = new Batch(batchPath);
            }
            return _currentBatch;
        }


        internal static Batch UpdateBatch(string batchScript) {
            Batch batch = new Batch(_currentBatch.Path);
            
            string[] lines = batchScript.Split(new char[] { '\r', '\n' });
            foreach (string line in lines) {
                try {
					batch.AddCommand(line);
				}
                catch (Exception ex) {
                    // Error in batch code line...
                }
            }

            _currentBatch = batch;

            return batch;
		}
    }
}
