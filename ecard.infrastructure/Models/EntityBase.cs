//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Runtime.Serialization;

namespace Ecard.Models
{
    [DataContract]
    public class EntityBase : IEntityBase
    {
        public EntityBase()
        {
            State = 1;
        }
        public int Id { get; set; }
        public int State { get; set; }
    }
    public interface IKeySetter
    {
        int Id { get; set; }
    }
    public interface IKeyObject
    {
        int Id { get;   }
    }
    public interface IEntityBase : IKeyObject
    {
        int State { get; set; }
    }
}
