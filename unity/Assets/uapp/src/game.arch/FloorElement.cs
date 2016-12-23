using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public interface IFloorElement : IPolygonElement {


	}
	
	public class FloorElement : PolygonElement, IFloorElement {

		public FloorElement(RealWorldTexture texture) : base("floor", texture) {

		}

	}

}
