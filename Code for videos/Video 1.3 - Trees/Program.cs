using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_1._3___Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Video 1.3");

            TreeNode<string> root = new TreeNode<string>("Root");
            TreeNode<string> node1 = new TreeNode<string>("L1");
            TreeNode<string> node2 = new TreeNode<string>("L2");
            TreeNode<string> node3 = new TreeNode<string>("L3");
            TreeNode<string> leaf1_1 = new TreeNode<string>("L11");
            TreeNode<string> leaf1_2 = new TreeNode<string>("L12");
            TreeNode<string> leaf1_3 = new TreeNode<string>("L13");
            TreeNode<string> leaf2_1 = new TreeNode<string>("L21");
            TreeNode<string> leaf2_2 = new TreeNode<string>("L22");

            root.AddChild(node1);
            root.AddChild(node2);
            root.AddChild(node3);

            node1.AddChild(leaf1_1);
            node1.AddChild(leaf1_2);
            node1.AddChild(leaf1_3);

            node2.AddChild(leaf2_1);
            node2.AddChild(leaf2_2);

            string tree = root.SubTreeToString();

            Console.WriteLine("Printing tree");
            Console.WriteLine(tree);

            Console.WriteLine("End of video 1.3");

            while (true) ;
        }
    }
}
