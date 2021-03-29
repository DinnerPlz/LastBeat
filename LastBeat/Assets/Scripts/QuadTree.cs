using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;
using System;

// this is a a remake of QuadTreeCpu cuz it was shit
// just comments and organizational stuff
// i pray to anyone who needs to read this code

namespace QuadTree
{
    public class QuadTree
    {
        static public int currentDepth = 0;

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

        // i dont know which of these i need

        public class Node
        {
            public Node p; // the perent of node
            public Node[] c; // children
            public int depth;
            public int pos; // relitive position of node compared to perent
            public bool isFather; // if the node has perent, faster than p == null

            public bool[] rock = new bool[2];
            // [Buffer one] [Buffer two]

            Color col;

            public int buff; 
            // if 0 read from buffer one, and write to buffer two
            // if 1 read from buffer two, and write to buffer one

            public void SplitNode()
            {
                for(int i = 0; i < 4; i++)
                {
                    c[i] = new Node();
                    c[i].depth = depth + 1;
                    c[i].p = this;
                    c[i].pos = (byte)i;
                }
                if (c[0].depth > currentDepth)
                    currentDepth = c[0].depth;
            }
            public void Expand(byte newPos)
            {
                if (isFather == false)
                    return; // dont expand if perent exists
                p = new Node(); // make pernet
                for (int i = 0; i < 4; i++)
                {
                    p.c[i] = (byte)i == newPos ? this : new Node();
                } // child def


                //other vars def
                p.depth = depth - 1;
                p.isFather = true;
                isFather = false;
                //currentDepth += 1;

            } // creates a perent node, with the existing node at position NewPos withing the perents children
            private Node FindNeighbor(byte direct)
            {
                byte[] code = new byte[100]; // this number needs to be generated in feauter
                Node n; // a "pointer" for the nodes
                byte d = direct;
                n = this;
                int i = 0;
                while (n.p != null)
                {
                    code[i] = quadLookUpO[n.pos, d, 0];
                    d = quadLookUpO[n.pos, d, 1];
                    if (d == 0xff)
                        break;
                    n = n.p;
                    i++;
                }
                code[i++] = 0xff;
                

                return FindNodeFromRef(this, code);
                // optimizations, lists are very slow
            } // remove lists and use lower D array for FSM
            // dont feel like fixing rn
            private Node FindNodeFromRef(Node n, byte[] add)
            {
                int i = 0;
                n = n.p; // i dont know why this is nessisary
                while (true)
                {
                    if (add[i + 1] == 0xff)
                        break; // break if addres ends
                    if (n.isFather == true)
                        break; // break if this is the top node
                    n = n.p;
                    i++;
                } // this goes up the tree as far as needed
                // this wraps edge to edge, dont know how to fix

                while (i >= 0)
                {
                    n = n.c[add[i]]; // this goes down the node, according to the address
                    i--;
                } // goes down tree, according to instructions from add

                return n; // this is the requseted node
            } // gets node via relitive address
            public Node CreateNodeTree(int depth, int id, Node n)
            {
                if (id == depth)
                    return null;


                if (n == null)
                {
                    n = new Node
                    {
                        isFather = true,
                        depth = 0
                    };
                    currentDepth = -id; // i think, i should test this
                }
                    



                n.SplitNode();
                for (int i = 0; i < 4; i++)
                {
                    n.c[i] = CreateNodeTree(depth, id + 1, n.c[i]);
                }

                return n;
            } // recursivly make tree
            public int ChildNodeCount()
            {

                if (c[0] == null)
                    return 1;
                int e = 0;
                for (int i = 0; i < 4; i++)
                {
                    e += c[i].ChildNodeCount();
                }
                e++;
                return e;
            } // gets the amount of nodes under this one
            public Texture2D RenderToTexture2D()
            {
                // function assumes full quadtree rn
                int tSize = -depth + currentDepth; // total depth idk 
                int length = (int)Mathf.Pow(2, tSize );// side length
                int e = length;
                Texture2D tex = new Texture2D(length,length);
                tex.filterMode = FilterMode.Point;
                

                for (int x = 0; x < length; x++)
                {
                    for (int y = 0; y < length; y++)
                    {
                        tex.SetPixel(x, y, Color.white);
                    }
                }
                tex.Apply();

                byte[] add = new byte[tSize]; // used for addressing



                // recurse thorugh tree and output image
                RenderRecurse(tex, add, 0); // beging recusion at id = 0



                tex.Apply(); // apply changes make to textue
                if (buff == 0)
                {
                    buff = 1;
                }
                else
                {
                    buff = 0;
                }

                return tex;
            }
            public Texture2D RenderRecurse(Texture2D tex, byte[] add, int id)
            {
                id++; //increment id

                //double e = Math.Sqrt(1356);


                // the address is only used at the end of the tree to get a postion
                if (c[0] == null)
                {
                    ComputeNode();

                    DrawNode(add, ref tex);

                    return tex;
                    {
                        Node[] neighbors = new Node[4];

                        for (byte i = 0; i < 0x4; i++)
                        {

                            neighbors[i] = FindNeighborO(i);
                            if (rock[buff])
                            {
                                neighbors[i].col = Color.red;


                                //neighbors[i].col = Color.magenta;
                                if (pos != i)
                                {
                                    p.c[i].col = Color.red;
                                    //Debug.Log(depth + " " + p.depth);
                                    Node req; // ussed for addressing
                                    req = p.c[i]; // p.c[i] reffers to brothers
                                    string str = "";
                                    while (true)
                                    {
                                        str = req.pos + " " + str;
                                        if (req.p == null)
                                            break;
                                        req = req.p;
                                    }
                                    Debug.Log(str);
                                }


                            }



                        }
                        if (col != Color.red)
                        {
                            col = Color.white;
                        } // caution hacky shit, fix later

                        int x = 0; // pos
                        int y = 0; // pos
                        for (int i = 1; i < add.Length; i++)
                        {

                            x += (add[i] & 0x1) << (depth - i + 1);//why 1? WHO FUCKING KNOWS
                            y += (add[i] & 0x2) << (depth - i);
                        }

                        // col.a = 255;
                        if (rock[buff])
                        {
                            col = Color.yellow;
                        }

                        //col.g = (x % 3) == 0 ? 1f : 0f;
                        //col.b = (float)y / 32f;


                        // copute shit


                        tex.SetPixel(x, y, col);
                        return tex;
                    } // might need, not commenting tho cuz shit breaks

                } // set some sort of color
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        add[id] = (byte)i; // further address for next recurese

                        if (i == 4)
                        {
                            Debug.Log(add);
                        }
                        c[i].RenderRecurse(tex, add, id);
                    } // recurse
                } // go further down tree
                

