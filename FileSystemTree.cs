using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary
{
	namespace FileSystemExt
	{
		using System.IO;

		public class FileSystemTree
		{

			private FileSystemTreeNode _root;

			public FileSystemTree(string root)
			{
				_root = new FileSystemTreeNode(root, null);
			}

			void CreateTree(FileSystemTreeNode rootNode)
			{
				IEnumerable<string> dirs = Directory.EnumerateDirectories(rootNode.Info.FullName);
				foreach (var dir in dirs)
				{
					var dirTempNode = new FileSystemTreeNode( dir, rootNode );
					rootNode.AddNode(dirTempNode);
					CreateTree(dirTempNode);
				}
				IEnumerable<string> files = Directory.EnumerateFiles(rootNode.Info.FullName);
				foreach (var file in files)
				{
					rootNode.AddNode(new FileSystemTreeNode(file, rootNode));
				}
			}

			public static FileSystemTree GetFileSystemTree(string root)
			{
				FileSystemTree returnNode = new FileSystemTree(root);
				returnNode.CreateTree(returnNode._root);
				return returnNode;
			}

			public static FileSystemTree GetDiferent(FileSystemTree first, FileSystemTree second)
			{
//				FileSystemTree returnFileSystemTreeDiferent = new FileSystemTree();
				return first;
			}
		}
	}
}
