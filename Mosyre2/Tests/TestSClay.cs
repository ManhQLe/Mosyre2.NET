using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mosyre2.Tests
{
	public class TestSClay : MosyreTest
	{
		class Add2Number : RClay {
			public Add2Number() {
				var agr = Agreement as RAgreement;
				agr.SensorPoints = new List<object> { "A", "B" };
			}

			protected override void OnResponse(object cp)
			{
				Center["O"] = Center.GetSignal<float>("A") + Center.GetSignal<float>("B");
			}
		}

		class Times2Number : RClay {
			public Times2Number()
			{
				var agr = Agreement as RAgreement;
				agr.SensorPoints = new List<object> { "A"};
			}

			protected override void OnResponse(object cp)
			{
				Center["O"] = Center.GetSignal<float>("A") * 2;
			}
		}

		public TestSClay()
		{
		}

		protected override void Run(string[] args)
		{
			var waiter = new AutoResetEvent(false);
			var a1 = new Add2Number();
			var t1 = new Times2Number();
			var t2 = new Times2Number();			

			var s1 = new Starter();
			var s2 = new Starter();

			Conduit.CreateLink(t1, "O",a1, "A");
			Conduit.CreateLink(t2, "O", a1, "B");

			float data = 0;

			SClay sclay = new SClay(new SAgreement
			{
				LayoutMap = new Dictionary<object, object[]> {
					{"X", new object[]{t1,"A" } },
					{"Y", new object[]{t2,"A" } },
					{"O", new object[]{a1,"O" } }
				}
			});

			var vclay = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "IN" },

				Response = (center, clay, sp) =>
				{
					data = center.GetSignal<float>("IN");
					waiter.Set();
				}
			});

			Conduit.CreateLink(sclay, "X", s1, "OUT");
			Conduit.CreateLink(sclay, "Y", s2, "OUT");
			Conduit.CreateLink(sclay, "O", vclay, "IN");

			s1.Test(.4f);
			s2.Test(.5f);
			waiter.WaitOne();
			Assert(data == 1.8f, "Supposed to be 1.8");
		}
	}
}
