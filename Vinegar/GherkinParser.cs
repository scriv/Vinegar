using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinegar
{
	public class GherkinParser
	{
		public GherkinParser()
		{
		}

		public bool TryParse(string featureText, out Feature feature)
		{
			feature = new Feature();
			var context = ParserContext.None;
			int lineIndex = 0;
			List<string> tags = new List<string>();
			StepType? lastStepType = null;

			foreach (string line in featureText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
			{
				try
				{
					string trimmedLine = line.TrimStart();

					if (trimmedLine.StartsWith("#", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}

					if (trimmedLine.StartsWith("@"))
					{
						tags.Clear();

						foreach (string tag in trimmedLine.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries))
						{
							tags.Add(tag.Trim());
						}

						continue;
					}

					if (trimmedLine.StartsWith("Feature:", StringComparison.OrdinalIgnoreCase))
					{
						this.RequireContext(context, ParserContext.None, lineIndex, line);

						feature.Title = line.Substring(line.IndexOf(":") + 1).Trim();
						feature.Tags = tags;
						context = ParserContext.Feature;

					}
					else if (trimmedLine.StartsWith("Scenario:", StringComparison.OrdinalIgnoreCase))
					{
						this.RequireContext(context, ParserContext.Feature, lineIndex, line);

						Scenario scenario = new Scenario();
						scenario.Tags = tags;
						scenario.Title = line.Substring(line.IndexOf(":") + 1).Trim();

						feature.Scenarios.Add(scenario);
						context = ParserContext.Scenario;
					}
					else if (trimmedLine.StartsWith("Given", StringComparison.OrdinalIgnoreCase)
						|| trimmedLine.StartsWith("When", StringComparison.OrdinalIgnoreCase)
						|| trimmedLine.StartsWith("Then", StringComparison.OrdinalIgnoreCase))
					{
						tags.Clear();
						this.RequireContext(context, ParserContext.Scenario, lineIndex, line);

						int seperatorIndex = trimmedLine.IndexOf(' ');
						StepType stepType = (StepType)Enum.Parse(typeof(StepType), trimmedLine.Substring(0, seperatorIndex), true);
						Scenario scenario = feature.Scenarios[feature.Scenarios.Count - 1];

						var step = new Step() { Type = stepType, Text = trimmedLine.Substring(seperatorIndex).Trim() };
						scenario.Steps.Add(step);

						lastStepType = stepType;
					}
					else if (trimmedLine.StartsWith("And", StringComparison.OrdinalIgnoreCase))
					{
						tags.Clear();
						this.RequireContext(context, ParserContext.Scenario, lineIndex, line);

						// TODO: Require existing step type

						int seperatorIndex = trimmedLine.IndexOf(' ');
						Scenario scenario = feature.Scenarios[feature.Scenarios.Count - 1];

						var step = new Step() { Type = lastStepType.GetValueOrDefault(), Text = trimmedLine.Substring(seperatorIndex).Trim() };
						scenario.Steps.Add(step);
					}
					else if (!string.IsNullOrWhiteSpace(trimmedLine))
					{
						tags.Clear();

						if (!string.IsNullOrEmpty(feature.Description))
						{
							feature.Description += Environment.NewLine;
						}

						feature.Description += trimmedLine.TrimEnd();
					}
				}
				finally
				{
					lineIndex++;
				}
			}

			return true;
		}

		private void RequireContext(ParserContext currentContext, ParserContext requiredContext, int line, string lineText)
		{
			if (currentContext != requiredContext)
			{
				throw new GherkinParserException(currentContext);
			}
		}

		internal enum ParserContext
		{
			None,
			Feature,
			Scenario
		}
	}
}