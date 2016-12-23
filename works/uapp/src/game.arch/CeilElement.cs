using UnityEngine;
using System.Collections;

namespace uapp {

	public interface ICeilElement : IPolygonElement {
		
	}

	public class CeilElement : PolygonElement, ICeilElement {

		public CeilElement(RealWorldTexture texture) : base("ceil", texture) {

		}

		public override ArchitectureBuildResult Build() {
			return trianglulate(false);
		}

	}
}
