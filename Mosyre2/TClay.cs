using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{

	public abstract class TClay: Clay
	{
		Dictionary<object, object> _mem = new Dictionary<object, object>();

		public TClay(Agreement agr) : base(agr)
		{
		}

		//Nomenclayture
		internal protected virtual void Remember(object thisThing, object asSymbol) {
			_mem[asSymbol] = thisThing;
		}

		internal protected virtual object Recall(object symbol)
		{
			return _mem.ContainsKey(symbol) ? _mem[symbol] : null;
		}

		internal protected virtual T Recall<T>(object symbol)
		{
			return (T)Recall(symbol);
		}

		internal protected virtual void Forget(object symbol) {
			_mem.Remove(symbol);
		}

	}
}
