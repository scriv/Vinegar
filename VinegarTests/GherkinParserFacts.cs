using System.Linq;
using NUnit.Framework;
using Vinegar;

namespace VinegarTests
{
    public class GherkinParserFacts
    {
		[TestFixture]
		public class ParseMethod
		{
			[Test]
			public void Parses_feature_name()
			{
				// Arrange
				string featureText = "Feature: Test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Title, Is.EqualTo("Test"));
			}

			[Test]
			public void Parses_feature_description_multiline()
			{
				// Arrange
				string featureText = @"Feature: Test
									   In order to test this description,
									   as a developer
									   I want to write a test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Description, Is.EqualTo(@"In order to test this description,
as a developer
I want to write a test"));
			}

			[Test]
			public void Parses_feature_description_multiline_with_comment()
			{
				// Arrange
				string featureText = @"Feature: Test
									   In order to test this description,
									   as a developer
									   # Comment line 
									   I want to write a test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Description, Is.EqualTo(@"In order to test this description,
as a developer
I want to write a test"));
			}

			[Test]
			public void Parses_feature_description_singleline()
			{
				// Arrange
				string featureText = @"Feature: Test
									   A single line description";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Description, Is.EqualTo(@"A single line description"));
			}

			[Test]
			public void Parses_feature_tags()
			{
				// Arrange
				string featureText = @"@Test @Tag
									   Feature: Test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Tags.Count, Is.EqualTo(2));
				Assert.That(feature.Tags[0], Is.EqualTo("Test"));
				Assert.That(feature.Tags[1], Is.EqualTo("Tag"));
			}

			[Test]
			public void Parses_feature_tags_split_by_comment()
			{
				// Arrange
				string featureText = @"@Test @Tag
                                       # Rando comment
									   Feature: Test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Tags.Count, Is.EqualTo(2));
				Assert.That(feature.Tags[0], Is.EqualTo("Test"));
				Assert.That(feature.Tags[1], Is.EqualTo("Tag"));
			}

			[Test]
			public void Parses_scenario_tags()
			{
				// Arrange
				string featureText = @"Feature: Test

									   @Test @Tag
									   Scenario: Test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Scenario scenario = feature.Scenarios[0];

				Assert.That(scenario.Tags.Count, Is.EqualTo(2));
				Assert.That(scenario.Tags[0], Is.EqualTo("Test"));
				Assert.That(scenario.Tags[1], Is.EqualTo("Tag"));
			}

			[Test]
			public void Parses_scenario_tags_split_by_comment()
			{
				// Arrange
				string featureText = @"Feature: Test

									   @Test @Tag
									   # Rando comment
									   Scenario: Test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Scenario scenario = feature.Scenarios[0];

				Assert.That(scenario.Tags.Count, Is.EqualTo(2));
				Assert.That(scenario.Tags[0], Is.EqualTo("Test"));
				Assert.That(scenario.Tags[1], Is.EqualTo("Tag"));
			}

			[Test]
			public void Parses_scenario_name()
			{
				// Arrange
				string featureText = @"Feature: Test
									   Scenario: Test Scenario";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Scenarios.Count, Is.EqualTo(1), "A scenario was expected to be parsed");
				Assert.That(feature.Scenarios[0].Title, Is.EqualTo("Test Scenario"));
			}

			[Test]
			public void Parses_given_step()
			{
				// Arrange
				string featureText = @"Feature: Test
									   Scenario: Test Scenario
											Given I have a scenario with a single given";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Step step = feature.Scenarios[0].Steps[0];

				Assert.That(step.Type, Is.EqualTo(StepType.Given));
				Assert.That(step.Text, Is.EqualTo("I have a scenario with a single given"));
			}

