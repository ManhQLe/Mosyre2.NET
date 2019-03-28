using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public class Contact
	{
		public Contact(IClay clay, object connectPoint) {
			Clay = clay;
			ConnectPoint = connectPoint;
		}

		public IClay Clay { get; }
		public object ConnectPoint { get; }
	}
}
