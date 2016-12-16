using UnityEngine;
using System.Collections;

namespace project {
	
	public enum CategoryTableColumnIds {
		id,
		name,
	}

	public class CategoryTable : uapp.FileTable<Category> {

		protected override Category newRecord() {
			return new Category();
		}

        
		protected override int getColumnIdByName(string columnName) {
			if (columnName == "id") {
				return (int)CategoryTableColumnIds.id;
			} else if (columnName == "名字") {
				return (int)CategoryTableColumnIds.name;
			}
			return 0;
		}

		protected override bool setRecord(Category record, string word, int columnIndex) {
			if (columnIndex == (int)CategoryTableColumnIds.id) {
				record.Id = int.Parse(word);
			} else if (columnIndex == (int)CategoryTableColumnIds.name) {
				record.Name = word;
			}
			return true;
		}

		protected override int getNumColumns() {
			return 2;
		}

		protected override string getRecord(Category record, int columnIndex) {
			if (columnIndex == (int)CategoryTableColumnIds.id) {
				return record.Id.ToString();
			} else if (columnIndex == (int)CategoryTableColumnIds.name) {
				return record.Name;
			}
			return null;
		}

		protected override string getColumnNameByIndex(int columnIndex) {
			if (columnIndex == (int)CategoryTableColumnIds.id) {
				return "id";
			} else if (columnIndex == (int)CategoryTableColumnIds.name) {
				return "名字";
			}
			return null;
		}
	}
}