			[Test]
			public void Parses_given_step_with_and()
			{
				// Arrange
				string featureText = @"Feature: Test
									   Scenario: Test Scenario
											Given I have a scenario with a single given
											And I also have an and";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Scenarios[0].Steps.Count, Is.EqualTo(2));

				Scenario scenario = feature.Scenarios[0];

				Assert.That(scenario.Steps[0].Type, Is.EqualTo(StepType.Given));
				Assert.That(scenario.Steps[0].Text, Is.EqualTo("I have a scenario with a single given"));

				Assert.That(scenario.Steps[1].Type, Is.EqualTo(StepType.Given));
				Assert.That(scenario.Steps[1].Text, Is.EqualTo("I also have an and"));
			}

			[Test]
			public void Parses_when_step()
			{
				// Arrange
				string featureText = @"Feature: Test
									   Scenario: Test Scenario
											When I run the test";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Step step = feature.Scenarios[0].Steps[0];

				Assert.That(step.Type, Is.EqualTo(StepType.When));
				Assert.That(step.Text, Is.EqualTo("I run the test"));
			}

			[Test]
			public void Parses_then_step()
			{
				// Arrange
				string featureText = @"Feature: Test
									   Scenario: Test Scenario
											Then The test should pass";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Step step = feature.Scenarios[0].Steps[0];

				Assert.That(step.Type, Is.EqualTo(StepType.Then));
				Assert.That(step.Text, Is.EqualTo("The test should pass"));
			}

			[Test]
			public void Parses_all_step_types_together()
			{
				// Arrange
				string featureText = @"Feature: Test
									   Scenario: Test Scenario
											Given I have a scenario with a single given
											When I run the test
											Then The test should pass";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Scenarios[0].Steps.Count, Is.EqualTo(3));

				Scenario scenario = feature.Scenarios[0];

				Assert.That(scenario.Steps[0].Type, Is.EqualTo(StepType.Given));
				Assert.That(scenario.Steps[0].Text, Is.EqualTo("I have a scenario with a single given"));

				Assert.That(scenario.Steps[1].Type, Is.EqualTo(StepType.When));
				Assert.That(scenario.Steps[1].Text, Is.EqualTo("I run the test"));

				Assert.That(scenario.Steps[2].Type, Is.EqualTo(StepType.Then));
				Assert.That(scenario.Steps[2].Text, Is.EqualTo("The test should pass"));
			}

			[Test]
			public void Parses_multiple_scenarios()
			{
				// Arrange
				string featureText = @"Feature: Test

									   Scenario: Test Scenario 1
											Given I have the first test
											When I assert
											Then It should be unique

									   Scenario: Test Scenario 2
											Given I have the second test
											When I assert
											Then It should be unique";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				Assert.That(feature.Scenarios.Count, Is.EqualTo(2));
				Assert.That(feature.Scenarios[0].Title == "Test Scenario 1");
				Assert.That(feature.Scenarios[1].Title == "Test Scenario 2");
			}

			[Test]
			public void Parses_DataTable()
			{
				// Arrange
				string featureText = @"Feature: Test

									   Scenario: Test Scenario 1
											Given I have a test
											When I assert
											Then It should have a data table:
												| Field | Value |
												| Age   | 29    |
												| Name  | Scriv |";
				var parser = new GherkinParser();

				// Act
				Feature feature;
				bool parseSuccess = parser.TryParse(featureText, out feature);

				// Assert
				DataTable table = feature.Scenarios.Last().Steps.Last().DataTable;

				Assert.That(table, Is.Not.Null);

				// Headers
				Assert.That(table.Headers.Count, Is.EqualTo(2));
				Assert.That(table.Headers[0], Is.EqualTo("Field"));
				Assert.That(table.Headers[1], Is.EqualTo("Value"));
				Assert.That(table.Rows.Count, Is.EqualTo(2));

				// First row
				Assert.That(table.Rows[0]["Field"], Is.EqualTo("Age"));
				Assert.That(table.Rows[0]["Value"], Is.EqualTo("29"));

				// Second row
				Assert.That(table.Rows[1]["Field"], Is.EqualTo("Name"));
				Assert.That(table.Rows[1]["Value"], Is.EqualTo("Scriv"));
			}
		}
    }
}
