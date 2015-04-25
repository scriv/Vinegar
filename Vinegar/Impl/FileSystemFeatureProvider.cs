using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Vinegar.Impl
{
	public class FileSystemFeatureProvider : IFeatureProvider
	{
		private readonly GherkinParser m_parser = new GherkinParser();
		private readonly DirectoryInfo m_workingDirectory = new DirectoryInfo(@"C:\temp\");

		public Task<bool> Save(Feature feature, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Feature>> GetFeatures(IProgress<string> progress)
		{
			return Task.Factory.StartNew(() => this.GetFeatureFiles(progress))
				.ContinueWith(t => this.GetFeatures(t, progress));
		}

		private FileInfo[] GetFeatureFiles(IProgress<string> progress)
		{
			FileInfo[] featureFiles = m_workingDirectory.GetFiles("*.feature", SearchOption.AllDirectories);
			progress.Report("Discovered " + featureFiles.Length + " features");

			return featureFiles;
		}

		private IEnumerable<Feature> GetFeatures(Task<FileInfo[]> task, IProgress<string> progress)
		{
			var features = new ConcurrentBag<Feature>();

			Parallel.ForEach(task.Result, file =>
			{
				Feature feature;

				if (this.TryLoadFeature(file, progress, out feature))
				{
					features.Add(feature);
				}
			});

			return (IEnumerable<Feature>)features;
		}

		private bool TryLoadFeature(FileInfo file, IProgress<string> progress, out Feature feature)
		{
			feature = null;
			string featureText = File.ReadAllText(file.FullName);

			if (m_parser.TryParse(featureText, out feature))
			{
				progress.Report("Loaded " + file.FullName);
			}
			else
			{
				progress.Report("Could not load " + file.FullName);
			}

			return feature != null;
		}
	}
}
