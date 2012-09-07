using System;
using System.Windows;

namespace MyLibrary
{
	using System.Collections.Generic;
	using System.IO;

	namespace FileSystemExt
	{
		public class FileSystem
		{
			public static IEnumerable<string> GetListPaths(string root, IEnumerable<string> unclud )
			{
				try
				{

				var pathsDirsAndFiles = new List<string>(Directory.EnumerateDirectories(root));
				if (unclud != null)
					foreach (var uncludEntry in unclud)
					{
						pathsDirsAndFiles.Remove(uncludEntry);
					}
				var returnList = new List<string>();
				foreach (var entry in pathsDirsAndFiles)
				{
					returnList.Add(entry);
					returnList.AddRange(GetListPaths(entry, unclud));
				}
				pathsDirsAndFiles = new List<string>(Directory.EnumerateFiles(root));
				if (unclud != null)
					foreach (var uncludEntry in unclud)
					{
						pathsDirsAndFiles.Remove(uncludEntry);
					}
				returnList.AddRange(pathsDirsAndFiles);
//				returnList.AddRange(GetListPaths(entry, unclud));
				return returnList;
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
				return null;
			}
		}
	}
}