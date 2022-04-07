// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Sarif;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type Run for sorting.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.3.0")]
    internal sealed class RunComparer : IComparer<Run>
    {
        internal static readonly RunComparer Instance = new RunComparer();

        public int Compare(Run left, Run right)
        {
            int compareResult = 0;
            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = ToolComparer.Instance.Compare(left.Tool, right.Tool);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Invocations.ListCompares(right.Invocations, InvocationComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = ConversionComparer.Instance.Compare(left.Conversion, right.Conversion);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.Language, right.Language);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.VersionControlProvenance.ListCompares(right.VersionControlProvenance, VersionControlDetailsComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.OriginalUriBaseIds.DictionaryCompares(right.OriginalUriBaseIds, ArtifactLocationComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Artifacts.ListCompares(right.Artifacts, ArtifactComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.LogicalLocations.ListCompares(right.LogicalLocations, LogicalLocationComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Graphs.ListCompares(right.Graphs, GraphComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Results.ListCompares(right.Results, ResultComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = RunAutomationDetailsComparer.Instance.Compare(left.AutomationDetails, right.AutomationDetails);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.RunAggregates.ListCompares(right.RunAggregates, RunAutomationDetailsComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.BaselineGuid, right.BaselineGuid);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.RedactionTokens.ListCompares(right.RedactionTokens);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.DefaultEncoding, right.DefaultEncoding);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = string.Compare(left.DefaultSourceLanguage, right.DefaultSourceLanguage);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.NewlineSequences.ListCompares(right.NewlineSequences);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.ColumnKind.CompareTo(right.ColumnKind);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = ExternalPropertyFileReferencesComparer.Instance.Compare(left.ExternalPropertyFileReferences, right.ExternalPropertyFileReferences);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.ThreadFlowLocations.ListCompares(right.ThreadFlowLocations, ThreadFlowLocationComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Taxonomies.ListCompares(right.Taxonomies, ToolComponentComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Addresses.ListCompares(right.Addresses, AddressComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Translations.ListCompares(right.Translations, ToolComponentComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Policies.ListCompares(right.Policies, ToolComponentComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.WebRequests.ListCompares(right.WebRequests, WebRequestComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.WebResponses.ListCompares(right.WebResponses, WebResponseComparer.Instance);
            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = SpecialLocationsComparer.Instance.Compare(left.SpecialLocations, right.SpecialLocations);
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