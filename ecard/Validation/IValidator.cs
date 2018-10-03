//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Collections;
using System.Collections.Generic;

namespace Ecard.Validation
{
    public interface IValidator
    {
        IEnumerable<ValidationError> Validate();
    }
    public class ValidationError
    {
        public ValidationError(string propertyName, string error)
        {
            PropertyName = propertyName;
            Error = error;
        }

        public string Error { get; set; }
        public string PropertyName { get; set; }
    }
}
