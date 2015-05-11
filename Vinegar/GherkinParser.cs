using System;
using System.Collections.Generic;
using System.Linq;

namespace Vinegar
{
	/// <summary>
	/// Parses Gherkin language text.
	/// </summary>
	public sealed class GherkinParser
	{
		private readonly ISyntaxParser[] m_parsers;

		/// <summary>
		/// Initializes a new instance of the <see cref="GherkinParser"/> class.
		/// </summary>
		public GherkinParser()
		{
			m_parsers = new ISyntaxParser[]
			{ 
				new CommentParser(), 
				new TagParser(), 
				new FeatureParser(), 
				new ScenarioParser(), 
				new StepParser(),
				new DataTableParser(),
				new FreeTextParser()
			};
		}

		/// <summary>
		/// Converts Gherkin language text to its <see cref="Feature"/> equivalent. A return value indicates whether the operation succeeded.
		/// </summary>
		/// <param name="featureText">The feature text.</param>
		/// <param name="feature">The feature.</param>
		/// <returns><c>true</c> if the parse succeeded; otherwise, <c>false</c>.</returns>
		public bool TryParse(string featureText, out Feature feature)
		{
			var state = new ParsingState();
			int lineIndex = 0;

			foreach (string line in featureText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
			{
				string trimmedLine = line.TrimStart();

				foreach (ISyntaxParser parser in m_parsers)
				{
					bool parsed = false;

					foreach (string prefix in parser.LinePrefixes)
					{
						if (trimmedLine.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
						{
							parser.Parse(trimmedLine, state);
							parsed = true;
							break;
						}
					}

					if (parsed) break;
				}

				lineIndex++;
			}

			feature = state.Feature;
			return true;
		}

		private class ParsingState
		{
			public Feature Feature { get; set; }
			public Scenario CurrentScenario { get; set; }
			public Step CurrentStep { get; set; }
			public List<string> CurrentTags { get; set; }
		}

		/// <summary>
		/// Defines the parser behaviour for lines of Gherkin syntax.
		/// </summary>
		private interface ISyntaxParser
		{
			/// <summary>
			/// Gets the line prefixes that this parser should handle.
			/// </summary>
			/// <value>
			/// The line prefixes.
			/// </value>
			string[] LinePrefixes { get; }

			/// <summary>
			/// Parses the specified line.
			/// </summary>
			/// <param name="line">The line.</param>
			/// <param name="parsingState">State of the parsing.</param>
			void Parse(string line, ParsingState parsingState);
		}

		private class FeatureParser : ISyntaxParser
		{
			public string[] LinePrefixes { get { return new[] { "Feature:" }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				parsingState.Feature = new Feature();
				parsingState.Feature.Title = line.Substring(line.IndexOf(":") + 1).Trim();
				parsingState.Feature.Tags = parsingState.CurrentTags;
			}
		}

		private class ScenarioParser : ISyntaxParser
		{
			public string[] LinePrefixes { get { return new[] { "Scenario:" }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				Scenario scenario = new Scenario();
				scenario.Tags = parsingState.CurrentTags;
				scenario.Title = line.Substring(line.IndexOf(":") + 1).Trim();

				parsingState.Feature.Scenarios.Add(scenario);
				parsingState.CurrentScenario = scenario;
			}
		}

		private class StepParser : ISyntaxParser
		{
			private const string andStepName = "And";

			public string[] LinePrefixes { get { return new[] { "Given", "When", "Then", andStepName }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				int seperatorIndex = line.IndexOf(' ');
				StepType stepType;

				if (line.StartsWith(andStepName, StringComparison.OrdinalIgnoreCase))
				{
					stepType = parsingState.CurrentScenario.Steps[parsingState.CurrentScenario.Steps.Count - 1].Type;
				}
				else
				{
					stepType = (StepType)Enum.Parse(typeof(StepType), line.Substring(0, seperatorIndex), true);
				}

				Scenario scenario = parsingState.CurrentScenario;

				var step = new Step() { Type = stepType, Text = line.Substring(seperatorIndex).Trim() };
				scenario.Steps.Add(step);
				parsingState.CurrentStep = step;
			}
		}

		/// <summary>
		/// Parses comment lines (lines that start with #).
		/// </summary>
		private class CommentParser : ISyntaxParser
		{
			/// <summary>
			/// Gets the line prefixes that this parser should handle.
			/// </summary>
			/// <value>
			/// The line prefixes.
			/// </value>
			public string[] LinePrefixes { get { return new[] { "#" }; } }

			/// <summary>
			/// Parses the specified line.
			/// </summary>
			/// <param name="line">The line.</param>
			/// <param name="parsingState">State of the parsing.</param>
			public void Parse(string line, ParsingState parsingState)
			{
				// No-op
			}
		}

		/// <summary>
		/// Parses tags (@tag1 @tag2) proceeding features and steps.
		/// </summary>
		private class TagParser : ISyntaxParser
		{
			/// <summary>
			/// Gets the line prefixes that this parser should handle.
			/// </summary>
			/// <value>
			/// The line prefixes.
			/// </value>
			public string[] LinePrefixes { get { return new[] { "@" }; } }

			/// <summary>
			/// Parses the specified line.
			/// </summary>
			/// <param name="line">The line.</param>
			/// <param name="parsingState">State of the parsing.</param>
			public void Parse(string line, ParsingState parsingState)
			{
				var tags = new List<string>();

				foreach (string tag in line.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
				{
					tags.Add(tag.Trim());
				}

				parsingState.CurrentTags = tags;
			}
		}

		private class FreeTextParser : ISyntaxParser
		{
			public string[] LinePrefixes { get { return new[] { string.Empty }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				if (!string.IsNullOrWhiteSpace(parsingState.Feature.Description))
				{
					parsingState.Feature.Description += Environment.NewLine;
				}

				parsingState.Feature.Description += line.Trim();
			}
		}

		private class DataTableParser : ISyntaxParser
		{
			public string[] LinePrefixes { get { return new[] { "|" }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				if (parsingState.CurrentStep.DataTable == null)
				{
					parsingState.CurrentStep.DataTable = new DataTable();
					parsingState.CurrentStep.DataTable.Headers = ParseRow(line);
				}
				else
				{
					DataTableRow row = new DataTableRow(parsingState.CurrentStep.DataTable) { Cells = ParseRow(line) };
					parsingState.CurrentStep.DataTable.Rows.Add(row);
				}
			}

			private static List<string> ParseRow(string line)
			{
				var cells = new List<string>();

				foreach (string cell in line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (!string.IsNullOrWhiteSpace(cell))
					{
						cells.Add(cell.Trim());
					}
				}

				return cells;
			}
		}
	}
}