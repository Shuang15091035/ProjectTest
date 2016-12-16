using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {
	
	public enum SKUTableColumnIds {
		Id,
		Name,
        Area,
		Category,
		Brand,
		Model,
		Price,
		Preview,
		SourceId,
		PackageId,
	}

	public class SKUTable : uapp.FileTable<SKU> {

		protected override SKU newRecord() {
			return new SKU();
		}

		protected override int getColumnIdByName(string columnName) {
			if (columnName == "id") {
				return (int)SKUTableColumnIds.Id;
			} else if (columnName == "名字") {
				return (int)SKUTableColumnIds.Name;
            } else if (columnName == "区域") {
                return (int)SKUTableColumnIds.Area;
            } else if (columnName == "分类") {
				return (int)SKUTableColumnIds.Category;
			} else if (columnName == "品牌") {
				return (int)SKUTableColumnIds.Brand;
			} else if (columnName == "型号") {
				return (int)SKUTableColumnIds.Model;
			}  else if (columnName == "单价") {
				return (int)SKUTableColumnIds.Price;
			} else if (columnName == "缩略图") {
				return (int)SKUTableColumnIds.Preview;
			}  else if (columnName == "素材id") {
				return (int)SKUTableColumnIds.SourceId;
			}  else if (columnName == "套餐id") {
				return (int)SKUTableColumnIds.PackageId;
			} 
			return 0;
		}

		protected override bool setRecord(SKU record, string word, int columnIndex) {
			if (columnIndex == (int)SKUTableColumnIds.Id) {
				record.Id = uapp.CoreUtils.ParseInt(word, 0);
				if (record.Id == 0) {
					return false;
				}
			} else if (columnIndex == (int)SKUTableColumnIds.Name) {
				record.Name = word;
            } else if (columnIndex == (int)SKUTableColumnIds.Area) {
                record.Area = word;
            } else if (columnIndex == (int)SKUTableColumnIds.Category) {
				record.Category = word;
			} else if (columnIndex == (int)SKUTableColumnIds.Brand) {
				record.Brand = word;
			} else if (columnIndex == (int)SKUTableColumnIds.Model) {
				record.Model = word;
			}  else if (columnIndex == (int)SKUTableColumnIds.Price) {
				record.Price = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)SKUTableColumnIds.Preview) {
				record.Preview = getFile(word, columnIndex);
			} else if (columnIndex == (int)SKUTableColumnIds.SourceId) {
				record.SourceId = uapp.CoreUtils.ParseInt(word, -1);
			} else if (columnIndex == (int)SKUTableColumnIds.PackageId) {
				record.PackageId = uapp.CoreUtils.ParseInt(word, -1);
			}
			return true;
		}

		protected override uapp.IFile getFile(string word, int columnIndex) {
			return uapp.File.Resource (word);
		}

		protected override int getNumColumns() {
			return 10;
		}

		protected override string getRecord(SKU record, int columnIndex) {
			if (columnIndex == (int)SKUTableColumnIds.Id) {
				return record.Id.ToString();
			} else if (columnIndex == (int)SKUTableColumnIds.Name) {
				return record.Name;
            } else if (columnIndex == (int)SKUTableColumnIds.Area) {
                return record.Area;
            } else if (columnIndex == (int)SKUTableColumnIds.Category) {
				return record.Category;
			} else if (columnIndex == (int)SKUTableColumnIds.Brand) {
                return record.Brand;
			} else if (columnIndex == (int)SKUTableColumnIds.Model) {
				return record.Model;
			} else if (columnIndex == (int)SKUTableColumnIds.Price) {
				return record.Price.ToString();
			} else if (columnIndex == (int)SKUTableColumnIds.Preview) {
				return record.Preview == null ? "" : record.Preview.Path;
			} else if (columnIndex == (int)SKUTableColumnIds.SourceId) {
				return record.SourceId.ToString();
			} else if (columnIndex == (int)SKUTableColumnIds.PackageId) {
				return record.PackageId.ToString();
			}
			return null;
		}

		protected override string getColumnNameByIndex(int columnIndex) {
			if (columnIndex == (int)SKUTableColumnIds.Id) {
				return "id";
			} else if (columnIndex == (int)SKUTableColumnIds.Name) {
				return "名字";
			} else if (columnIndex == (int)SKUTableColumnIds.Area) {
                return "区域";
            } else if (columnIndex == (int)SKUTableColumnIds.Category) {
				return "分类";
			} else if (columnIndex == (int)SKUTableColumnIds.Brand) {
				return "品牌";
			} else if (columnIndex == (int)SKUTableColumnIds.Model) {
				return "型号";
			}  else if (columnIndex == (int)SKUTableColumnIds.Price) {
				return "单价";
			} else if (columnIndex == (int)SKUTableColumnIds.Preview) {
				return "缩略图";
			} else if (columnIndex == (int)SKUTableColumnIds.SourceId) {
				return "素材id";
			} else if (columnIndex == (int)SKUTableColumnIds.PackageId) {
				return "套餐id";
			}
			return null;
		}

		public void Build(IList<SKU> skus, IList<Source> sources) {
			if (skus == null || sources == null) {
				return;
			}
			foreach (var sku in skus) {
				foreach (var source in sources) {
					if (sku.SourceId == source.Id) {
						sku.Source = source;
						break;
					}
				}
			}
		}
	}
}
