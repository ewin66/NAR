﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using log4net;
using Palmmedia.ReportGenerator.Parser;
using Palmmedia.ReportGenerator.Parser.Analysis;
using Palmmedia.ReportGenerator.Properties;
using Palmmedia.ReportGenerator.Reporting.Rendering;

namespace Palmmedia.ReportGenerator.Reporting
{
    /// <summary>
    /// Converts a coverage report generated by PartCover, OpenCover or NCover into a readable report.
    /// In contrast to the XSLT-Transformation included in PartCover, the report is more detailed.
    /// It does not only show the coverage quota, but also includes the source code and visualizes which line has been covered.
    /// </summary>
    public class ReportBuilder
    {
        /// <summary>
        /// The Logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReportBuilder));

        /// <summary>
        /// The parser to use.
        /// </summary>
        private readonly IParser parser;

        /// <summary>
        /// The renderer factory to use.
        /// </summary>
        private readonly IRendererFactory rendererFactory;

        /// <summary>
        /// The directory where the generated report should be saved.
        /// </summary>
        private readonly string targetDirectory;

        /// <summary>
        /// The assembly filter.
        /// </summary>
        private readonly IAssemblyFilter assemblyFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportBuilder"/> class.
        /// </summary>
        /// <param name="parser">The IParser to use.</param>
        /// <param name="rendererFactory">The renderer factory.</param>
        /// <param name="targetDirectory">The directory where the generated report should be saved.</param>
        /// <param name="assemblyFilter">The assembly filter.</param>
        public ReportBuilder(IParser parser, IRendererFactory rendererFactory, string targetDirectory, IAssemblyFilter assemblyFilter)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }

            if (rendererFactory == null)
            {
                throw new ArgumentNullException("rendererFactory");
            }

            if (targetDirectory == null)
            {
                throw new ArgumentNullException("targetDirectory");
            }

            if (assemblyFilter == null)
            {
                throw new ArgumentNullException("assemblyFilter");
            }

