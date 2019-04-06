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
			return x.Clay == y.Clay && x.ConnectPoint == y.ConnectPoint;
		}


		public int GetHashCode(Contact obj)
		{
			return (obj.Clay.GetHashCode() ^ obj.ConnectPoint.GetHashCode()).GetHashCode();
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
		internal HashSet<Contact> _contacts = new HashSet<Contact>(new ContactComparer());

		public Conduit() : this(new CAgreement())
		{
		}

		public Conduit(CAgreement theAgreement): base(theAgreement)
		{
			
		}

		public override void Connect(Clay withClay, object atConnectPoint)
		{
			if (withClay is Conduit)
			{
				var c = _contacts.FirstOrDefault(x => x.Clay == withClay);
				if (c != null)
					return;
			}

			_contacts.Add(new Contact(withClay, atConnectPoint));
		}

		public override void Disconnect(Clay withClay, object atConnectPoint)
		{
			_contacts.Remove(new Contact(withClay, atConnectPoint));
		}

		protected override void OnSignal(Clay fromClay, object atConnectPoint, object signal)
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

		public void Link(LinkDef def) {
			Clay.Connect(this, def.Clay, def.ConnectPoint, def.ConnectPoint);
		}

		public void Link(params LinkDef[] def) {
			foreach (var d in def) {
				Clay.Connect(this, d.Clay, d.ConnectPoint, d.ConnectPoint);
			}
		}

		public static Conduit CreateLink(params LinkDef[] def) {
			var c = new Conduit();
			c.Link(def);
			return c;
		}

		public static Conduit CreateLink(params object[] def) {
			var c = new Conduit();
			if ((def.Length & 1) != 0)
				throw new Exception("Invalid definition");

			for (int i = 0; i < def.Length; i += 2)
			{
				if (def[i] is Clay)
				{
					c.Link(new LinkDef((Clay)def[i], def[i + 1]));
				}
				else
					throw new Exception("Invalid definition");
			}
			return c;
		}
	}
}
