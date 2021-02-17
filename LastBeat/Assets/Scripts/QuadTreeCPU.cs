using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace QuadTree
{
    public class QuadTree
    {
        const int _MAXDEPTH = 100;
        static public int currentDepth;
        public static byte[,,] quadLookUp = new byte[,,] { 
            {{0x1, 0xff },{0x1, 0x1 },{0x2, 0xff },{0x2, 0x3 },{0x3, 0x3 },{0x3, 0xff },{0x3, 0x1 },{0x3, 0x7 }},
            {{0x0, 0x0 },{0x0, 0xff },{0x3, 0xff },{0x3, 0x3 },{0x2, 0x4 },{0x2, 0xff },{0x2, 0xff},{0x2, 0x3 }},
            {{0x3, 0xff },{0x3, 0x1 },{0x0, 0x2 },{0x0, 0xff },{0x1, 0xff },{0x1, 0x2 },{0x1, 0x6 },{0x1, 0x0 }},
            {{0x2, 0x0 },{0x2, 0xff },{0x1, 0x2 },{0x1, 0xff },{0x0, 0x0 },{0x0, 0x5 },{0x0, 0x2 },{0x0, 0xff }}
        }; // pos 0x3
        /*
         * R, L, D, U, RU, RD, LD, LU
         * 0xff for halt
         * kinda supposed to be recursive(idk)
         * https://web.archive.org/web/20120907211934/http://ww1.ucmss.com/books/LFS/CSREA2006/MSV4517.pdf
         * i dont belive in god but i pray this makes any sense to me tommorw
         * ok so... *hits blunt*
         * the dimetions of the array indicate as follows :
         * [0x0 - 0x3 the quadrent the neighbors are looking for is]
         * [0x0 - 0x7 the eight squares that can be reffereanced]
         * [instruction to create the address]
         * when you are trying to find the neghbors you do as so:
         * this is a quad at the top left square, sampling to left
         */

        public class Node
        {
            public Node p; // perent
            public Node[] c; // children
            public int r; // depth
            byte pos; // gives relitive position 

            public Node FindNeighbor(byte direction)
            {
                Node n = null;

                

                return n;
            }


        }
    } // haha refactor 
    public class QuadTreeh
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
         */

        const int _MINDEPTH = -100; // no nodes with a depth smaller will be made
        static public int currnetDepth; // the largest current node, used for nighbor finding

        
        public class Node
        {
            public Node[] c; // children
            public Node p; // ref to perent node
            public int r; // depth of node, 0 is base level, pos inf is small, neg inf is big
            byte pos;
            /*
             * _______
             * |00|01|
             * |__|__|
             * |10|11|
             * |__|__|
             */ 

            // vars used for sim
            public byte[] FindNeighbor(byte direct)
            {
                switch (direct)
                {
                    //case
                }
                return null;
            }
            public bool[][] FindNeighbors()
            {
                // use finite state macine
                // top left 0x0
                // top right 0x1
                // bottom left 0x2
                // bottom right 0x3

                byte[][] neigh;

                switch (pos)
                {
                    case 0x0: // top left


                        break;
                    case 0x01: // top right

                        break;
                    case 0x2: // bottom left

                        break;
                    case 0x3: // top right

                        break;
                } 
                return null;
            }
            public Node()
            {
                // write neighbors
            }
        }
        public Node baseNode;
        public void CreateParentNode(Node n, int corner)
        {
              
            /*
             * function is used to expand the tree
             * a larger node will be created above n, and all requirement 
             * to make a quadtree will be met
             * corner
             * in accordance to http://www.lcad.icmc.usp.br/~jbatista/procimg/quadtree_neighbours.pdf
             *
             * n is the old father node
             * p is new father node
             * n is now child of p
             */

            if (n.p != null)
                return; // only should becalled on father nodes
            if (n.r - 1 < _MINDEPTH)
                return; // do not go below min depth

            Node p = new Node();

            p.r = n.r - 1; // set depth
            currnetDepth = p.r;
            p.c = new Node[4];

            for (int i = 0; i < p.c.Length; i++)
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
        public QuadTreeh()
        {
            baseNode = new Node();
            baseNode.r = 0;
        }

        public void Expand()
        {
            //run func on all nodes with r = 0;
        }
        public void Simulate(ref Node n)
        {
            //simluates sigle node
            Debug.Log("glizzy");
        }
        

    }

}