using UnityEngine;
using System.Collections;

namespace project {

    public enum PlanTableColumnIds {
        Id,
        Name,
        Scale,
        Background,
        File,
        Preview,
		PackagePrice,
		QRCode,
		FPSpx,
		FPSpz,
		FPSh,
		FPSax,
		FPSay,
		BIRDpx,
		BIRDpz,
		BIRDh,
    }

    public class PlanTable : uapp.FileTable<Plan> {

        public PlanTable (uapp.IFile dir = null) : base (dir) {

        }

        protected override Plan newRecord () {
            return new Plan ();
        }

        protected override int getColumnIdByName (string columnName) {
            if (columnName == "id") {
                return (int)PlanTableColumnIds.Id;
            } else if (columnName == "名字") {
                return (int)PlanTableColumnIds.Name;
            } else if (columnName == "比例尺") {
                return (int)PlanTableColumnIds.Scale;
            } else if (columnName == "背景图") {
                return (int)PlanTableColumnIds.Background;
            } else if (columnName == "文件") {
                return (int)PlanTableColumnIds.File;
            } else if (columnName == "缩略图") {
                return (int)PlanTableColumnIds.Preview;
			} else if (columnName == "套餐单价") {
				return (int)PlanTableColumnIds.PackagePrice;
			} else if (columnName == "二维码") {
				return (int)PlanTableColumnIds.QRCode;
			} else if (columnName == "FPSpx") {
				return (int)PlanTableColumnIds.FPSpx;
			} else if (columnName == "FPSpz") {
				return (int)PlanTableColumnIds.FPSpz;
			} else if (columnName == "FPSh") {
				return (int)PlanTableColumnIds.FPSh;
			} else if (columnName == "FPSax") {
				return (int)PlanTableColumnIds.FPSax;
			} else if (columnName == "FPSay") {
				return (int)PlanTableColumnIds.FPSay;
			} else if (columnName == "BIRDpx") {
				return (int)PlanTableColumnIds.BIRDpx;
			} else if (columnName == "BIRDpz") {
				return (int)PlanTableColumnIds.BIRDpz;
			} else if (columnName == "BIRDh") {
				return (int)PlanTableColumnIds.BIRDh;
			}
            return 0;
        }

        protected override bool setRecord (Plan record, string word, int columnIndex) {
            if (columnIndex == (int)PlanTableColumnIds.Id) {
				record.Id = uapp.CoreUtils.ParseInt(word, 0);
            } else if (columnIndex == (int)PlanTableColumnIds.Name) {
                record.Name = word;
            } else if (columnIndex == (int)PlanTableColumnIds.Scale) {
				record.Scale = uapp.CoreUtils.ParseFloat(word, 1.0f);
            } else if (columnIndex == (int)PlanTableColumnIds.Background) {
                record.Background = getFile (word, columnIndex);
            } else if (columnIndex == (int)PlanTableColumnIds.File) {
                record.File = getFile (word, columnIndex);
            } else if (columnIndex == (int)PlanTableColumnIds.Preview) {
                record.Preview = getFile (word, columnIndex);
			} else if (columnIndex == (int)PlanTableColumnIds.PackagePrice) {
				record.PackagePrice = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.QRCode) {
				record.QRCode = getFile (word, columnIndex);
			} else if (columnIndex == (int)PlanTableColumnIds.FPSpx) {
				record.FPSpx = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.FPSpz) {
				record.FPSpz = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.FPSh) {
				record.FPSh = uapp.CoreUtils.ParseFloat(word, Photographer.DefaultFPSCameraHeight);
			} else if (columnIndex == (int)PlanTableColumnIds.FPSax) {
				record.FPSax = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.FPSay) {
				record.FPSay = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.BIRDpx) {
				record.BIRDpx = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.BIRDpz) {
				record.BIRDpz = uapp.CoreUtils.ParseFloat(word, 0.0f);
			} else if (columnIndex == (int)PlanTableColumnIds.BIRDh) {
				record.BIRDh = uapp.CoreUtils.ParseFloat(word, Photographer.DefaultBirdCameraHeight);
			}
			return true;
        }

        protected override uapp.IFile getFile (string word, int columnIndex) {
//			if (columnIndex == (int)PlanTableColumnIds.File || columnIndex == (int)PlanTableColumnIds.Preview || columnIndex == (int)PlanTableColumnIds.QRCode) {
//                return uapp.File.Resource (word);
//            }
			if (columnIndex == (int)PlanTableColumnIds.File) {
				return uapp.File.Scene (word);
			}
			if (columnIndex == (int)PlanTableColumnIds.Preview || columnIndex == (int)PlanTableColumnIds.QRCode) {
				return uapp.File.Resource (word);
			}
            return uapp.File.DataPath (System.IO.Path.Combine (SharedModel.Instance.LocalAssetsDir.Path, word));
        }

//        protected override int getNumColumns () {
//            return 16;
//        }
//
//        protected override string getRecord (Plan record, int columnIndex) {
//            if (columnIndex == (int)PlanTableColumnIds.Id) {
//                return record.Id.ToString ();
//            } else if (columnIndex == (int)PlanTableColumnIds.Name) {
//                return record.Name;
//            } else if (columnIndex == (int)PlanTableColumnIds.Scale) {
//                return record.Scale.ToString ();
//            } else if (columnIndex == (int)PlanTableColumnIds.Background) {
//                return record.Background == null ? "" : record.Background.Path;
//            } else if (columnIndex == (int)PlanTableColumnIds.File) {
//                return record.File == null ? "" : record.File.Path;
//            } else if (columnIndex == (int)PlanTableColumnIds.Preview) {
//                return record.Preview == null ? "" : record.Preview.Path;
//			} else if (columnIndex == (int)PlanTableColumnIds.PackagePrice) {
//				return record.PackagePrice.ToString ();
//			} else if (columnIndex == (int)PlanTableColumnIds.QRCode) {
//				return record.QRCode.ToString ();
//			}
//            return null;
//        }
//
//        protected override string getColumnNameByIndex (int columnIndex) {
//            if (columnIndex == (int)PlanTableColumnIds.Id) {
//                return "id";
//            } else if (columnIndex == (int)PlanTableColumnIds.Name) {
//                return "名字";
//            } else if (columnIndex == (int)PlanTableColumnIds.Scale) {
//                return "比例尺";
//            } else if (columnIndex == (int)PlanTableColumnIds.Background) {
//                return "背景图";
//            } else if (columnIndex == (int)PlanTableColumnIds.File) {
//                return "文件";
//            } else if (columnIndex == (int)PlanTableColumnIds.Preview) {
//                return "缩略图";
//			} else if (columnIndex == (int)PlanTableColumnIds.PackagePrice) {
//				return "套餐单价";
//			} else if (columnIndex == (int)PlanTableColumnIds.QRCode) {
//				return "二维码";
//			}
//            return null;
//        }
    }
}
