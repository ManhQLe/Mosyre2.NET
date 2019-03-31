using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2.Tests
{
	public abstract class MosyreTest
	{
		public abstract void Run(string[] args);

		public void Start(string[] args) {
			var bk = Console.ForegroundColor;
			try
			{
				Console.WriteLine($"Starting {this.GetType()}...");

				Run(args);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"Test completed!");
				Console.ForegroundColor = bk;
			}
			catch (Exception ex) {

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex);
				Console.ForegroundColor = bk;
			}
		}
	}
}
