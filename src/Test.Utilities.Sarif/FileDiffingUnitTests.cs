﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using FluentAssertions;

using Microsoft.CodeAnalysis.Sarif.Readers;
using Microsoft.CodeAnalysis.Sarif.VersionOne;
using Microsoft.CodeAnalysis.Sarif.Writers;
using Microsoft.CodeAnalysis.Test.Utilities.Sarif;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Sarif
{
    public abstract class FileDiffingUnitTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly bool _testProducesSarifCurrentVersion;
        private readonly TestAssetResourceExtractor _resourceExtractor;

        public FileDiffingUnitTests(ITestOutputHelper outputHelper, bool testProducesSarifCurrentVersion = true)
        {
            _outputHelper = outputHelper;
            _testProducesSarifCurrentVersion = testProducesSarifCurrentVersion;
            _resourceExtractor = new TestAssetResourceExtractor(this.GetType());

            Directory.CreateDirectory(TestOutputDirectory);
        }

        protected virtual Assembly ThisAssembly => this.GetType().Assembly;

        /// <summary>
        /// The name of the type under test, typically derived form the test class name, e.g.:
        ///     MergeCommand (derived from observing a test class named 'MergeCommandTests')
        /// </summary>
        protected virtual string TypeUnderTest => this.GetType().Name.Substring(0, this.GetType().Name.Length - "Tests".Length);

        /// <summary>
        /// A specific unit test output directory, to store baseline ('expected') files and actual, etc., e.g.:
        /// D:\src\sarif-sdk\bld\bin\AnyCPU_Debug\Test.UnitTests.Sarif.Multitool.Library\netcoreapp3.1\UnitTestOutput.MergeCommand
        /// </summary>
        protected virtual string TestOutputDirectory => Path.Combine(GetBuildPath(), "UnitTestOutput." + TypeUnderTest);

        /// <summary>
        /// The root directory of a 'product', a project under test. Typically the \src\ dir that holds the solution, e.g.:
        /// 
        ///     D:\src\sarif-sdk\src\
        /// </summary>
        protected virtual string ProductRootDirectory => DirectoryHelpers.GetEnlistmentRoot();

        /// <summary>
        /// The directory at the root of the project which stores shared test assets (i.e.
        /// files which may be consumed by unit tests for multiple test binaries), e.g.:
        /// 
        ///     D:\src\sarif-sdk\src\TestData
        /// </summary>
        protected virtual string ProductTestDataDirectory => @$"{ProductRootDirectory}\TestData\";

        /// <summary>
        /// Returns the test binary name without file extensions, e.g.:
        /// 
        ///     Test.UnitTests.Sarif.Multitool.Library
        /// </summary>
        protected virtual string TestBinaryName => Path.GetFileNameWithoutExtension(ThisAssembly.Location);

        /// <summary>
        /// A directory at the root of a test binary which stores tests assets only for its tests, e.g.:
        /// 
        ///     D:\src\sarif-sdk\src\Test.UnitTests.Sarif.Multitool.Library\TestData
        /// </summary>
        protected virtual string TestBinaryTestDataDirectory => @$"{ProductRootDirectory}\{TestBinaryName}\TestData\";

        /// <summary>
        /// A root namespace from which to retrieve test resources. This value is generated by the RESX compiler
        /// from the default namespace of the binary and the directory structure that contains the embedded resource. E.g.:
        /// 
        ///     Microsoft.CodeAnalysis.Test.UnitTests.Sarif.Multitool.TestData.
        /// </summary>
        protected virtual string TestLogResourceNameRoot => @$"{TestBinaryName}.TestData.{TypeUnderTest}";

        public string GetExpectedOutputFileFromResource(string resourceName)
            => GetResourceText(GetFullResourcePathForInputResource(resourceName));
        
        public string GetInputSarifTextFromResource(string resourceName)
            => GetResourceText(GetFullResourcePathForExpectedOutputResource(resourceName));

        private string GetFullResourcePathForInputResource(string resourceName)
        {
            return $"{TestLogResourceNameRoot}.ExpectedOutputs.{resourceName}";
        }

        private string GetFullResourcePathForExpectedOutputResource(string resourceName)
        {
            return $"{TestLogResourceNameRoot}.Inputs.{resourceName}";
        }

        private string GetBuildPath()
        {
            string path = typeof(FileDiffingUnitTests).Assembly.Location;
            return Path.GetDirectoryName(path);
        }

        // Most tests convert a single input file to a single output file, but some have multiple
        // inputs and/or multiple outputs. Derived test classes must implement one or the other of
        // the methods ConstructTestOutputFromInputResource (which handles the one-to-one case) and
        // ConstructTestOutputsFromInputResources (which handles the many-to-many case). These
        // methods can't (or at least, shouldn't) both be abstract because derived test classes
        // shouldn't have to supply even an empty implementation of the one that they don't use.
        // Therefore this class implements both of those methods to throw NotImplementedException
        // so you don't accidentally call the wrong one.
        protected virtual string ConstructTestOutputFromInputResource(string inputResourceName, object parameter)
            => throw new NotImplementedException(nameof(ConstructTestOutputFromInputResource));

        protected virtual IDictionary<string, string> ConstructTestOutputsFromInputResources(IEnumerable<string> inputResourceNames, object parameter)
            => throw new NotImplementedException(nameof(ConstructTestOutputsFromInputResources));

        protected virtual void RunTest(string inputResourceName,
                                       string expectedOutputResourceName = null,
                                       object parameter = null,
                                       bool enforceNotificationsFree = false)
        {
            // In the simple case of one input file and one output file, the output resource name
            // can be inferred from the input resource name. In the case of arbitrary numbers of
            // input and output files (the other overload of RunTest), the output resource names
            // must be specified explicitly.
            expectedOutputResourceName = expectedOutputResourceName ?? inputResourceName;

            // When we retrieve test input content, we use the resource name as the test specified it.
            // But when we construct the names of the actual and expected output files, we ensure that
            // the name has the ".sarif" extension. This is necessary for testing classes such as the
            // Fortify converter whose input is not SARIF. In the other overload of RunTest, this is
            // not necessary because, again, the output resource names are specified explicitly.
            expectedOutputResourceName = Path.GetFileNameWithoutExtension(expectedOutputResourceName) + SarifConstants.SarifFileExtension;
            string expectedSarifText = GetExpectedOutputFileFromResource(expectedOutputResourceName);

            string actualSarifText = ConstructTestOutputFromInputResource(inputResourceName, parameter);

            // The comparison code is shared between this one-input-to-one-output method and the
            // overload that takes multiple inputs and multiple outputs. So set up the lists and
            // dictionaries that the shared comparison method expects.
            var inputResourceNames = new List<string> { inputResourceName };

            const string SingleOutputDictionaryKey = "single";

            var expectedOutputResourceNameDictionary = new Dictionary<string, string>
            {
                [SingleOutputDictionaryKey] = expectedOutputResourceName
            };

            var expectedSarifTexts = new Dictionary<string, string>
            {
                [SingleOutputDictionaryKey] = expectedSarifText
            };

            var actualSarifTexts = new Dictionary<string, string>
            {
                [SingleOutputDictionaryKey] = actualSarifText
            };

            CompareActualToExpected(inputResourceNames,
                                    expectedOutputResourceNameDictionary,
                                    expectedSarifTexts,
                                    actualSarifTexts,
                                    enforceNotificationsFree);
        }

        protected virtual void RunTest(IList<string> inputResourceNames,
                                       IDictionary<string, string> expectedOutputResourceNames,
                                       object parameter = null,
                                       bool enforceNotificationsFree = false)
        {
            var expectedSarifTexts = expectedOutputResourceNames.ToDictionary(
                pair => pair.Key,
                pair => GetExpectedOutputFileFromResource(pair.Value));

            IEnumerable<string> fullInputResourceNames = inputResourceNames.Select(name => GetFullResourcePathForInputResource(name));

            IDictionary<string, string> actualSarifTexts = ConstructTestOutputsFromInputResources(fullInputResourceNames, parameter);

            CompareActualToExpected(inputResourceNames,
                                    expectedOutputResourceNames,
                                    expectedSarifTexts,
                                    actualSarifTexts,
                                    enforceNotificationsFree);
        }

        private void CompareActualToExpected(
            IList<string> inputResourceNames,
            IDictionary<string, string> expectedOutputResourceNameDictionary,
            IDictionary<string, string> expectedSarifTextDictionary,
            IDictionary<string, string> actualSarifTextDictionary,
            bool enforceNotificationsFree)
        {
            if (inputResourceNames.Count == 0)
            {
                throw new ArgumentException("No input resources were specified", nameof(inputResourceNames));
            }

            if (expectedOutputResourceNameDictionary.Count == 0)
            {
                throw new ArgumentException("No expected output resources were specified", nameof(expectedOutputResourceNameDictionary));
            }

            if (expectedSarifTextDictionary.Count != expectedOutputResourceNameDictionary.Count)
            {
                throw new ArgumentException($"The number of expected output files ({expectedSarifTextDictionary.Count}) does not match the number of expected output resources {expectedOutputResourceNameDictionary.Count}");
            }

            if (expectedSarifTextDictionary.Count != actualSarifTextDictionary.Count)
            {
                throw new ArgumentException($"The number of actual output files ({actualSarifTextDictionary.Count}) does not match the number of expected output files {expectedSarifTextDictionary.Count}");
            }

            bool passed = true;
            var filesWithErrors = new List<string>();

            // Reify the list of keys because we're going to modify the dictionary in place.
            List<string> keys = expectedSarifTextDictionary.Keys.ToList();

            foreach (string key in keys)
            {
                if (_testProducesSarifCurrentVersion)
                {
                    PrereleaseCompatibilityTransformer.UpdateToCurrentVersion(expectedSarifTextDictionary[key], Formatting.Indented, out string transformedSarifText);
                    expectedSarifTextDictionary[key] = transformedSarifText;

                    passed &= AreEquivalent<SarifLog>(actualSarifTextDictionary[key],
                                                      expectedSarifTextDictionary[key],
                                                      out SarifLog actual);

                    if (enforceNotificationsFree &&
                        actual != null &&
                        (actual.Runs[0].Invocations?[0].ToolExecutionNotifications != null ||
                         actual.Runs[0].Invocations?[0].ToolConfigurationNotifications != null))
                    {
                        passed = false;
                        filesWithErrors.Add(key);
                    }
                }
                else
                {
                    passed &= AreEquivalent<SarifLogVersionOne>(
                        actualSarifTextDictionary[key],
                        expectedSarifTextDictionary[key],
                        out SarifLogVersionOne actual,
                        SarifContractResolverVersionOne.Instance);
                }
            }

            string expectedRootDirectory = null;
            string actualRootDirectory = null;

            bool firstKey = true;
            foreach (string key in expectedOutputResourceNameDictionary.Keys)
            {
                string expectedFilePath = GetOutputFilePath("ExpectedOutputs", expectedOutputResourceNameDictionary[key]);
                string actualFilePath = GetOutputFilePath("ActualOutputs", expectedOutputResourceNameDictionary[key]);

                if (firstKey)
                {
                    expectedRootDirectory = Path.GetDirectoryName(expectedFilePath);
                    actualRootDirectory = Path.GetDirectoryName(actualFilePath);

                    Directory.CreateDirectory(expectedRootDirectory);
                    Directory.CreateDirectory(actualRootDirectory);

                    firstKey = false;
                }

                File.WriteAllText(expectedFilePath, expectedSarifTextDictionary[key]);
                File.WriteAllText(actualFilePath, actualSarifTextDictionary[key]);
            }

            StringBuilder sb = null;

            if (!passed)
            {
                string errorMessage = string.Empty;

                if (filesWithErrors.Count > 0)
                {
                    errorMessage =
                        "one or more files contain an unexpected notification (which likely " +
                        "indicates that an unhandled exception was encountered at analysis time): " +
                        Environment.NewLine +
                        string.Join(Environment.NewLine, filesWithErrors) +
                        Environment.NewLine + Environment.NewLine;
                }

                errorMessage += string.Format(@"there should be no unexpected diffs detected comparing actual results to '{0}'.", string.Join(", ", inputResourceNames));
                sb = new StringBuilder(errorMessage);

                sb.AppendLine("To compare all difference for this test suite:");
                sb.AppendLine(GenerateDiffCommand(TypeUnderTest, expectedRootDirectory, actualRootDirectory) + Environment.NewLine);

                sb.AppendLine(
                    "To rebaseline with current behavior:");

                // TODO I suspect this baseline command works in some contexts (when we have a specific type under
                // test) but breaks the general case of rebaselining some tests that are global to the project, 
                // such as in the BinSkim project.
                string testDirectory = Path.Combine(TestBinaryTestDataDirectory, TypeUnderTest, "ExpectedOutputs");
                sb.AppendLine(GenerateRebaselineCommand(TypeUnderTest, testDirectory, actualRootDirectory));
            }

            ValidateResults(sb?.ToString());
        }

        protected string GetOutputFilePath(string directory, string resourceName)
        {
            string fileName = Path.GetFileNameWithoutExtension(resourceName) + SarifConstants.SarifFileExtension;
            return Path.Combine(TestOutputDirectory, directory, fileName);
        }

        protected virtual string GetResourceText(string resourcePath)
        {
            return _resourceExtractor.GetResourceText(resourcePath);
        }

        protected byte[] GetResourceBytes(string resourcePath)
        {
            return _resourceExtractor.GetResourceBytes(resourcePath);
        }

        protected void ValidateResults(string output)
        {
            if (!string.IsNullOrEmpty(output))
            {
                _outputHelper.WriteLine(output);
                output.Length.Should().Be(0, because: output);
            }
        }

        public static string GenerateRebaselineCommand(string suiteName, string expected, string actual)
        {
            actual = Path.GetFullPath(actual);
            expected = Path.GetFullPath(expected);

            Directory.CreateDirectory(expected);

            string diffText = string.Format(CultureInfo.InvariantCulture, "xcopy /Y \"{0}\" \"{1}\"", actual, expected);

            string qualifier = string.Empty;

            if (File.Exists(expected))
            {
                qualifier = Path.GetFileNameWithoutExtension(expected);
            }

            string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            fullPath = Path.Combine(fullPath, "Rebaseline" + suiteName + qualifier + ".cmd");

            File.WriteAllText(fullPath, diffText);
            return fullPath;
        }


        public static string GenerateDiffCommand(string suiteName, string expected, string actual)
        {
            actual = Path.GetFullPath(actual);
            expected = Path.GetFullPath(expected);

            string diffText = string.Format(CultureInfo.InvariantCulture, "%DIFF% \"{0}\" \"{1}\"", expected, actual);

            string qualifier = string.Empty;

            if (File.Exists(expected))
            {
                qualifier = Path.GetFileNameWithoutExtension(expected);
            }

            string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            fullPath = Path.Combine(fullPath, "Diff" + suiteName + qualifier + ".cmd");

            File.WriteAllText(fullPath, diffText);
            return fullPath;
        }

        public static bool AreEquivalent<T>(string actualSarif,
                                            string expectedSarif,
                                            out T actualObject,
                                            IContractResolver contractResolver = null)
        {
            actualObject = default;

            expectedSarif = expectedSarif ?? "{}";
            JToken expectedToken = JsonConvert.DeserializeObject<JToken>(expectedSarif);

            JToken actualToken = JsonConvert.DeserializeObject<JToken>(actualSarif);
            if (!JToken.DeepEquals(actualToken, expectedToken)) { return false; }

            // Make sure we can successfully roundtrip what was just generated.
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };

            actualObject = JsonConvert.DeserializeObject<T>(actualSarif, settings);
            string roundTrippedSarif = JsonConvert.SerializeObject(actualObject, settings);

            JToken roundTrippedToken = JsonConvert.DeserializeObject<JToken>(roundTrippedSarif);
            return (JToken.DeepEquals(actualToken, roundTrippedToken));
        }        
    }
}