                return tex;
            }
            void ComputeNode()
            {
                
                col.r = UnityEngine.Random.Range(0f,1f);
                col.b = UnityEngine.Random.Range(0f,1f);
                col.g = UnityEngine.Random.Range(0f,1f);
                
                //Debug.Log(col);
            }
            void DrawNode(byte[] add, ref Texture2D tex)
            {
                int x = 0; // pos
                int y = 0; // pos
                for (int i = 1; i < add.Length; i++)
                {

                    x += (add[i] & 0x1) << (depth - i + 1);//why 1? WHO FUCKING KNOWS
                    y += (add[i] & 0x2) << (depth - i);
                }
                tex.SetPixel(x, y, col);
            }
            public Node()
            {
                c = new Node[4];
            }

            public void DrawTree()
            {
                GetPos(false, 0);
                if (c[1] == null)
                    return;
                for (int i = 0; i < 4; i++)
                {
                    c[i].DrawTree();
                }
            } // draw tree using gizmos (slow asf)
            private void GetPos(bool sphere, float offSet)
            {
                // janky shit
                Vector3 vec = new Vector3(0, 0, 1);

                Node f = this;
                int i = (int)math.pow(2, (currentDepth - depth)); // depth 
                while (true)
                {
                    if (f.p == null)
                        break;
                    vec += new Vector3(i * (f.pos & 0x1), i * (f.pos & 0x2));
                    f = f.p;
                    i *= 2;
                }
                vec = new Vector3(vec.x, vec.y / 2);
                if (sphere)
                {
                    Gizmos.DrawSphere(vec + new Vector3(0, 0, offSet * 2), 0.25f - offSet / 4);
                }
                else
                {

                    Gizmos.DrawWireCube(vec, new Vector3(Mathf.Pow(2, currentDepth - depth) / 2, Mathf.Pow(2, currentDepth - depth) / 2, 10 - depth));
                }
            } // visualization for debug (shity code)
            private Node FindNeighborO(byte direct)
            {
                // workes using a FSM
                List<byte> code = new List<byte>();
                Node n; // a "pointer" for the nodes
                byte d = direct;
                n = this;
                while (n.p != null)
                {
                    code.Add(quadLookUpO[n.pos, d, 0]);
                    d = quadLookUpO[n.pos, d, 1];
                    if (d == 0xff)
                        break;
                    n = n.p;
                }
                code.Add(0xff); // make sure that the code ends
                                // this is mostly just a saefty so infine recurtion doesnt happen

                return FindNodeFromRef(this, code.ToArray());

            } // the original function FindNEighbor does not work

        } // main node class
        public class Compute
        {
            public class Execute
            {

            } // 
        }
        public unsafe struct NodeS
        {

        } // struct version of node, for compute buffers

    }
    public unsafe class QuadTreePtr
    {
        public unsafe struct Node
        {
            // address are to be added to &Node

            public fixed long c[4]; // childeren address
            // i cant have an array of node* (sad)
            // convert form long to void*:
            // void* p = (void*)c[i];
            // address 0, 8, 16, 24

            public Node* p; //perent
            // address 32

            public int depth;
            //address  40

            public byte pos;
            //address  44

            public bool isFather;
            //address  45



        }
        public class Generate
        {
            
        }
        public unsafe void Split(Node n)
        {
            Node[] childs = new Node[4];
            fixed (void* arrayStart = &childs[0])
            {
                long add = (long)new IntPtr(arrayStart); // address of array start
                for (int i = 0; i < 4; i++)
                {
                    childs[i] = new Node();
                    childs[i].depth = n.depth + 1;
                    childs[i].p = &n;
                    childs[i].pos = (byte)i;


                    n.c[i] = add + (i * sizeof(Node));
                }
            }
            


            
        }

    } // Quadtree except it uses pointes (kill me)
}
