using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SharpSvn;

namespace Vinegar.Impl
{
	//Export(typeof(IFeatureProvider))]
	public class SvnFeatureProvider : IFeatureProvider
	{
		private SvnTarget m_target;
		private readonly GherkinParser m_parser = new GherkinParser();

		public SvnFeatureProvider()
		{
			m_target = SvnTarget.FromString("https://svn.starrez.com");
		}

		public string Name { get { return "Subversion"; } }

		public Task<bool> Save(Feature feature, IProgress<int> progress)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Feature>> GetFeatures(IProgress<string> progress)
		{
			return Task.Factory.StartNew(() => this.GetFeatureFiles(progress))
				.ContinueWith(t => this.GetFeatures(t, progress));
		}

		private SvnListEventArgs[] GetFeatureFiles(IProgress<string> progress)
		{
			using (var client = new SvnClient())
			{
				progress.Report("Connected to SVN");

				Collection<SvnListEventArgs> files;

				if (client.GetList(m_target, out files))
				{
					SvnListEventArgs[] featureFiles = files.Where(f => Path.GetExtension(f.Path) == ".feature").ToArray();

					progress.Report("Discovered " + featureFiles.Length + " features");

					return featureFiles;
				}
				else
				{
					throw new Exception();
				}
			}
		}

		private IEnumerable<Feature> GetFeatures(Task<SvnListEventArgs[]> task, IProgress<string> progress)
		{
			var features = new ConcurrentBag<Feature>();

			using (var client = new SvnClient())
			{
				Parallel.ForEach(task.Result, e =>
				{
					Feature feature;
					
					if (this.TryLoadFeature(e, client, progress, out feature))
					{
						features.Add(feature);
					}
				});
			}

			return (IEnumerable<Feature>)features;
		}

		private bool TryLoadFeature(SvnListEventArgs file, SvnClient client, IProgress<string> progress, out Feature feature)
		{
			feature = null;

			using (var memoryStream = new MemoryStream())
			{
				var target = SvnTarget.FromUri(file.Uri);

				if (client.Write(target, memoryStream))
				{
					using (var reader = new StreamReader(memoryStream))
					{
						string featureText = reader.ReadToEnd();

						if (m_parser.TryParse(featureText, out feature))
						{
							progress.Report("Loaded " + Path.GetFileName(file.Path));
						}
						else
						{
							progress.Report("Could not load " + Path.GetFileName(file.Path));
						}
					}
				}
				else
				{
					progress.Report("Could not load " + Path.GetFileName(file.Path));
				}
			}

			return feature != null;
		}
	}
}