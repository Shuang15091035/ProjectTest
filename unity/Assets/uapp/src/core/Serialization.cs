/**
 * @file Serialization.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System;
using System.Collections;

namespace uapp {

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class SerializedField : Attribute {
		
		private string mName;
		private bool mUseToSerialize;
		private bool mUseToDeserialize;
		
		public SerializedField(string name) : this(name, true, true) {

		}

		public SerializedField(string name, bool useToSerialize, bool useToDeserialize) {
			mName = name;
			mUseToSerialize = useToSerialize;
			mUseToDeserialize = useToDeserialize;
		}
		
		public string Name {
			get {
				return mName;
			}
		}

		public bool UseToSerialize {
			get {
				return mUseToSerialize;
			}
		}

		public bool UseToDeserialize {
			get {
				return mUseToDeserialize;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class SerializedObject : SerializedField {

		public SerializedObject(string name) : base(name) {

		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class SerializedArray : SerializedField {

		public SerializedArray(string name) : base(name) {

		}
	}
}
