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

        /*
        public static byte[,,] quadLookUp1 = new byte[,,] {
            {{0x1, 0xff },{0x1, 0x1 },{0x2, 0xff },{0x2, 0x3 },{0x3, 0x3 },{0x3, 0xff },{0x3, 0x1 },{0x3, 0x7 }},
            {{0x0, 0x0 },{0x0, 0xff },{0x3, 0xff },{0x3, 0x3 },{0x2, 0x4 },{0x2, 0xff },{0x2, 0xff},{0x2, 0x1 }},
            {{0x3, 0xff },{0x3, 0x1 },{0x0, 0x2 },{0x0, 0xff },{0x1, 0xff },{0x1, 0x2 },{0x1, 0x6 },{0x1, 0x0 }},
            {{0x2, 0x0 },{0x2, 0xff },{0x1, 0x2 },{0x1, 0xff },{0x0, 0x0 },{0x0, 0x5 },{0x0, 0x2 },{0x0, 0xff }}
        }; // backup
        */
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
                //n = FindNodeFromRef(this, code.ToArray());

                return n;
            }
            /*
            public Node[] FindNeighborP()
            {
                Node[] neighbors = new Node[4] { null, null, null, null}; // working on addreses currently


                byte[] add0 = new byte[currentDepth];
                byte[] add1 = new byte[currentDepth];
                byte[] add2 = new byte[currentDepth];
                byte[] add3 = new byte[currentDepth];

                //  R, L , U, D

                Node n0 = this;
                Node n1;
                Node n2;
                Node n3;

                
                public readonly static byte[] quadLookUp = new byte[] {
                    0x1, 0xff ,0x1, 0x1 ,0x2, 0xff ,0x2, 0x3 ,
                    0x0, 0x0 ,0x0, 0xff ,0x3, 0xff ,0x3, 0x3 ,
                    0x3, 0xff ,0x3, 0x1 ,0x0, 0x2 ,0x0, 0xff ,
                    0x2, 0x0 ,0x2, 0xff ,0x1, 0x2 ,0x1, 0xff 
                    }; // pos 0x3
                    
                switch (pos)
                {
                    case 0x0:
                        add0[0] = 0x1;
                        add1[0] = 0x1;
                        add2[0] = 0x2;
                        add3[0] = 0x2;
                        break;
                    case 0x1:
                        add0[0] = 0x0;
                        add1[0] = 0x0;
                        add2[0] = 0x3;
                        add3[0] = 0x3;
                        break;
                    case 0x2:
                        add0[0] = 0x3;
                        add1[0] = 0x3;
                        add2[0] = 0x0;
                        add3[0] = 0x0;
                        break;
                    case 0x3:
                        add0[0] = 0x2;
                        add1[0] = 0x2;
                        add2[0] = 0x1;
                        add3[0] = 0x1;
                        break;
                }
                switch (pos) // only two neighbors need further prossesing
                {
                    case 0x0:
                        proccessAdd(ref add2, pos, 0x1);
                        proccessAdd(ref add3, pos, 0x3);
                        break;
                    case 0x1:
                        proccessAdd(ref add0, pos, 0x0);
                        proccessAdd(ref add3, pos, 0x3);
                        break;
                    case 0x2:
                        proccessAdd(ref add1, pos, 0x1);
                        proccessAdd(ref add2, pos, 0x2);
                        break;
                    case 0x3:
                        proccessAdd(ref add0, pos, 0x0);
                        proccessAdd(ref add2, pos, 0x2);
                        break;
                }
                

                return neighbors;
            } // parallelization for fing neighbor
            public void proccessAdd(ref byte[] add, byte pos, byte direction)
            {
                Node n = this.p; // perent because the first byte is already proccesed
                byte d;
                d = add[0];

                int i = 1;
                while (true)
                {
                    add[i] = quadLookUp[(n.pos << 3) + d];
                    if (n.p.isFather == true)
                        break;
                    d = quadLookUp[(n.pos << 3) + d + 1];
                    if (d == 0xff)
                        break;
                    n = n.p;
                    i++;

                }
            }
            */ // was slower :\

            public Node FindNodeFromRef(Node n, byte[] add)
            {

                // finds a node relitive to another
                for (int i = 0; i < add.Length; i++)
                {
                    if (n.p == null)
                        return null; // address is invalid
                    n = n.p;
                } // goes up to perent
                for (int i = 0; i < add.Length; i++)
                {
                    n = n.c[add[i]];
                }
                return n;
            }

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
                    for (int b = 0; b < 4; b++)
                    {
                        q = new Node();
                        n.c[a].c[b] = q;
                        q.p = n.c[a];
                        q.pos = (byte)b;
                        for (int c = 0; c < 4; c++)
                        {
                            q = new Node();
                            n.c[a].c[b].c[c] = q;
                            q.p = n.c[a].c[b] ;
                            q.pos = (byte)c;
                        }
                    }
                }

                return n;
            } // debug
            public Node()
            {
                c = new Node[4];
            }
        }
    } // haha refactor 
}
   