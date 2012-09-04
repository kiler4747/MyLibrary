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

			FileSystemTree GetDiferent(FileSystemTree otherTree, int flags)
			{
				if (_root.Info.FullName != otherTree._root.Info.FullName)
				{
					return this;
				}

			}

			FileSystemTreeNode GetDiferent(FileSystemTreeNode thisTreeNode, FileSystemTreeNode otherTreeNode, Diferent flags)
			{
				if (thisTreeNode.Info.FullName != otherTreeNode.Info.FullName)
					return thisTreeNode;
				FileSystemTreeNode returnTreeNode = new FileSystemTreeNode( thisTreeNode.Info.FullName, thisTreeNode.Parent );
				foreach (var childThisNode in thisTreeNode.Children)
				{
					if (otherTreeNode.Children[childThisNode.Key] != null)
					{
						if (flags.HasFlag(Diferent.LastModified))
							if (childThisNode.Value.Info.LastWriteTime > otherTreeNode.Children[childThisNode.Key].Info.LastWriteTime)
								returnTreeNode.Children.Add( childThisNode.Key, childThisNode.Value );
							else
								returnTreeNode.Children.Add(childThisNode.Key, otherTreeNode.Children[childThisNode.Key]);

					}
				}
			}

			public static FileSystemTree GetFileSystemTree(string root)
			{
				FileSystemTree returnNode = new FileSystemTree(root);
				returnNode.CreateTree(returnNode._root);
				return returnNode;
			}

			[Flags]
			enum Diferent
			{
				None = 0,
				LastModified = 0x02,
				LastCreate = 0x04,

			}

			public static FileSystemTree GetDiferent(FileSystemTree first, FileSystemTree second, int flags  )
			{
				FileSystemTree returnTree = new FileSystemTree( first._root.Info.FullName );

				return returnTree.GetDiferent( second, flags );
			}
		}
	}
}
