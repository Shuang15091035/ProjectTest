using UnityEngine;
using System.Collections;

namespace project {

    public enum SourceTableColumnIds {
        Id,
        Name,
        Type,
        File,
        File2,
        File3,
		File4,
		File5,
        Preview,
        RealL,
        RealW,
        RealH,
        TillingX,
        TillingY,
    }

    public class SourceTable : uapp.FileTable<Source> {

        protected override Source newRecord () {
            return new Source ();
        }

        protected override int getColumnIdByName (string columnName) {
            if (columnName == "id") {
                return (int)SourceTableColumnIds.Id;
            } else if (columnName == "名字") {
                return (int)SourceTableColumnIds.Name;
            } else if (columnName == "类型") {
                return (int)SourceTableColumnIds.Type;
            } else if (columnName == "文件") {
                return (int)SourceTableColumnIds.File;
            } else if (columnName == "文件2") {
                return (int)SourceTableColumnIds.File2;
            } else if (columnName == "文件3") {
                return (int)SourceTableColumnIds.File3;
			} else if (columnName == "文件4") {
				return (int)SourceTableColumnIds.File4;
			} else if (columnName == "文件5") {
				return (int)SourceTableColumnIds.File5;
			} else if (columnName == "缩略图") {
                return (int)SourceTableColumnIds.Preview;
            } else if (columnName == "实际长度") {
                return (int)SourceTableColumnIds.RealL;
            } else if (columnName == "实际宽度") {
                return (int)SourceTableColumnIds.RealW;
            } else if (columnName == "实际高度") {
                return (int)SourceTableColumnIds.RealH;
            } else if (columnName == "平铺X") {
                return (int)SourceTableColumnIds.TillingX;
			} else if (columnName == "平铺Y") {
                return (int)SourceTableColumnIds.TillingY;
            }
            return 0;
        }

		protected override bool setRecord (Source record, string word, int columnIndex) {
            if (columnIndex == (int)SourceTableColumnIds.Id) {
				record.Id = uapp.CoreUtils.ParseInt(word, 0);
				if (record.Id == 0) {
					return false;
				}
            } else if (columnIndex == (int)SourceTableColumnIds.Name) {
                record.Name = word;
            } else if (columnIndex == (int)SourceTableColumnIds.Type) {
                record.Type = Data.ParseSource (word);
            } else if (columnIndex == (int)SourceTableColumnIds.File) {
                record.File = getFile (word, columnIndex);
            } else if (columnIndex == (int)SourceTableColumnIds.File2) {
                record.File2 = getFile (word, columnIndex);
            } else if (columnIndex == (int)SourceTableColumnIds.File3) {
                record.File3 = getFile (word, columnIndex);
			} else if (columnIndex == (int)SourceTableColumnIds.File4) {
				record.File4 = getFile (word, columnIndex);
			} else if (columnIndex == (int)SourceTableColumnIds.File5) {
				record.File5 = getFile (word, columnIndex);
			} else if (columnIndex == (int)SourceTableColumnIds.Preview) {
				record.Preview = getFile (word, columnIndex);
            } else if (columnIndex == (int)SourceTableColumnIds.RealL) {
                record.RealL = uapp.CoreUtils.ParseInt (word, 1000);
            } else if (columnIndex == (int)SourceTableColumnIds.RealW) {
				record.RealW = uapp.CoreUtils.ParseInt (word, 1000);
            } else if (columnIndex == (int)SourceTableColumnIds.RealH) {
				record.RealH = uapp.CoreUtils.ParseInt (word, 1000);
            } else if (columnIndex == (int)SourceTableColumnIds.TillingX) {
                record.TillingX = uapp.CoreUtils.ParseFloat (word, 1.0f);
            } else if (columnIndex == (int)SourceTableColumnIds.TillingY) {
                record.TillingY = uapp.CoreUtils.ParseFloat (word, 1.0f); ;
            }
			return true;
        }

        protected override uapp.IFile getFile (string word, int columnIndex) {
            return uapp.File.Resource (word);
        }

        protected override int getNumColumns () {
            return 14;
        }

        protected override string getRecord (Source record, int columnIndex) {
            if (columnIndex == (int)SourceTableColumnIds.Id) {
                return record.Id.ToString ();
            } else if (columnIndex == (int)SourceTableColumnIds.Name) {
                return record.Name;
            } else if (columnIndex == (int)SourceTableColumnIds.Type) {
                return Data.SourceTypeToString (record.Type);
            } else if (columnIndex == (int)SourceTableColumnIds.File) {
                return record.File == null ? "" : record.File.Path;
            } else if (columnIndex == (int)SourceTableColumnIds.File2) {
                return record.File2 == null ? "" : record.File2.Path;
            } else if (columnIndex == (int)SourceTableColumnIds.File3) {
                return record.File3 == null ? "" : record.File3.Path;
			} else if (columnIndex == (int)SourceTableColumnIds.File4) {
				return record.File4 == null ? "" : record.File4.Path;
			} else if (columnIndex == (int)SourceTableColumnIds.File5) {
				return record.File5 == null ? "" : record.File5.Path;
			} else if (columnIndex == (int)SourceTableColumnIds.Preview) {
                return record.Preview == null ? "" : record.Preview.Path;
            } else if (columnIndex == (int)SourceTableColumnIds.RealL) {
                return record.RealL.ToString ();
            } else if (columnIndex == (int)SourceTableColumnIds.RealW) {
                return record.RealW.ToString ();
            } else if (columnIndex == (int)SourceTableColumnIds.RealH) {
                return record.RealH.ToString ();
            } else if (columnIndex == (int)SourceTableColumnIds.TillingX) {
                return record.TillingX.ToString ();
            } else if (columnIndex == (int)SourceTableColumnIds.TillingY) {
                return record.TillingY.ToString ();
            }
            return null;
        }

        protected override string getColumnNameByIndex (int columnIndex) {
            if (columnIndex == (int)SourceTableColumnIds.Id) {
                return "id";
            } else if (columnIndex == (int)SourceTableColumnIds.Name) {
                return "名字";
            } else if (columnIndex == (int)SourceTableColumnIds.Type) {
                return "分类";
            } else if (columnIndex == (int)SourceTableColumnIds.File) {
                return "文件";
            } else if (columnIndex == (int)SourceTableColumnIds.File2) {
                return "文件2";
            } else if (columnIndex == (int)SourceTableColumnIds.File3) {
                return "文件3";
			} else if (columnIndex == (int)SourceTableColumnIds.File4) {
				return "文件4";
			} else if (columnIndex == (int)SourceTableColumnIds.File5) {
				return "文件5";
			} else if (columnIndex == (int)SourceTableColumnIds.Preview) {
                return "缩略图";
            } else if (columnIndex == (int)SourceTableColumnIds.RealL) {
                return "实际长度";
            } else if (columnIndex == (int)SourceTableColumnIds.RealW) {
                return "实际宽度";
            } else if (columnIndex == (int)SourceTableColumnIds.RealH) {
                return "实际高度";
            } else if (columnIndex == (int)SourceTableColumnIds.TillingX) {
				return "平铺X";
            } else if (columnIndex == (int)SourceTableColumnIds.TillingY) {
				return "平铺Y";
            }
            return null;
        }
    }
}
