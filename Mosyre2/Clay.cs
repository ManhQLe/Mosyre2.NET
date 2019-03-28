using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public abstract class Clay : IClay
	{
		public Dictionary<object, object> Agreement { get => throw new NotImplementedException();}
		public List<Contact> Contacts { get => throw new NotImplementedException();}

		public abstract void Connect(IClay withClay, object atConnectPoint);


		public abstract void Disconnect(IClay withClay, object atConnectPoint);


		public abstract void OnSignal(IClay fromClay, object atConnectPoint, object signal);
	}
}
