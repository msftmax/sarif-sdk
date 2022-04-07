// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Sarif;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type PhysicalLocation for sorting.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.3.0")]
    internal sealed class PhysicalLocationComparer : IComparer<PhysicalLocation>
    {
        internal static readonly PhysicalLocationComparer Instance = new PhysicalLocationComparer();

        public int Compare(PhysicalLocation left, PhysicalLocation right)
        {
            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = AddressComparer.Instance.Compare(left.Address, right.Address);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = ArtifactLocationComparer.Instance.Compare(left.ArtifactLocation, right.ArtifactLocation);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = RegionComparer.Instance.Compare(left.Region, right.Region);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = RegionComparer.Instance.Compare(left.ContextRegion, right.ContextRegion);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Properties.DictionaryCompares(right.Properties, SerializedPropertyInfoComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            return compareResult;
        }
    }
}