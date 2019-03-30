using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Mosyre2
{
	public class RCore {
	}

	public delegate void RClayFx(RCore center, IClay clay, object cp);

	public class RAgreement : Agreement
	{
		public RAgreement() {
			SensorPoints = new List<object>();
			IsStaged = true;
			Response = _noResponse;
		}
		public List<object> SensorPoints { get; set; }
		public RClayFx Response { get; set; }
		public bool IsStaged  {get;set;}

		static void _noResponse(RCore center, IClay clay, object cp) {
		}
	}

	public class RClay<T>: TClay where T: RAgreement
	{
		private List<Contact> _contacts = new List<Contact>();
		private List<object> _collected = new List<object>();
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
			var pair = _contacts.Find(x => x.ConnectPoint == atConnectPoint);
			if (pair == null)
				_contacts.Add(new Contact(withClay, atConnectPoint));
			else
				pair.Clay = withClay;
		}

		public override void Disconnect(IClay withClay, object atConnectPoint)
		{
			var idx = _contacts.FindIndex(x => x.ConnectPoint == atConnectPoint && x.Clay == withClay);
			if (idx >= 0)
				_contacts.RemoveAt(idx);
		}

		public override void OnSignal(IClay fromClay, object atConnectPoint, object signal)
		{
			var idx = _contacts.FindIndex(x => x.ConnectPoint == atConnectPoint && x.Clay == fromClay);
			if (idx >= 0 && IsValidSensorPoint(atConnectPoint)) {
				var contact = _contacts[idx];
				SetSignal(atConnectPoint, signal);
				if (IsAllSignalsReady()) {
					if ((Agreement as RAgreement).IsStaged)
						_collected.Clear();
					OnResponse(atConnectPoint);
				}

			}
		}

		protected virtual void OnResponse(object cp) {
			(Agreement as RAgreement).Response(null, this as IClay, cp);
		}

		protected virtual void OnInit() {

		}

		protected bool IsValidSensorPoint(object cp) {
			return SensorPoints.Find(x => x == cp) == null;
		}

		protected bool IsAllSignalsReady() {
			return SensorPoints.All(x => _collected.Contains(x));
		}

		protected void SetSignal(object connectPoint, object signal) {
			Remember(signal, connectPoint);
			if (!_collected.Contains(connectPoint)) {
				_collected.Add(connectPoint);
			}
		}
	}

	
}
