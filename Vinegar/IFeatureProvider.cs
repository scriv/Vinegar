using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinegar
{
    public interface IFeatureProvider
    {
		Task<bool> Save(Feature feature, IProgress<int> progress);
		Task<IEnumerable<Feature>> GetFeatures(IProgress<string> progress);
    }
}