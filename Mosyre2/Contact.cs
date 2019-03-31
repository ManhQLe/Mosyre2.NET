using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public class Contact
	{
		public Contact(Clay clay, object connectPoint) {
			Clay = clay;
			ConnectPoint = connectPoint;
		}

		public Clay Clay { get; internal set; }
		public object ConnectPoint { get; internal set; }
	}
}
