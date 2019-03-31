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
		int _one = 0;
		RCore _core;

		public RClay():this(new RAgreement()) {

		}

		public RClay(RAgreement agr) : base(agr)
		{
			_core = new RCore { myClay = this };
		}

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

		public override void OnSignal(Clay fromClay, object atConnectPoint, object signal)
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
			(Agreement as RAgreement).Response(_core, this, cp);
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
