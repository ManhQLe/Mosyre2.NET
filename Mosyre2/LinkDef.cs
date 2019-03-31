using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public class LinkDef
	{	
		public Clay Clay { get; set; }
		public object ConnectPoint { get; set; }
		public LinkDef(Clay clay, object cp) {
			Clay = clay;
			ConnectPoint = cp;
		}
	}
}
