#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion

namespace ACadSvgStudio.Defs {

    internal class DefsListViewItem : ListViewItem {

        public string DefsId { get; }


        public DefsListViewItem(string defsId) : base() {
            DefsId = defsId;
        }


		public override string ToString() {
			return DefsId;
		}
	}
}
