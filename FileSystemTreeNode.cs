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
			private string _name;
			private IDictionary<string, FileSystemTreeNode> _children;
			private FileSystemTreeNode _parent;

			private FileSystemTreeNode()
			{
				_name = null;
				_children = new Dictionary<string, FileSystemTreeNode>();
				_parent = null;
			}

			public FileSystemTreeNode(string name, FileSystemTreeNode parent = null)
				: this()
			{
				int indexSeparator = name.IndexOf(PathCombine.Separator);
				if (indexSeparator >= 0)
				{
					AddNode(name.Substring(indexSeparator + 1));
					_name = name.Substring(0, indexSeparator - 1);
				}
				else
				{
					_name = name;
				}
				_parent = parent;
			}

			public FileSystemTreeNode(FileSystemTreeNode node)
			{
				_name = node.Name;
				_children = new Dictionary<string, FileSystemTreeNode>(node.Children);
				_parent = new FileSystemTreeNode(node.Parent);
			}

			public string Name
			{
				get { return _name; }
				set { _name = value; }
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

			public void AddNode(IEnumerable<string> newNodes )
			{
				foreach (var newNode in newNodes)
				{
					AddNode(newNode);
				}
			}

			public void AddNode(IEnumerable<FileSystemTreeNode> newNodes)
			{
				foreach (var newNode in newNodes)
				{
					AddNode(newNode);
				}
			}

			public void AddNode(string newNode)
			{
				AddNode(new FileSystemTreeNode(newNode));
			}

			public void AddNode(FileSystemTreeNode newNode)
			{
				newNode._parent = this;
				_children.Add(newNode.Name,newNode);
			}

			public string GetPath()
			{
				if (_parent == null)
					return Name;
				return _parent.GetPath() + PathCombine.Separator + Name;
			}
		}
}
}
