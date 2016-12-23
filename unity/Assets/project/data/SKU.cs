using UnityEngine;
using System.Collections;

namespace project {
	
	public class SKU {

		public int Id;
		public string Name;
        public string Area;
		//public int CategoryId;
		public string Category;
		//public int BrandId;
		public string Brand;
		public string Model;
		public float Price;
		public uapp.IFile Preview;
		public int SourceId;
		public Source Source;
		public int PackageId; // 套餐id
		public Plan Package;

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			var sku = obj as SKU;
			if (Id == sku.Id) {
				return true;
			}
			// NOTE 暂时preview相同先视为同一个sku
			if (uapp.CoreUtils.ObjectEquals(Preview, sku.Preview)) {
				return true;
			}
			if (uapp.CoreUtils.ObjectEquals(Source, sku.Source)) {
				return true;
			}
			return false;
		}
	}
}
