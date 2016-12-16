using UnityEngine;
using System.Collections;

namespace project {

    public enum SourceType {
        Model,
        Material,
    }

    public class Source {

        public int Id;
        public string Name;
        public SourceType Type;
        public uapp.IFile File;
        public uapp.IFile File2;
        public uapp.IFile File3;
		public uapp.IFile File4;
		public uapp.IFile File5;
        public uapp.IFile Preview;
        public int RealL;
        public int RealW;
        public int RealH;
        public float TillingX = 1.0f;
        public float TillingY = 1.0f;

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			var source = obj as Source;
			if (Id == source.Id) {
				return true;
			}
			if (!uapp.CoreUtils.ObjectEquals(File, source.File)
				|| !uapp.CoreUtils.ObjectEquals(File2, source.File2)
				|| !uapp.CoreUtils.ObjectEquals(File3, source.File3)
				|| !uapp.CoreUtils.ObjectEquals(File4, source.File4)
				|| !uapp.CoreUtils.ObjectEquals(File5, source.File5)) {
				return false;
			}
			return true;
		}
    }
}
