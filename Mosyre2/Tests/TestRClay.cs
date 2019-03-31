using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Mosyre2.Tests
{
	public class TestRClay : MosyreTest
	{
		public TestRClay()
		{
		}

		public override void Run(string[] args)
		{
			var clay1 = new RClay();
			var agr = clay1.Agreement as RAgreement;

			Debug.Assert(agr.IsStaged == true, "IsStage is not true");

			var clay2 = new RClay();

			clay1.Connect(clay2, "P1");

			Debug.Assert(clay1._contacts["P1"] == clay2, "Did not make connection with clay2");

			clay1.Connect(clay2, "P2");

			Debug.Assert(clay1._contacts["P2"] == clay2, "Did not make connection with clay2");

			clay1.Connect(clay2, "P3");

			Debug.Assert(clay1._contacts.Count == 3, "Making connection with clays");

			var clay3 = new RClay();

			clay1.Connect(clay3, "P1");

			Debug.Assert(clay1._contacts["P1"] == clay3, "Did not make connection with clay3");

			Debug.Assert(clay1._contacts.Count == 3, "Missing connection, clay1 is supposed to have clay2 and clay3 ");

			

			clay1.Disconnect(clay1, "P1");

			Debug.Assert(clay1._contacts["P1"] == clay3, "clay3 should not be disconnected");
			Debug.Assert(clay1._contacts["P1"] == clay3, "clay3 should not be disconnected");
			Debug.Assert(clay1._contacts["P3"] == clay2, "clay2 should not be disconnected");

			clay1.Disconnect(clay1, "P3");
			Debug.Assert(clay1._contacts["P1"] == clay3, "clay3 should not be disconnected");
			Debug.Assert(clay1._contacts["P1"] == clay3, "clay3 should not be disconnected");

			Debug.Assert(!clay1._contacts.ContainsKey("P3"), "clay2 at P3 should be diconnected");


		}
	}
}
