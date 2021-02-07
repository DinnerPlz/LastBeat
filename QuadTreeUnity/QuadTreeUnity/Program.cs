#define DEBUG

using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace QuadTreeUnity
{

    /*

    */
    /*
    public class QuadTreeA
    {
        
         * Nodes will be added by scanning the open nodes, and placing a node there
         * The procces of scanning will be increased by row termination, where the first bit in a row
         * defines if the row is full, each row is comprised of 31 bits and a signal bit
         * the signal bit is at the front
         * 
         
        public struct node
        {
            public int c0, c1, c2, c3;
            public int i, d;

            // other data
            public float[] q;
        };
        int d;
        public node[] n = new node[31 * 108000];
        public bool[,] open = new bool[1860, 1860];
        public void FragmentNode(int id)
        {
            // takes a node and adds children
            int[] c = new int[4];
            for(int i = 0; i < 4; i++)
            {
                int res = FindEmptyNode();
                if(res == 0)
                    throw new Exception("Error array full");
                c[i] = res;
            } // gets id for four children and make sure that the array isnt full
            n[id].c0 = c[0];
            n[id].c1 = c[1];
            n[id].c2 = c[2];
            n[id].c3 = c[3];


        }
        public int FindEmptyNode()
        {
            UInt32 id = 0;

            for (UInt32 y = 0; y < open.GetLength(1); y++)
            {
                //Console.WriteLine(y);
                if (open[0, y])
                    continue; // if the first bool is true coninue to next 

                for (UInt32 x = 1; x < open.GetLength(0); x++)
                {
                    if (!open[x, y])
                    {
                        int[] c;
                        id = (UInt32)(y * open.GetLength(1) + x);
                        c = new int[] { (int)id};
                        if(x == open.GetLength(0)-1)
                        {
                            open[0, y] = true;
                            //Console.WriteLine("///////////////////////////// NEW");
                        }
                        //Console.WriteLine(id + "    " + x + "    " + y);
                        open[x, y] = true; // gonna regret later
                        return (int)id;
                    }
                }
                
            }
            Console.WriteLine("bad");
            return -1;
        }
        public void NodeCollapseCheck()
        {
            bool collapse = false;



            if (collapse)
                NodeCollapse(); // if needed collapse the node
        }
        public void NodeCollapse()
        {
            // find all children in node and deallocate
        }
    } // garbage
    */

    
    public class QuadTree
    {
        /*
         * This is the second attempt to make a quad tree
         * QuadTreeA was discarded due inabliliy to expand past a certin point
         * it has also been made apperent that pointers can be used
         * I will likely be running the simulating on CPU
         * bause I have decided that the speed is satifactory
         * but I likey will still leave room for a GUP implentation
         * This will likely be achived in a new rendition of the quadtree class
         * and will work on the same array baised pointer system used in
         * quadtreeA
         * 
         * The aim of this quadtree is to allow for dynamic size,
         * this will be achived by allowing the base node (node with no perents)
         * to create a perent and change it's depth to r = 1
         * An implentation of a neighbor finder will be used to finnalize
         * this implentaion of a quad tree
         * 
         * DEFINIONS:
         * r reffers to the depth of a specific node
         * the base node reffers to the least deep node at start
         * nodes may either expand in a positive or negitive direction
         * positive being smaller that the base node 
         * negitive being larger than the base node
         * A father node is a node with no perents
         *
         */

        /*
        public unsafe struct Node
        {
            // Base node 
            public Node *c0;
            public Node *c1;
            public Node *c2;
            public Node *c3; // children  0-3


            public Node *p; // perent

            public int r; // depth
            
            // all other data
            
        }
        */
        const int _MAXDEPTH = 30; // devisons will not go further than r = 20
        const int _MINDEPTH = -30; // no nodes with a depth smaller will be made
        // total depth = 60, 60^4 tiles possible,  12,960,000

        public Node baseNode;

        public class Node
        {
            
            public Node[] c; // children

            public Node p; // ref to perent node

            public int r; // depth of node, 0 is base level, pos inf is small, neg inf is big

            public float e;
            public float e2;
            public float e3;
            public float e4;
            public float e5;
            public float e6;
            public float e7;
        
        }
        unsafe void SplitNode()
        {
            // adds child nodes 
        }
        public void CreateParentNode(Node n, int corner)
        {
            // function is used to expand the tree
            // a larger node will be created above n, and all requirement 
            // to make a quadtree will be met
            // corner
            // 2 = north west
            // 3 = north east
            // 0 = south east
            // 1 = south west
            // in accordance to http://www.lcad.icmc.usp.br/~jbatista/procimg/quadtree_neighbours.pdf

            // n is the old father node
            // p is new father node
            // n is now child of p

            if (n.p != null)
                return; // only should becalled on father nodes
            if (n.r - 1 < _MINDEPTH)
                return; // do not go below min depth

            Node p = new Node();

            p.r = n.r - 1; // set depth
            p.c = new Node[4];

            for (int i = 0; i < p.c.Length; i ++)
            {
                if (i == corner)
                {
                    p.c[i] = n;
                    // if the corner lines up with the node, set that child to n
                }
                else
                {
                    p.c[i] = new Node();
                    // is the corner is not the one defined make a new node
                }
                p.c[i].r = n.r; 
                // set depth of new father node's children to the same as the node being slip
            }
            n.p = p;


        }
        public QuadTree()
        {
            baseNode = new Node();
            baseNode.r = 0;
        }
    
    }

    unsafe public class Program
    {
#if DEBUG 
        void Visualize()
        {
            // visualize tree (debug)

        }

#endif
        unsafe static void Main(string[] args)
        {
            /*
                QuadTree qt;
                for(int i = 0; i < qt.n.Length; i++)
                {
                    qt.n[i].q = new float[] { 1,2,3,4,5,6,7,8,9,0};
                }
                qt = new QuadTree();

                for (int i = 0; i < 200000;i++) {
                    qt.FragmentNode(i);
                }

            */

            QuadTree qt = new QuadTree();
            


            var watch = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Start prg");
            watch.Start();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);


            Console.WriteLine("here?");
            QuadTree.Node e = new QuadTree.Node();
            e = qt.baseNode;
            while (true)
            {
                qt.CreateParentNode(e, 0);

                e = e.p;
                Console.WriteLine(e.r);

            }
        }
    }
}
