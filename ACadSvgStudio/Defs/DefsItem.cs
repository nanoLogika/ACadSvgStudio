#region copyright LGPL nanoLogika
//  Copyright 2023, nanoLogika GmbH.
//  All rights reserved. 
//  This source code is licensed under the "LGPL v3 or any later version" license. 
//  See LICENSE file in the project root for full license information.
#endregion


namespace ACadSvgStudio.Defs
{

    internal class DefsItem
    {

        public string Id { get; private set; }

        public List<DefsItem> Children { get; private set; } = new List<DefsItem>();


        public DefsItem(string id)
        {
            Id = id;
        }
    }
}
