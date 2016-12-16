using UnityEngine;
using System.Collections;

namespace project {
	
	public enum AreaTableColumnIds {
		id,
		name,
	}

	public class AreaTable : uapp.FileTable<Area> {

		protected override Area newRecord() {
			return new Area();
		}

		protected override int getColumnIdByName(string columnName) {
			if (columnName == "id") {
				return (int)AreaTableColumnIds.id;
			} else if (columnName == "名字") {
				return (int)AreaTableColumnIds.name;
			}
			return 0;
		}

		protected override bool setRecord(Area record, string word, int columnIndex) {
			if (columnIndex == (int)AreaTableColumnIds.id) {
				record.Id = int.Parse(word);
			} else if (columnIndex == (int)AreaTableColumnIds.name) {
				record.Name = word;
			}
			return true;
		}

		protected override int getNumColumns() {
			return 2;
		}

		protected override string getRecord(Area record, int columnIndex) {
			if (columnIndex == (int)AreaTableColumnIds.id) {
				return record.Id.ToString();
			} else if (columnIndex == (int)AreaTableColumnIds.name) {
				return record.Name;
			}
			return null;
		}

		protected override string getColumnNameByIndex(int columnIndex) {
			if (columnIndex == (int)AreaTableColumnIds.id) {
				return "id";
			} else if (columnIndex == (int)AreaTableColumnIds.name) {
				return "名字";
			}
			return null;
		}
	}
}
