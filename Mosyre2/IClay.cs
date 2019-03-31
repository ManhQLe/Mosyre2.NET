using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosyre2
{
	public interface IClay
	{

		void OnSignal(IClay fromClay, object atConnectPoint, object signal);

		void Connect(IClay withClay, object atConnectPoint);

		void Disconnect(IClay withClay, object atConnectPoint);

	}

}
