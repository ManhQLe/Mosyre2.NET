using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public abstract class Clay: IClay	
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

		internal static void Vibrate(IClay clay, object atConnectPoint, object signal, IClay srcClay) {
			clay.OnSignal(srcClay, atConnectPoint, signal);
		}

		public static void Connect(IClay clay1, IClay clay2, object atCp1, object atCp2 = null) {
			clay1.Connect(clay2, atCp1);
			clay2.Connect(clay1, atCp2 == null ? atCp1 : atCp2);
		}
	}
}
