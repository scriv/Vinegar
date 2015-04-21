using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinegar
{
	public class GherkinParserException : Exception
	{
		internal GherkinParserException(GherkinParser.ParserContext currentContext)
		{
		}
	}
}