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
			AutoResetEvent waiter = new AutoResetEvent(false);
			int data = 0;
			var clay1 = new Starter();

			var clay2 = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "IN" },
				Response = (center, clay, cp) => {
					data = center.GetSignal<int>("IN");
					Console.WriteLine($"Received: {data}");
					waiter.Set();
				}
			});

			Conduit.CreateLink(new LinkDef[] {
				new LinkDef(clay1,"OUT"),
				new LinkDef(clay2,"IN")
			});

			clay1.Test(1);

			waiter.WaitOne();
			Assert(data == 1, "Supposed to be 1");
		}
	}
}
