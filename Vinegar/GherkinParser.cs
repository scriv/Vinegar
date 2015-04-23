using System;
using System.Collections.Generic;

namespace Vinegar
{
	public class GherkinParser
	{
		private readonly ISectionParser[] m_parsers;

		public GherkinParser()
		{
			m_parsers = new ISectionParser[]
			{ 
				new CommentParser(), 
				new TagParser(), 
				new FeatureParser(), 
				new ScenarioParser(), 
				new StepParser(), 
				new FreeTextParser()
			};
		}

		public bool TryParse(string featureText, out Feature feature)
		{
			var state = new ParsingState();
			int lineIndex = 0;

			foreach (string line in featureText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
			{
				string trimmedLine = line.TrimStart();

				foreach (ISectionParser parser in m_parsers)
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
			public IList<string> CurrentTags { get; set; }
		}

		private interface ISectionParser
		{
			string[] LinePrefixes { get; }
			void Parse(string line, ParsingState parsingState);
		}

		private class FeatureParser : ISectionParser
		{
			public string[] LinePrefixes { get { return new[] { "Feature:" }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				parsingState.Feature = new Feature();
				parsingState.Feature.Title = line.Substring(line.IndexOf(":") + 1).Trim();
				parsingState.Feature.Tags = parsingState.CurrentTags;
			}
		}

		private class ScenarioParser : ISectionParser
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

		private class StepParser : ISectionParser
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

		private class CommentParser : ISectionParser
		{
			public string[] LinePrefixes { get { return new[] { "#" }; } }

			public void Parse(string line, ParsingState parsingState)
			{
				// No-op
			}
		}

		private class TagParser : ISectionParser
		{
			public string[] LinePrefixes { get { return new[] { "@" }; } }

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

		private class FreeTextParser : ISectionParser
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
	}
}