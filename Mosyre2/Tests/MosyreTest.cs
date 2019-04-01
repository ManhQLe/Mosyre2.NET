using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2.Tests
{
	public abstract class MosyreTest
	{
		protected abstract void Run(string[] args);

		public void Start(string[] args) {
			var bk = Console.ForegroundColor;
			try
			{
				Console.Write($"Testing {this.GetType()}...");

				Run(args);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write($"Passed!\n");
				Console.ForegroundColor = bk;
			}
			catch (Exception ex) {

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex);
				Console.ForegroundColor = bk;
			}
		}

		protected void Assert(bool x, string message) {
			if (!x)
				throw new Exception(message);
		}
	}
}
