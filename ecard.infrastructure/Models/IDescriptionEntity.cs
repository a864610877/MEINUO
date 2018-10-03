﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Ecard.Models
{
    public interface IDescriptionEntity : INamedEntity
    {
        string Description { get; }
    }
}
