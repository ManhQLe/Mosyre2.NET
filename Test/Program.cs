using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mosyre2.Tests;
namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			MosyreTest t = new TestRClay();
			t.Start(args);

			t = new TestConduit();
			t.Start(args);

			t = new TestConnection();
			t.Start(args);

			t = new TestMultiConnections();
			t.Start(args);

			t = new TestTwoLayerConnection();
			t.Start(args);

			t = new TestConduitConnection();
			t.Start(args);

			t = new TestSClay();
			t.Start(args);
		}
	}
}
