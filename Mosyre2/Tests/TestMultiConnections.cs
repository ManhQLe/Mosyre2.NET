using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mosyre2.Tests
{
	public class TestMultiConnections : MosyreTest
	{		

		protected override void Run(string[] args)
		{
			var waiter = new AutoResetEvent(false);

			
			int data = 0;
			var clay1 = new Starter();
			var clay2 = new Starter();
			var clay3 = new Starter();

			var clayx = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "A","B","C" },
				Response = (center, clay, cp) => {
					var A = center.GetSignal<int>("A");
					var B = center.GetSignal<int>("B");
					var C = center.GetSignal<int>("C");
					data = A + B + C;
					waiter.Set();
				}
			});

			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(clay1,"OUT"),
				new LinkDef(clayx,"A")
			});
			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(clay2,"OUT"),
				new LinkDef(clayx,"B")
			});
			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(clay3,"OUT"),
				new LinkDef(clayx,"C")
			});



			clay1.Test(1);
			clay2.Test(2);
			clay3.Test(3);

			waiter.WaitOne();
			Assert(data == 6, "Supposed to be 6");
		}
	}
}
