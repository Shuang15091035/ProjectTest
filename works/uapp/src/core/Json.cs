/**
 * @file Json.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-13
 * @brief
 */
using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class JsonFormatter {

		public string SerializeText(object obj) {
			if (obj == null) {
				return null;
			}
			IDictionary<string, object> jsonObject = serializeObject(obj);
			if (jsonObject == null) {
				return null;
			}
			string json = MiniJSON.Json.Serialize(jsonObject);
			return json;
		}

		public object DeserializeText(string json, Type type) {
			object obj = CreateObject(type);
			if (obj == null) {
				return null;
			}
			object jsonObject = MiniJSON.Json.Deserialize(json);
            if (deserializeObject((IDictionary<string, object>)jsonObject, type, obj)) {
                return obj;
            }
            return null;
		}

		public bool Serialize(object obj, IFile file) {
			if (file == null) {
				return false;
			}
			IDictionary<string, object> jsonObject = serializeObject(obj);
			if (jsonObject == null) {
				return false;
			}
			string jsonText = MiniJSON.Json.Serialize(jsonObject);
			return file.WriteText(jsonText);
		}

		public object Deserialize(IFile file, Type type) {
			if (file == null) {
				return false;
			}
			object obj = CreateObject(type);
			if (obj == null) {
				return null;
			}
			string jsonText = file.Text;
			object jsonObject = MiniJSON.Json.Deserialize(jsonText);
			return deserializeObject((IDictionary<string, object>)jsonObject, type, obj);
		}

		/**
		 * 若不想把对象域依次序列化其成员，可标记对象域为SerializedField而不是SerializedObject
		 * 例如将Vector3序列化成(x, y, z)而不是{x:##, y: ##, z:##}
		 */
		protected virtual object GetFieldValue(object obj, FieldInfo field, string name) {
			return field.GetValue(obj);
		}

        protected virtual object GetPropertyValue(object obj, PropertyInfo property, string name) {
            return property.GetValue(obj, null);
        }

		protected virtual void SerializeOthers(object obj, IDictionary<string, object> jsonObject) {

		}

		protected virtual object CreateObject(Type type) {
            object obj = type.Assembly.CreateInstance(type.FullName);
            return obj;
		}

		protected virtual bool SetFieldValue(object obj, FieldInfo field, object value) {
			try {
				Type fieldType = field.FieldType;
				value = Convert.ChangeType(value, fieldType);
				field.SetValue(obj, value);
			} catch (Exception) {
				return false;
			}
			return true;
		}

		protected virtual bool DeserializeOthers(IDictionary<string, object> jsonObject, object obj) {
			return true;
		}

		protected IDictionary<string, object> serializeObject(object obj) {
			if (obj == null) {
				return null;
			}
			IDictionary<string, object> jsonObject = new Dictionary<string, object>();
			Type type = obj.GetType();
			foreach (FieldInfo field in type.GetFields()) {
				serializeField(obj, field, jsonObject);
			}
            foreach (PropertyInfo property in type.GetProperties()) {
                serializeProperty(obj, property, jsonObject);
            }
			SerializeOthers(obj, jsonObject);
			return jsonObject;
		}

		protected void serializeField(object obj, FieldInfo field, IDictionary<string, object> jsonObject) {
			string name = null;
			object value = null;
			SerializedObject[] serializedObjects = (SerializedObject[])field.GetCustomAttributes(typeof(SerializedObject), false);
//			if (serializedObjects == null || serializedObjects.Length == 0) {
//				serializedObjects = (SerializedObject[])obj.GetType().GetCustomAttributes(typeof(SerializedObject), false);
//			}
			SerializedField[] serializedFields = (SerializedField[])field.GetCustomAttributes(typeof(SerializedField), false);
			SerializedArray[] serializedArrays = (SerializedArray[])field.GetCustomAttributes(typeof(SerializedArray), false);
			if (serializedObjects != null && serializedObjects.Length == 1) {
				SerializedObject serializedObject = serializedObjects[0];
				if (serializedObject.UseToSerialize) {
					name = serializedObject.Name;
					value = serializeObject(field.GetValue(obj));
				}
			} else if (serializedArrays != null && serializedArrays.Length == 1) {
				SerializedArray serializedArray = serializedArrays[0];
				if (serializedArray.UseToSerialize) {
					name = serializedArray.Name;
					IEnumerable valueArray = field.GetValue(obj) as IEnumerable;
					if (valueArray != null) {
						ArrayList array = new ArrayList();
						foreach (object item in valueArray) {
							array.Add(serializeObject(item));
						}
						value = array;
					}
				}
			} else if (serializedFields != null && serializedFields.Length == 1) {
				SerializedField serializedField = serializedFields[0];
				if (serializedField.UseToSerialize) {
					name = serializedField.Name;
					value = GetFieldValue(obj, field, name);
				}
			}
			if (name == null || value == null) {
				return;
			}
			jsonObject[name] = value;
		}

        protected void serializeProperty(object obj, PropertyInfo property, IDictionary<string, object> jsonObject) {
            string name = null;
            object value = null;
            SerializedObject[] serializedObjects = (SerializedObject[])property.GetCustomAttributes(typeof(SerializedObject), false);
            //			if (serializedObjects == null || serializedObjects.Length == 0) {
            //				serializedObjects = (SerializedObject[])obj.GetType().GetCustomAttributes(typeof(SerializedObject), false);
            //			}
            SerializedField[] serializedFields = (SerializedField[])property.GetCustomAttributes(typeof(SerializedField), false);
            SerializedArray[] serializedArrays = (SerializedArray[])property.GetCustomAttributes(typeof(SerializedArray), false);
            if (serializedObjects != null && serializedObjects.Length == 1) {
                SerializedObject serializedObject = serializedObjects[0];
                if (serializedObject.UseToSerialize) {
                    name = serializedObject.Name;
                    value = serializeObject(property.GetValue(obj, null));
                }
            } else if (serializedArrays != null && serializedArrays.Length == 1) {
                SerializedArray serializedArray = serializedArrays[0];
                if (serializedArray.UseToSerialize) {
                    name = serializedArray.Name;
                    IEnumerable valueArray = property.GetValue(obj, null) as IEnumerable;
                    if (valueArray != null) {
                        ArrayList array = new ArrayList();
                        foreach (object item in valueArray) {
                            array.Add(serializeObject(item));
                        }
                        value = array;
                    }
                }
            } else if (serializedFields != null && serializedFields.Length == 1) {
                SerializedField serializedField = serializedFields[0];
                if (serializedField.UseToSerialize) {
                    name = serializedField.Name;
                    value = GetPropertyValue(obj, property, name);
                }
            }
            if (name == null || value == null) {
                return;
            }
            jsonObject[name] = value;
        }

		private bool deserializeObject(IDictionary<string, object> jsonObject, Type type, object obj) {
			if (jsonObject == null) {
				return false;
			}
			foreach (FieldInfo field in type.GetFields()) {
				if (!deserializeField(jsonObject, obj, field)) {
					return false;
				}
			}
			return DeserializeOthers(jsonObject, obj);
		}

		private bool deserializeField(IDictionary<string, object> jsonObject, object obj, FieldInfo field) {
			string name = null;
			object value = null;
			SerializedObject[] serializedObjects = (SerializedObject[])field.GetCustomAttributes(typeof(SerializedObject), false);
			if (serializedObjects != null && serializedObjects.Length == 1) {
				SerializedObject serializedObject = serializedObjects[0];
				if (serializedObject.UseToDeserialize) {
					name = serializedObject.Name;
					if (jsonObject.ContainsKey(name)) {
						value = jsonObject[name];
						Type type = field.GetType();
						object fieldObj = CreateObject(type);
						if (fieldObj == null) {
							return false;
						}
						return deserializeObject((IDictionary<string, object>)value, field.GetType(), fieldObj);
					}
				}
			} else { // TODO handle SerializedArray
				SerializedField[] serializedFields = (SerializedField[])field.GetCustomAttributes(typeof(SerializedField), false);
				if (serializedFields != null && serializedFields.Length == 1) {
					SerializedField serializedField = serializedFields[0];
					if (serializedField.UseToSerialize) {
						name = serializedField.Name;
						if (jsonObject.ContainsKey(name)) {
							value = jsonObject[name];
							SetFieldValue(obj, field, value);
						}
					}
				}
			}
			return true;
		}

		protected IDictionary<string, object> createJsonObject() {
			return new Dictionary<string, object>();
		}
	}
}

