//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Runtime.Serialization;

namespace Ecard.Models
{
    [DataContract]
    public class NamedEntity : INamedEntity
    {
        [DataMember]
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
