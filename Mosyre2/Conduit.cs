using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	internal class ContactComparer : IEqualityComparer<Contact>
	{
		
		public bool Equals(Contact x, Contact y)
		{
			return x.Clay.Equals(y.Clay) && x.ConnectPoint.Equals(y.ConnectPoint);
		}


		public int GetHashCode(Contact obj)
		{
			return obj.GetHashCode();
		}
	}


	public class CAgreement:Agreement {
		public CAgreement() {
			ParallelTrx = true;
		}

		public bool ParallelTrx { get; set; }
	}

	public class Conduit : Clay
	{
		HashSet<Contact> _contacts = new HashSet<Contact>(new ContactComparer());

		public Conduit() : this(new CAgreement())
		{
		}

		public Conduit(CAgreement theAgreement): base(theAgreement)
		{
			
		}

		public override void Connect(IClay withClay, object atConnectPoint)
		{
			if (withClay is Conduit)
			{
				var c = _contacts.FirstOrDefault(x => x.Clay == withClay);
				if (c.Clay == withClay)
					return;
			}

			_contacts.Add(new Contact(withClay, atConnectPoint));
		}

		public override void Disconnect(IClay withClay, object atConnectPoint)
		{
			_contacts.Remove(new Contact(withClay, atConnectPoint));
		}

		public override void OnSignal(IClay fromClay, object atConnectPoint, object signal)
		{
			var valid = _contacts.Contains(new Contact(fromClay, atConnectPoint));
			if (valid)
				foreach (var c in _contacts)
				{
					if (c.ConnectPoint != atConnectPoint || c.Clay != fromClay)
					{
						if ((Agreement as CAgreement).ParallelTrx)
						{
							Task.Run(() => Clay.Vibrate(c.Clay, c.ConnectPoint, signal, this));
						}
						else
							Clay.Vibrate(c.Clay, c.ConnectPoint, signal, this);
					}
				}
		}

		public void LinkWith(params LinkDef[] def) {
			foreach (var d in def) {
				Clay.Connect(this, d.Clay, d.ConnectPoint, d.ConnectPoint);
			}
		}

		public static Conduit CreateLink(params LinkDef[] def) {
			var c = new Conduit();
			c.LinkWith(def);
			return c;
		}
	}
}
