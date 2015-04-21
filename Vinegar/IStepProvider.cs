using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinegar
{
	public interface IStepProvider
	{
		Task<IEnumerable<Step>> GetSteps(IProgress<int> progress);
	}
}