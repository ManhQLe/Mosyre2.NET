using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mosyre2.Tests
{
	public class TestTwoLayerConnection: MosyreTest
	{
		class FilterAgr : RAgreement
		{
			public int Weight { get; set; }
		}

		class SignalFilter : RClay {
			public SignalFilter(FilterAgr agr) : base(agr) {
				agr.SensorPoints = new List<object> { "IN" };
			}

			protected override void OnResponse(object cp)
			{
				var A = Agreement as FilterAgr;
				var o = A.Weight * Center.GetSignal<int>("IN");
				Center["OUT"] = o;
			}
		}

		protected override void Run(string[] args)
		{
			var waiter = new AutoResetEvent(false);


			int data = 0;
			var clay1 = new Starter();

			var f1 = new SignalFilter(new FilterAgr
			{
				Weight = 2
			});
			var f2 = new SignalFilter(new FilterAgr
			{
				Weight = 3
			});

			var f3 = new SignalFilter(new FilterAgr {
				Weight = 4
			});




			var clayx = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "A", "B", "C" },
				Response = (center, clay, cp) => {

					var A = center.GetSignal<int>("A");
					var B = center.GetSignal<int>("B");
					var C = center.GetSignal<int>("C");
					data = A + B + C;
					waiter.Set();
				}
			});

			Conduit.CreateLink(
				new LinkDef(clay1, "OUT"),
				new LinkDef(f1, "IN"),
				new LinkDef(f2, "IN"),
				new LinkDef(f3, "IN")
			);

			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(f1,"OUT"),
				new LinkDef(clayx,"A")
			});
			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(f2,"OUT"),
				new LinkDef(clayx,"B")
			});
			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(f3,"OUT"),
				new LinkDef(clayx,"C")
			});



			clay1.Test(5);			

			waiter.WaitOne();
			Assert(data == 45, "Supposed to be 45");
		}
	}
}
