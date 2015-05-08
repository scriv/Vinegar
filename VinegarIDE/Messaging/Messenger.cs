using System.ComponentModel.Composition;
using GalaSoft.MvvmLight.Messaging;

namespace Vinegar.Ide.Messaging
{
	[Export(typeof(IMessenger))]
	public class Messenger : GalaSoft.MvvmLight.Messaging.Messenger
	{
	}
}