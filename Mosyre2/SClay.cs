using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosyre2
{
	public delegate Dictionary<object, LinkDef[]> SClayBuildFx(SClay clay);

	public class SAgreement : RAgreement {
		internal Dictionary<object, Conduit> _links = new Dictionary<object, Conduit>();

		public SAgreement() {
			LayoutMap = new Dictionary<object, LinkDef[]>();
		}

		public Dictionary<object, LinkDef[]> LayoutMap { get; set; }

		public SClayBuildFx Build { get; set; } = _DefaultBuild;

		static private Dictionary<object, LinkDef[]> _DefaultBuild(SClay clay) {
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
				agr._links[atConnectPoint].LinkWith(new LinkDef(withClay, atConnectPoint));
				base.Connect(withClay, atConnectPoint);
			}			
		}



		virtual protected Dictionary<object, LinkDef[]> OnBuild() {
			return (Agreement as SAgreement).Build(this);
		}
	}
}
