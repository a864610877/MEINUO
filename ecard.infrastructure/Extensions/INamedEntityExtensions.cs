//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Linq;
using Ecard.Models;

namespace Ecard
{
    public static class NamedEntityExtensions
    {
        public static IQueryable<T> ByName<T>(this IQueryable<T> query, string name)
             where T : class, INamedEntity
        {
            return query.Where(x => x.Name == name);
        }
        public static string GetDisplayName(this INamedEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.DisplayName))
                return entity.DisplayName;

            return entity.Name;
        }
        public static IQueryable<T> WithName<T>(this IQueryable<T> query, string name)
            where T : class, INamedEntity
        {
            if (string.IsNullOrEmpty(name)) return query;
            return query.Where(x => x.Name.Contains(name));
        }
    }
}
