using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Trees
{
    public class QuadTree
    {
        //const int _MAXDEPTH = 100;
        static public int currentDepth = 3;

        public readonly byte[] quadLookUp2 = new byte[] {
            0x1, 0xff ,0x1, 0x1, 0x2,
            0x0, 0x0 ,0x0, 0xff, 0x3,
            0x3, 0xff ,0x3, 0x1, 0x0,
            0x2, 0x0 ,0x2, 0xff, 0x1
        }; // pos 0x3
        

        public readonly static byte[] quadLookUp = new byte[] {
            0x1, 0xff ,0x1, 0x1 ,0x2, 0xff ,0x2, 0x3 ,
            0x0, 0x0 ,0x0, 0xff ,0x3, 0xff ,0x3, 0x3 ,
            0x3, 0xff ,0x3, 0x1 ,0x0, 0x2 ,0x0, 0xff ,
            0x2, 0x0 ,0x2, 0xff ,0x1, 0x2 ,0x1, 0xff 
        }; // pos 0x3

        public static byte[,,] quadLookUpO = new byte[,,] {
            {{0x1, 0xff },{0x1, 0x1 },{0x2, 0xff },{0x2, 0x3 },{0x3, 0x3 },{0x3, 0xff },{0x3, 0x1 },{0x3, 0x7 }},
            {{0x0, 0x0 },{0x0, 0xff },{0x3, 0xff },{0x3, 0x3 },{0x2, 0x4 },{0x2, 0xff },{0x2, 0xff},{0x2, 0x3 }},
            {{0x3, 0xff },{0x3, 0x1 },{0x0, 0x2 },{0x0, 0xff },{0x1, 0xff },{0x1, 0x2 },{0x1, 0x6 },{0x1, 0x0 }},
            {{0x2, 0x0 },{0x2, 0xff },{0x1, 0x2 },{0x1, 0xff },{0x0, 0x0 },{0x0, 0x5 },{0x0, 0x2 },{0x0, 0xff }}
        };

        /*
        public static byte[,,] quadLookUp1 = new byte[,,] {
            {{0x1, 0xff },{0x1, 0x1 },{0x2, 0xff },{0x2, 0x3 },{0x3, 0x3 },{0x3, 0xff },{0x3, 0x1 },{0x3, 0x7 }},
            {{0x0, 0x0 },{0x0, 0xff },{0x3, 0xff },{0x3, 0x3 },{0x2, 0x4 },{0x2, 0xff },{0x2, 0xff},{0x2, 0x1 }},
            {{0x3, 0xff },{0x3, 0x1 },{0x0, 0x2 },{0x0, 0xff },{0x1, 0xff },{0x1, 0x2 },{0x1, 0x6 },{0x1, 0x0 }},
            {{0x2, 0x0 },{0x2, 0xff },{0x1, 0x2 },{0x1, 0xff },{0x0, 0x0 },{0x0, 0x5 },{0x0, 0x2 },{0x0, 0xff }}
        }; // backup
        */
        /*
         * 
         * 
         * 
         * 
         * .
         * 
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
         * bool[0,1] is the instrustion set, {0x1, 0x1}
         * meaning the code is (1, whatever is to the left to perent node)
         * 
         */

        public class Node
        {
            public Node p; // perent
            public Node[] c; // children
            public int r; // depth
            public byte pos; // gives relitive position 
            public bool isFather;


            // prefomance shit
            
            public Node FindNeighbor(byte direction)
            {
                byte[] code;
                Node n = this; 
                byte d;

                code = new byte[currentDepth];
                d = direction; 


                int i = 0;
                while (true)
                {
                    code[i] = quadLookUp[(n.pos << 3) + d];
                    if (n.p.isFather == true)
                        break;
                    d = quadLookUp[(n.pos << 3) + d + 1];
                    if (d == 0xff)
                        break;
                    n = n.p;
                    i++;
                    
                }
                code[i] = 0xff; // return marker
                n = FindNodeFromRef(this, code);

                return n;
            }
            public Node FindNeighborO(byte direct)
            {
                List<byte> code = new List<byte>();
                Node n; // curent node, default is this
                byte d = direct;
                n = this;
                while (n.p != null)
                {
                    //case
                    code.Add(quadLookUpO[n.pos, d, 0]);
                    d = quadLookUpO[n.pos, d, 1];
                    if (d == 0xff)
                        break;
                    n = n.p;

                }
                code.Add(0xff);
                return FindNodeFromRef(this, code.ToArray());

            }

            public Node FindNodeFromRef(Node n, byte[] add)
            {


                int i = 0;
                while (true)
                {
                    //Debug.Log(i);
                    if (add[i] == 0xff)
                        break;
                    if (n.isFather == true)
                        break;
                    n = n.p;
                    i++;
                }
                
                

                string addString = "";
                foreach (byte part in add)
                {
                    addString += " " + (int)part;
                }
                //Debug.Log(addString);
                return n;

                
            } // i think this is broken

            public Node CreateNodeTree()
            {
                Node n = new Node
                {
                    isFather = true
                };
                Node q;

                for (int a = 0; a < 4; a++) {
                    q = new Node();
                    n.c[a] = q;
                    q.p = n;
                    q.pos = (byte)a;
                    q.r = 1;
                    for (int b = 0; b < 4; b++)
                    {
                        q = new Node();
                        n.c[a].c[b] = q;
                        q.p = n.c[a];
                        q.pos = (byte)b;
                        q.r = 2;
                        for (int c = 0; c < 4; c++)
                        {
                            q = new Node();
                            n.c[a].c[b].c[c] = q;
                            q.p = n.c[a].c[b] ;
                            q.pos = (byte)c;
                            q.r = 3;
                        }
                    }
                }

                return n;
            } // debug
            public Node()
            {
                c = new Node[4];
            }
            public void ExpandQuadtree(byte direc)
            {
                
            }
            public NodeS ToNodeS()
            {
                return new NodeS
                {
                    r = this.r,
                    pos = this.pos,
                    isFather = this.isFather
                };
            }
            public int ChildNodeCount()
            {
                
                if (this.c[0] == null)
                    return 1;
                int e = 0;
                for (int i = 0; i < 4; i++)
                {
                    e += c[i].ChildNodeCount();
                }
                e++;
                return e;
            } // gets the amount of nodes under this one
            public void GetPos(bool sphere)
            {
                // janky shit
                Vector3 vec = new Vector3(0, 0, 1);

                Node f = this;
                int i = (int)math.pow(2, (currentDepth - r)); // depth 
                while (true)
                {
                    if (f.p == null)
                        break;
                    vec += new Vector3(i * (f.pos & 0x1), i * (f.pos & 0x2));
                    f = f.p;
                    i *= 2;
                }
                vec = new Vector3(vec.x, vec.y / 2);
                if(sphere)
                {
                    Gizmos.DrawSphere(vec, 0.25f);
                }
                else {

                    Gizmos.DrawWireCube(vec, new Vector3(Mathf.Pow(2, currentDepth - r) / 2, Mathf.Pow(2, currentDepth - r) / 2, 10 - r));
                }
            }
            
        }
        public unsafe struct NodeS
        {
            public int p; // index of p
            public fixed int c[4];
            public int r;
            public int pos;
            public bool isFather;
        } // a struct respresentaion of the Node class
        public ComputeBuffer NodeTreeToBuffer(Node n)
        {
            unsafe
            {
                int count = n.ChildNodeCount();
                ComputeBuffer ret = new ComputeBuffer(count, sizeof(NodeS));

                NodeS[] arr = new NodeS[count];
                arr = computeOrder(arr, 0, n);
                arr[0] = n.ToNodeS();
                
                 

                return ret;
            }
        }
        public unsafe NodeS[] computeOrder(NodeS[] arr, int i, Node n)
        {
            for(int u = 0; u < arr.Length; u++)
            {
                if (arr[u].c[0] != 0)
                    continue;
                for (int y = 0; y < 4; y++)
                {
                    NodeS node = n.c[y].ToNodeS();
                    node.p = i;
                    arr[u + y] = node; // set child
                    
                }
            }
            return null;
        }
        public QuadTree BufferToNodeTree()
        {
            return null;
        }
    } // haha refactor 
}
   