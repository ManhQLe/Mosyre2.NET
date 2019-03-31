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

		public abstract void Connect(Clay withClay, object atConnectPoint);


		public abstract void Disconnect(Clay withClay, object atConnectPoint);


		internal protected abstract void OnSignal(Clay fromClay, object atConnectPoint, object signal);

		internal static void Vibrate(Clay clay, object atConnectPoint, object signal, Clay srcClay) {
			clay.OnSignal(srcClay, atConnectPoint, signal);
		}

		public static void Connect(Clay clay1, Clay clay2, object atCp1, object atCp2 = null) {
			clay1.Connect(clay2, atCp1);
			clay2.Connect(clay1, atCp2 == null ? atCp1 : atCp2);
		}
	}
}
