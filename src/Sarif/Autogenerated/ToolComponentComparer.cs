// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Sarif;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type ToolComponent for sorting.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.3.0")]
    internal sealed class ToolComponentComparer : IComparer<ToolComponent>
    {
        internal static readonly ToolComponentComparer Instance = new ToolComponentComparer();

        public int Compare(ToolComponent left, ToolComponent right)
        {
            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Guid, right.Guid);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Name, right.Name);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Organization, right.Organization);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Product, right.Product);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.ProductSuite, right.ProductSuite);
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

            compareResult = string.Compare(left.FullName, right.FullName);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Version, right.Version);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.SemanticVersion, right.SemanticVersion);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.DottedQuadFileVersion, right.DottedQuadFileVersion);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.ReleaseDateUtc, right.ReleaseDateUtc);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.DownloadUri.UriCompares(right.DownloadUri);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.InformationUri.UriCompares(right.InformationUri);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.GlobalMessageStrings.DictionaryCompares(right.GlobalMessageStrings, MultiformatMessageStringComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Notifications.ListCompares(right.Notifications, ReportingDescriptorComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Rules.ListCompares(right.Rules, ReportingDescriptorComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Taxa.ListCompares(right.Taxa, ReportingDescriptorComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Locations.ListCompares(right.Locations, ArtifactLocationComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Language, right.Language);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Contents.CompareTo(right.Contents);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.IsComprehensive.CompareTo(right.IsComprehensive);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.LocalizedDataSemanticVersion, right.LocalizedDataSemanticVersion);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.MinimumRequiredLocalizedDataSemanticVersion, right.MinimumRequiredLocalizedDataSemanticVersion);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = ToolComponentReferenceComparer.Instance.Compare(left.AssociatedComponent, right.AssociatedComponent);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = TranslationMetadataComparer.Instance.Compare(left.TranslationMetadata, right.TranslationMetadata);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.SupportedTaxonomies.ListCompares(right.SupportedTaxonomies, ToolComponentReferenceComparer.Instance);
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