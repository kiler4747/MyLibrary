namespace MyLibrary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;

	namespace FileSystemExt
	{
		public static class PathCombine
		{
#if UNIX
		public static readonly char Separator = Path.AltDirectorySeparatorChar;
#else
			public static readonly char Separator = Path.DirectorySeparatorChar;
#endif
			// Add Separator to path if need
			public static string Completing(string path)
			{
				if (path[path.Length - 1] != Separator)
					return path + Separator;
				return path;
			}
			public static string Combine(string path1, string path2)
			{
				if (null == path1 || null == path2)
					throw new ArgumentNullException();
				if (path1[path1.Length - 1] != Separator)
				{
					if (path2[0] != Separator)
						return path1 + Separator + path2;
					return path1 + path2;
				}
				if (path2[0] != Separator)
					return path1 + path2;
				return path1 + path2.Substring(1);
			}
			public static string Combine(string path1, string path2, string path3)
			{
				return Combine(Combine(path1, path2), path3);
			}
			public static string Combine(string path1, string path2, string path3, string path4)
			{
				return Combine(Combine(path1, path2, path3), path4);
			}
		}

		

		
	}

}
