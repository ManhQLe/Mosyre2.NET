using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{

	public abstract class TClay: Clay
	{
		Dictionary<object, object> _mem;

		public TClay(Agreement agr) : base(agr)
		{
		}

		//Nomenclayture
		protected virtual void Remember(object thisThing, object asSymbol) {
			_mem[asSymbol] = thisThing;
		}

		protected virtual object Recall(Enum symbol)
		{
			return _mem.ContainsKey(symbol) ? _mem[symbol] : null;
		}

		protected virtual void Forget(Enum symbol) {
			_mem.Remove(symbol);
		}

	}
}
