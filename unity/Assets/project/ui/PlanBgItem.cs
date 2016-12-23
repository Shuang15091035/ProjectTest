using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {

	public class PlanBgItem : uapp.ListItem<uapp.IFile> {

		public Image img_image;
		public Text tv_name;

		public override void SetRecord(uapp.IFile record) {
			img_image.sprite = record.GetContent<Sprite>();
			tv_name.text = record.BaseName;
		}

	}
}
