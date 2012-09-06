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

			FileSystemTree GetDiferent(FileSystemTree otherTree, Diferent flags)
			{
				if (_root.Info.FullName != otherTree._root.Info.FullName)
				{
					return this;
				}
				FileSystemTree returnTree = new FileSystemTree(_root.Info.FullName)
												{_root = GetDiferent(_root, otherTree._root, flags)};
				return returnTree;
			}

			FileSystemTreeNode GetDiferent(FileSystemTreeNode thisTreeNode, FileSystemTreeNode otherTreeNode, Diferent flags)
			{
				FileSystemTreeNode returnNode = null;
				bool isDid = false;
				if (otherTreeNode == null)
				{
					if (flags.HasFlag(Diferent.Deleted))
					{
						return new FileSystemTreeNode(thisTreeNode.Info.FullName, null);
						isDid = true;
					}
					else
					{
						throw new Exception("otherTreeNode == null");
					}
				}
				if (thisTreeNode == null)
				{
					if (flags.HasFlag(Diferent.LastCreated))
					{
						return returnNode = new FileSystemTreeNode(otherTreeNode.Info.FullName, null);
						isDid = true;
					}
					else 
						throw new Exception("ThisTreeNode == null");
				}
				returnNode = new FileSystemTreeNode(thisTreeNode.Info.FullName, null);
				foreach (var child in thisTreeNode.Children)
				{
					FileSystemTreeNode tempNode = GetDiferent(child.Value, otherTreeNode.Children[child.Key], flags);
					if (tempNode != null)
						returnNode.AddNode(tempNode);
				}
				if (returnNode.Children.Count == 0)
					returnNode = null;
				else
				{
					isDid = true;
				}
				if (flags.HasFlag(Diferent.Deleted) && !isDid)
					if (!File.Exists(otherTreeNode.Info.FullName) && (!Directory.Exists(otherTreeNode.Info.FullName)))
					{
						returnNode = new FileSystemTreeNode(otherTreeNode.Info.FullName, null);
						isDid = true;
					}
				if (flags.HasFlag(Diferent.LastModified) && !isDid)
				{
					if (thisTreeNode.Info.LastWriteTime > otherTreeNode.Info.LastWriteTime)
					{
						returnNode = new FileSystemTreeNode(thisTreeNode.Info.FullName, null);
						isDid = true;
					}
					else if (thisTreeNode.Info.LastWriteTime < otherTreeNode.Info.LastWriteTime)
					{
						returnNode = new FileSystemTreeNode(otherTreeNode.Info.FullName, null);
						isDid = true;
					}
				}
				if (flags.HasFlag(Diferent.LastCreated) && !isDid)
				{
					if (thisTreeNode.Info.CreationTime > otherTreeNode.Info.CreationTime)
					{
						returnNode = new FileSystemTreeNode(thisTreeNode.Info.FullName, null);
						isDid = true;
					}
					else if (thisTreeNode.Info.CreationTime < otherTreeNode.Info.CreationTime)
					{
						returnNode = new FileSystemTreeNode(otherTreeNode.Info.FullName, null);
						isDid = true;
					}
				}
				return returnNode;
			}

			public static FileSystemTree GetFileSystemTree(string root)
			{
				FileSystemTree returnNode = new FileSystemTree(root);
				returnNode.CreateTree(returnNode._root);
				return returnNode;
			}

			[Flags]
			public enum Diferent
			{
				None = 0,
				LastModified = 0x02,
				LastCreated = 0x04,
				Deleted = 0x06
			}

			public static FileSystemTree GetDiferent(FileSystemTree first, FileSystemTree second, Diferent flags  )
			{
				FileSystemTree returnTree = new FileSystemTree( first._root.Info.FullName );

				return returnTree.GetDiferent( second, flags );
			}
		}
	}
}
