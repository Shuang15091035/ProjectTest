using UnityEngine;
using System.Collections;

namespace project {
	
	public class Data : MonoBehaviour {
        
		public static SourceType ParseSource(string sourceType) {
			if (sourceType.Equals("模型")) {
				return SourceType.Model;
			} else if (sourceType.Equals("材质")) {
				return SourceType.Material;
			}
			return SourceType.Model;
		}

		public static string SourceTypeToString(SourceType sorceType) {
			switch (sorceType) {
				case SourceType.Model:
					{
						return "模型";
					}
				case SourceType.Material:
					{
						return "材质";
					}
				default:
					{
						return "模型";
					}
			}
		}

		public static Item ItemFromObject(GameObject itemObject) {
			return uapp.ObjectUtils.AddComponentIfNotExists<Item>(itemObject);
		}
	}
}