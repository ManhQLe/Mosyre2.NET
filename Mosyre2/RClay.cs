using System;
using System.Collections.Generic;
using System.Linq;

namespace Mosyre2
{
	public class RCore {
		internal RClay myClay;

		internal RCore() {
		}

		public object this[object cp] {
			get {
				return myClay.Recall(cp);
			}
			set {				
				if (myClay._contacts.ContainsKey(cp)) {
					Clay.Vibrate(myClay._contacts[cp], cp, value, myClay);
				}
			}
		}

		public T GetSignal<T>(object cp) {
			return (T)this[cp];
		}
	}

	public delegate void RClayFx(RCore center, RClay clay, object cp);

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

		static void _noResponse(RCore center, RClay clay, object cp) {
		}
	}

	public class RClay: TClay
	{
		internal Dictionary<object,Clay> _contacts = new Dictionary<object,Clay>();
		private List<object> _collected = new List<object>();
		long _one = 0;

		public RClay():this(new RAgreement()) {

		}

		public RClay(RAgreement agr) : base(agr)
		{
			Center = new RCore { myClay = this };
		}


		protected RCore Center { get; }

		protected List<object> SensorPoints
		{
			get {
				return (Agreement as RAgreement).SensorPoints;
			}
		}

		public override void Connect(Clay withClay, object atConnectPoint)
		{
			_contacts[atConnectPoint] = withClay;
		}

		public override void Disconnect(Clay withClay, object atConnectPoint)
		{
			if (_contacts.ContainsKey(atConnectPoint) && _contacts[atConnectPoint] == withClay)
				_contacts.Remove(atConnectPoint);
		}

		protected override void OnSignal(Clay fromClay, object atConnectPoint, object signal)
		{
			if (++_one == 1)
				OnInit();

			if (_contacts.ContainsKey(atConnectPoint) && 
				_contacts[atConnectPoint] == fromClay && IsValidSensorPoint(atConnectPoint)) {
			
				SetSignal(atConnectPoint, signal);
				if (IsAllSignalsReady()) {
					if ((Agreement as RAgreement).IsStaged)
						_collected.Clear();

					OnResponse(atConnectPoint);
				}
			}
		}

		protected virtual void OnResponse(object cp) {
			(Agreement as RAgreement).Response(Center, this, cp);
		}

		protected virtual void OnInit() {

		}

		protected bool IsValidSensorPoint(object cp) {
			return SensorPoints.Contains(cp);
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
