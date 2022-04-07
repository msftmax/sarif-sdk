// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Sarif;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type ReportingDescriptor for sorting.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.3.0")]
    internal sealed class ReportingDescriptorComparer : IComparer<ReportingDescriptor>
    {
        internal static readonly ReportingDescriptorComparer Instance = new ReportingDescriptorComparer();

        public int Compare(ReportingDescriptor left, ReportingDescriptor right)
        {
            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Id, right.Id);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.DeprecatedIds.ListCompares(right.DeprecatedIds);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Guid, right.Guid);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.DeprecatedGuids.ListCompares(right.DeprecatedGuids);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Name, right.Name);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.DeprecatedNames.ListCompares(right.DeprecatedNames);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = MultiformatMessageStringComparer.Instance.Compare(left.ShortDescription, right.ShortDescription);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = MultiformatMessageStringComparer.Instance.Compare(left.FullDescription, right.FullDescription);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.MessageStrings.DictionaryCompares(right.MessageStrings, MultiformatMessageStringComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = ReportingConfigurationComparer.Instance.Compare(left.DefaultConfiguration, right.DefaultConfiguration);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.HelpUri.UriCompares(right.HelpUri);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = MultiformatMessageStringComparer.Instance.Compare(left.Help, right.Help);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Relationships.ListCompares(right.Relationships, ReportingDescriptorRelationshipComparer.Instance);
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