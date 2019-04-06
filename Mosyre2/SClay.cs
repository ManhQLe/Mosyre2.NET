using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public delegate Dictionary<object, object[]> SClayBuildFx(SClay clay);

	public class SAgreement : RAgreement {
		internal Dictionary<object, Conduit> _links = new Dictionary<object, Conduit>();

		public SAgreement() {
			LayoutMap = new Dictionary<object, object[]>();
		}

		public Dictionary<object, object[]> LayoutMap { get; set; }

		public SClayBuildFx Build { get; set; } = _DefaultBuild;

		static private Dictionary<object, object[]> _DefaultBuild(SClay clay) {
			return (clay.Agreement as SAgreement).LayoutMap;
		}
	}

	public class SClay: RClay
	{
		public SClay(SAgreement agr) : base(agr) {
			var map = OnBuild();
			agr.SensorPoints = map.Keys.ToList();

			foreach (var cp in map.Keys) {
				agr._links.Add(cp, Conduit.CreateLink(map[cp]));
			}
		}

		public override void Connect(Clay withClay, object atConnectPoint)
		{
			var agr = (Agreement as SAgreement);
			if (agr._links.ContainsKey(atConnectPoint))
			{
				agr._links[atConnectPoint].Link(new LinkDef(withClay, atConnectPoint));
				base.Connect(withClay, atConnectPoint);
			}
		}

		virtual protected Dictionary<object, object[]> OnBuild() {
			return (Agreement as SAgreement).Build(this);
		}
	}
}
