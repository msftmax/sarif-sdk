﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

/// <summary>
/// Note: This comparer may not have all properties compared. Will be replaced by a comprehensive
/// comparer generated by JSchema as part of EqualityComparer in a planned comprehensive solution.
/// Tracking by issue: https://github.com/microsoft/jschema/issues/141
/// </summary>
namespace Microsoft.CodeAnalysis.Sarif.Comparers
{
    internal class ReportingConfigurationComparer : IComparer<ReportingConfiguration>
    {
        internal static readonly ReportingConfigurationComparer Instance = new ReportingConfigurationComparer();

        public int Compare(ReportingConfiguration left, ReportingConfiguration right)
        {
            int compareResult = 0;

            if (left.TryReferenceCompares(right, out compareResult))
            {
                return compareResult;
            }

            compareResult = left.Enabled.CompareTo(right.Enabled);

            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Level.CompareTo(right.Level);

            if (compareResult != 0)
            {
                return compareResult;
            }

            compareResult = left.Rank.CompareTo(right.Rank);

            if (compareResult != 0)
            {
                return compareResult;
            }

            // Note: There may be other properties are not compared.
            return compareResult;
        }
    }
}
