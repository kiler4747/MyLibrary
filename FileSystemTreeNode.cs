using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary
{
	namespace FileSystemExt
	{
		using System.IO;

		class FileSystemTreeNode
		{
			private FileSystemInfo _info;
			private IDictionary<string,FileSystemTreeNode> _children;
			private FileSystemTreeNode _parent;

			private FileSystemTreeNode()
			{
				_info = null;
				_children = new Dictionary<string, FileSystemTreeNode>();
				_parent = null;
			}

			public FileSystemTreeNode(string path, FileSystemTreeNode parent)
				: this()
			{
				if (File.Exists(path))
					_info = new FileInfo(path);
				else if (Directory.Exists(path))
				{
					_info = new DirectoryInfo(path);
				}
				else
				{
					throw new Exception("Неверно задан путь к файлу/папке");
				}
				_parent = parent;
			}

			public FileSystemInfo Info
			{
				get { return _info; }
				set { _info = value; }
			}

			public IDictionary<string,FileSystemTreeNode> Children
			{
				get { return _children; }
				set { _children = (IDictionary<string,FileSystemTreeNode>)value; }
			}

			public FileSystemTreeNode Parent
			{
				set { _parent = value; }
				get { return _parent; }
			}

			public void AddNode(FileSystemTreeNode newNode)
			{
				newNode._parent = this;
				_children.Add(newNode.Info.Name,newNode);
			}


		}
}
}
