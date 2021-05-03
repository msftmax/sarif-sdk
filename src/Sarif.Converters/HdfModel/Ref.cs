﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.CodeAnalysis.Sarif.Converters.HdfModel
{
    public partial struct Ref
    {
        public List<Dictionary<string, object>> AnythingMapArray;
        public string String;

        public static implicit operator Ref(List<Dictionary<string, object>> AnythingMapArray)
        {
            return new Ref { AnythingMapArray = AnythingMapArray };
        }

        public static implicit operator Ref(string String)
        {
            return new Ref { String = String };
        }
    }
}
