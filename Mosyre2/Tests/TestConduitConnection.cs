using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Mosyre2.Tests
{
	public class TestConduitConnection : MosyreTest
	{		
		protected override void Run(string[] args)
		{
			AutoResetEvent waiter = new AutoResetEvent(false);
			int data = 0;
			var clay1 = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "IN" },
				Response = (center, clay, cp) =>
				{
					center["OUT"] = (center.GetSignal<int>("IN") + 1);
				}
			});

			var clay2 = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "IN" },
				Response = (center, clay, cp) =>
				{
					center["OUT"] = (center.GetSignal<int>("IN") * 2);
				}
			});


			var clay3 = new RClay(new RAgreement
			{
				SensorPoints = new List<object> { "A","B" },
				Response = (center, clay, cp) =>
				{
					data = center.GetSignal<int>("A") + center.GetSignal<int>("B");
					waiter.Set();
				}
			});

			var start = new Starter();

			var con1 = Conduit.CreateLink(new LinkDef(clay1, "IN"), new LinkDef(clay2, "IN"));
			var con2 = new Conduit();
			con2.Link(new LinkDef(start, "OUT"));

			con1.Link(new LinkDef(con2, "X"));

			Conduit.CreateLink(new LinkDef(clay1, "OUT"), new LinkDef(clay3, "A"));
			Conduit.CreateLink(new LinkDef(clay2, "OUT"), new LinkDef(clay3, "B"));

			start.Test(3);
			
			waiter.WaitOne();
			Assert(data == 10, "Data supposed to be");
		}	
	}
}
