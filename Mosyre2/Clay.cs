using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public abstract class Clay
	{
		public Clay(Agreement theAgreement) {
			if (theAgreement == null)
				theAgreement = new Agreement();
			Agreement = theAgreement;
		}

		public Agreement Agreement { get; }
		
		
		public abstract void Connect(IClay withClay, object atConnectPoint);


		public abstract void Disconnect(IClay withClay, object atConnectPoint);


		public abstract void OnSignal(IClay fromClay, object atConnectPoint, object signal);
	}
}
