#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

using System.Collections.Specialized;

namespace ACadSvgStudio {

	internal class RecentlyOpenedFilesManager {

		private const int Limit = 10;


		private StringCollection GetRecentlyOpenedFilesStringCollection() {
			StringCollection stringCollection = Settings.Default.RecentlyOpenedFiles;
            if (stringCollection == null) {
				stringCollection = new StringCollection();
            }
			return stringCollection;
		}


		public List<string> RecentlyOpenedFiles() {
			List<string> list = new List<string>();
			foreach (string file in GetRecentlyOpenedFilesStringCollection())
			{
				list.Add(file);
			}
			return list;
		}


		public bool HasRecentlyOpenedFiles() {
			return RecentlyOpenedFiles().Count > 0;
		}

		public void RegisterFile(string fileName) {
			List<string> recentlyOpenedFiles = RecentlyOpenedFiles();
			if (recentlyOpenedFiles.Contains(fileName)) {
				int index = recentlyOpenedFiles.IndexOf(fileName);
				recentlyOpenedFiles.RemoveAt(index);
				recentlyOpenedFiles.Insert(0, fileName);
			}
			else {
				recentlyOpenedFiles.Insert(0, fileName);
			}

			StringCollection stringCollection = new StringCollection();
			stringCollection.AddRange(recentlyOpenedFiles.Take(Limit).ToArray());

			Settings.Default.RecentlyOpenedFiles = stringCollection;
			Settings.Default.Save();
		}
	}
}
