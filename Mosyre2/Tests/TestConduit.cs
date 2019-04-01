using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2.Tests
{
	public class TestConduit : MosyreTest
	{
		public TestConduit()
		{
		}

		protected override void Run(string[] args)
		{
			var c = new Conduit();
			var clay1 = new RClay();
			var clay2 = new RClay();
			var clay3 = new RClay();

			c.LinkWith(new LinkDef[] {
				new LinkDef(clay1,"A"),
				new LinkDef(clay2,"B"),
				new LinkDef(clay3,"C")
			});

			c.LinkWith(new LinkDef[] {
				new LinkDef(clay1,"A")
			});			

			Assert(c._contacts.Count == 3, "Should have 3 connections");

			Assert(clay1._contacts["A"] == c, "Clay1 should have 1 connection with c");

			Assert(clay2._contacts["B"] == c, "Clay2 should have 1 connection with c");

			var test1 = c._contacts.FirstOrDefault(x => x.Clay == clay1 && x.ConnectPoint.Equals("A"));
			Assert(test1 != null, "Conduit supposed to have clay1 at point A");

			var test2 = c._contacts.FirstOrDefault(x => x.Clay == clay2 && x.ConnectPoint.Equals("B"));
			Assert(test2 != null, "Conduit supposed to have clay1 at point B");

			var test3 = c._contacts.FirstOrDefault(x => x.Clay == clay3 && x.ConnectPoint.Equals("C"));
			Assert(test3 != null, "Conduit supposed to have clay1 at point C");

			var testx = c._contacts.FirstOrDefault(x => x.Clay == clay2 && x.ConnectPoint.Equals("A"));
			Assert(testx == null, "Clay2 should not have point A in Conduit");


			c.Disconnect(clay2, "C");
			testx = c._contacts.FirstOrDefault(x => x.Clay == clay2 && x.ConnectPoint.Equals("B"));
			Assert(testx != null, "Clay2 should not be disconnected");

			c.Disconnect(clay2, "B");
			testx = c._contacts.FirstOrDefault(x => x.Clay == clay2 && x.ConnectPoint.Equals("B"));
			Assert(testx == null, "Clay2 should be disconnected");
		}
	}
}