            this.parser = parser;
            this.rendererFactory = rendererFactory;
            this.targetDirectory = targetDirectory;
            this.assemblyFilter = assemblyFilter;
        }

        /// <summary>
        /// Starts the generation of the report.
        /// </summary>
        public void CreateReport()
        {
            var assemblies = this.parser.Assemblies.Where(a => this.assemblyFilter.IsAssemblyIncludedInReport(a.Name));

            int numberOfClasses = assemblies.Sum(a => a.Classes.Count());

            Logger.InfoFormat(Resources.AnalyzingClasses, numberOfClasses);

            int counter = 0;

            int degreeOfParallelism = this.rendererFactory.SupportsParallelClassReports ? Environment.ProcessorCount : 1;

            assemblies
                .AsParallel()
                .WithDegreeOfParallelism(degreeOfParallelism)
                .ForAll((assembly) =>
                {
                    assembly.Classes
                        .AsParallel()
                        .WithDegreeOfParallelism(degreeOfParallelism)
                        .ForAll((@class) =>
                        {
                            int current = Interlocked.Increment(ref counter);

                            Logger.DebugFormat(
                                CultureInfo.InvariantCulture,
                                " " + Resources.CreatingReport,
                                current,
                                numberOfClasses,
                                @class.Assembly.ShortName,
                                @class.Name);

                            this.CreateClassReport(@class);
                        });
                });

            this.CreateSummary(assemblies);
        }

        /// <summary>
        /// Creates the summary showing a overview of all assemblies and classes.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        private void CreateSummary(IEnumerable<Assembly> assemblies)
        {
            Logger.Debug(" " + Resources.CreatingSummary);

            int coveredLines = assemblies.Sum(a => a.CoveredLines);
            int coverableLines = assemblies.Sum(a => a.CoverableLines);
            decimal coverage = coverableLines == 0 ? 0 : (decimal)Math.Truncate(1000 * (double)coveredLines / (double)coverableLines) / 10;

            var summaryRenderer = this.rendererFactory.CreateSummaryRenderer();

            summaryRenderer.BeginSummaryReport(this.targetDirectory, Resources.Summary);
            summaryRenderer.Header(Resources.Summary);

            summaryRenderer.BeginKeyValueTable();
            summaryRenderer.KeyValueRow(Resources.GeneratedOn, DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString());
            summaryRenderer.KeyValueRow(Resources.Parser, this.parser.ToString());
            summaryRenderer.KeyValueRow(Resources.Assemblies2, assemblies.Count().ToString(CultureInfo.InvariantCulture));
            summaryRenderer.KeyValueRow(Resources.Files2, assemblies.SelectMany(a => a.Classes).SelectMany(a => a.Files).Distinct().Count().ToString(CultureInfo.InvariantCulture));
            summaryRenderer.KeyValueRow(Resources.Coverage2, coverage.ToString(CultureInfo.InvariantCulture) + "%");
            summaryRenderer.KeyValueRow(Resources.CoveredLines, coveredLines.ToString(CultureInfo.InvariantCulture));
            summaryRenderer.KeyValueRow(Resources.CoverableLines, coverableLines.ToString(CultureInfo.InvariantCulture));
            summaryRenderer.KeyValueRow(Resources.TotalLines, assemblies.Sum(a => a.TotalLines).GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
            summaryRenderer.FinishTable();

            summaryRenderer.Header(Resources.Assemblies);

            if (assemblies.Any())
            {
                summaryRenderer.BeginSummaryTable();

                foreach (var assembly in assemblies)
                {
                    summaryRenderer.SummaryAssembly(assembly);

                    foreach (var @class in assembly.Classes)
                    {
                        summaryRenderer.SummaryClass(@class);
                    }
                }

                summaryRenderer.FinishTable();
            }
            else
            {
                summaryRenderer.Paragraph("No assemblies have been covered.");
            }

            summaryRenderer.SaveSummaryReport(this.targetDirectory);
        }

        /// <summary>
        /// Creates the report for the given class.
        /// </summary>
        /// <param name="class">The class.</param>
        private void CreateClassReport(Class @class)
        {
            var classRenderer = this.rendererFactory.CreateClassRenderer();

            var fileAnalyses = @class.Files.Select(f => f.AnalyzeFile()).ToArray();

            classRenderer.BeginClassReport(this.targetDirectory, @class.Assembly.ShortName, @class.Name);

            classRenderer.Header(Resources.Summary);

            classRenderer.BeginKeyValueTable();
            classRenderer.KeyValueRow(Resources.Class, @class.Name);
            classRenderer.KeyValueRow(Resources.Assembly, @class.Assembly.ShortName);
            classRenderer.KeyValueRow(Resources.Files3, @class.Files.Select(f => f.Path));
            classRenderer.KeyValueRow(Resources.Coverage2, @class.CoverageQuota.ToString(CultureInfo.InvariantCulture) + "%");
            classRenderer.KeyValueRow(Resources.CoveredLines, @class.CoveredLines.ToString(CultureInfo.InvariantCulture));
            classRenderer.KeyValueRow(Resources.CoverableLines, @class.CoverableLines.ToString(CultureInfo.InvariantCulture));
            classRenderer.KeyValueRow(Resources.TotalLines, @class.TotalLines.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
            classRenderer.FinishTable();

            var metrics = @class.MethodMetrics;

            if (metrics.Any())
            {
                classRenderer.Header(Resources.Metrics);

                classRenderer.BeginMetricsTable(Enumerable.Repeat(Resources.Method, 1).Union(metrics.First().Metrics.Select(m => m.Name)));

                foreach (var metric in metrics)
                {
                    classRenderer.MetricsRow(metric);
                }

                classRenderer.FinishTable();
            }

            classRenderer.Header(Resources.Files);

            if (fileAnalyses.Length > 0)
            {
                var testMethods = @class.Files
                    .SelectMany(f => f.TestMethods)
                    .Distinct()
                    .OrderBy(l => l.ShortName);

                classRenderer.TestMethods(testMethods);

                foreach (var fileAnalysis in fileAnalyses)
                {
                    classRenderer.File(fileAnalysis.Path);

                    if (!string.IsNullOrEmpty(fileAnalysis.Error))
                    {
                        classRenderer.Paragraph(fileAnalysis.Error);
                    }
                    else
                    {
                        classRenderer.BeginLineAnalysisTable(new[] { string.Empty, "#", Resources.Line, Resources.Coverage });

                        foreach (var line in fileAnalysis.Lines)
                        {
                            classRenderer.LineAnalysis(line);
                        }

                        classRenderer.FinishTable();
                    }
                }
            }
            else
            {
                classRenderer.Paragraph(Resources.NoFilesFound);
            }

            classRenderer.SaveClassReport(this.targetDirectory, @class.Assembly.ShortName, @class.Name);
        }
    }
}