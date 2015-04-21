using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinegar
{
	public interface IFeatureRunner
	{
		Task<bool> Run(IEnumerable<Feature> features, IProgress<FeatureRun> progress);
	}
}