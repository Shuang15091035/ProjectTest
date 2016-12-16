using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class FilePatterns {
		// common
		public static string JPG = "^[\\w\\W]*.[jJ][pP][gG]$";
		public static string PNG = "^[\\w\\W]*.[pP][nN][gG]$";
		public static string DAE = "^[\\w\\W]*.[dD][aA][eE]$";
		// special
		public static string DAX = "^[\\w\\W]*.[dD][aA][xX]$";
		public static string PLA = "^[\\w\\W]*.[pP][lL][aA]$";
		public static string FIT = "^[\\w\\W]*.[fF][iI][tT]$";
	}
}