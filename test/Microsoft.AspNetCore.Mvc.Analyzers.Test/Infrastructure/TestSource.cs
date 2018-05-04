// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Testing;

namespace Microsoft.AspNetCore.Mvc.Analyzers.Infrastructure
{
    public class TestSource
    {
        private const string MarkerStart = "/*MM";
        private const string MarkerEnd = "*/";
        private static readonly string ProjectDirectory = GetProjectDirectory();

        public IDictionary<string, DiagnosticResultLocation> MarkerLocations { get; } 
            = new Dictionary<string, DiagnosticResultLocation>(StringComparer.Ordinal);

        public DiagnosticResultLocation? DefaultMarkerLocation { get; private set; }

        public string Source { get; private set; }

        public static TestSource Read(string testClassName, string testMethod)
        {
            var filePath = Path.Combine(ProjectDirectory, "TestFiles", testClassName, testMethod + ".cs");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"TestFile {testMethod} could not be found at {filePath}.", filePath);
            }

            var testInput = new TestSource();
            var lines = File.ReadAllLines(filePath);
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var markerStartIndex = line.IndexOf(MarkerStart, StringComparison.Ordinal);
                if (markerStartIndex != -1)
                {
                    var markerEndIndex = line.IndexOf(MarkerEnd, markerStartIndex, StringComparison.Ordinal);
                    var markerName = line.Substring(markerStartIndex + 2, markerEndIndex - markerStartIndex - 2);
                    var markerLocation = new DiagnosticResultLocation(i + 1, markerStartIndex + 1);
                    if (testInput.DefaultMarkerLocation == null)
                    {
                        testInput.DefaultMarkerLocation = markerLocation;
                    }

                    testInput.MarkerLocations.Add(markerName, markerLocation);
                    line = line.Substring(0, markerStartIndex) + line.Substring(markerEndIndex + MarkerEnd.Length);
                }

                lines[i] = line;
            }

            testInput.Source = string.Join(Environment.NewLine, lines);
            return testInput;
        }

        private static string GetProjectDirectory()
        {
            var solutionDirectory = TestPathUtilities.GetSolutionRootDirectory("Mvc");
            var assemblyName = typeof(TestSource).Assembly.GetName().Name;
            var projectDirectory = Path.Combine(solutionDirectory, "test", assemblyName);
            return projectDirectory;
        }
    }
}
