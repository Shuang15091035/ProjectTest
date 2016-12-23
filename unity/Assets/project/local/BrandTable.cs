using UnityEngine;
using System.Collections;

namespace project {
	
	public enum BrandTableColumnIds {
		id,
		name,
	}

	public class BrandTable : uapp.FileTable<Brand> {

		protected override Brand newRecord() {
			return new Brand();
		}

		protected override int getColumnIdByName(string columnName) {
			if (columnName == "id") {
				return (int)BrandTableColumnIds.id;
			} else if (columnName == "名字") {
				return (int)BrandTableColumnIds.name;
			}
			return 0;
		}

		protected override bool setRecord(Brand record, string word, int columnIndex) {
			if (columnIndex == (int)BrandTableColumnIds.id) {
				record.Id = int.Parse(word);
			} else if (columnIndex == (int)BrandTableColumnIds.name) {
				record.Name = word;
			} 
			return true;
		}

		protected override int getNumColumns() {
			return 2;
		}

		protected override string getRecord(Brand record, int columnIndex) {
			if (columnIndex == (int)BrandTableColumnIds.id) {
				return record.Id.ToString();
			} else if (columnIndex == (int)BrandTableColumnIds.name) {
				return record.Name;
			}
			return null;
		}

		protected override string getColumnNameByIndex(int columnIndex) {
			if (columnIndex == (int)BrandTableColumnIds.id) {
				return "id";
			} else if (columnIndex == (int)BrandTableColumnIds.name) {
				return "名字";
			} 
			return null;
		}
	}
}