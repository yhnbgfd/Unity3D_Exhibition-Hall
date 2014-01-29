using UnityEngine;
using System.Collections;

namespace ExhibitionHall.Common
{
	class NavMeshPara
	{
		public static string InitPath = "a";
		public static int InitSections = 0;
		public static string[] NavRoutes = new string[]{"a","b","c","d"};

		public static enum projectStatic
		{
			enStateInit,
			enStateChoose
		}
	}

}
