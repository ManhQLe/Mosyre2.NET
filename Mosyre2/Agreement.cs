using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Mosyre2
{

	public class Agreement
	{
		public T GetProperty<T>(string name)
		{
			var t = GetType();
			var p = t.GetProperty(name, BindingFlags.GetProperty | BindingFlags.Instance);
			if (p != null) {
				return (T)p.GetValue(this);
			}
			return default(T);
		}
	}
}
