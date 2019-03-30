using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public delegate void RClayFx(TClay clay);

	public class RAgreement : Agreement
	{
		public List<object> SensorPoints { get; set; } = new List<object>();
		public RClayFx Response { get; set; }
	}

	public class RClay<T>: TClay where T: RAgreement
	{
		public RClay(T agr) : base(agr)
		{			
		}

		protected List<object> SensorPoints
		{
			get {
				return (Agreement as RAgreement).SensorPoints;
			}
		}		

		public override void Connect(IClay withClay, object atConnectPoint)
		{
			throw new NotImplementedException();
		}

		public override void Disconnect(IClay withClay, object atConnectPoint)
		{
			throw new NotImplementedException();
		}

		public override void OnSignal(IClay fromClay, object atConnectPoint, object signal)
		{
			throw new NotImplementedException();
		}
	}

	
}
