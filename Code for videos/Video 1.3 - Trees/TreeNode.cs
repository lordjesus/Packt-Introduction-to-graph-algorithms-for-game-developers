using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_1._3___Trees
{
    class TreeNode<T>
    {
        public TreeNode<T> Parent { get; set; }
        public List<TreeNode<T>> Children { get; set; }
        public T Data { get; set; }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new List<TreeNode<T>>();
        }

        public void AddChild(TreeNode<T> child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public string SubTreeToString()
        {
            string tree = this.Data.ToString();
            if (this.Children.Count > 0)
            {
                tree += "(";
                for (int i = 0; i < this.Children.Count; i++)
                {
                    tree += this.Children[i].SubTreeToString();
                    if (i < this.Children.Count - 1)
                    {
                        tree += ", ";
                    }
                }
                tree += ")";
            }
            return tree;
        }
    }
}
