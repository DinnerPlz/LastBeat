using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuadTree.QuadTree;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static byte[] quadLookUp = new byte[] {
            0x1, 0xff ,0x1, 0x1 ,0x2, 0xff ,0x2, 0x3 ,
            0x0, 0x0 ,0x0, 0xff ,0x3, 0xff ,0x3, 0x3 ,
            0x3, 0xff ,0x3, 0x1 ,0x0, 0x2 ,0x0, 0xff ,
            0x2, 0x0 ,0x2, 0xff ,0x1, 0x2 ,0x1, 0xff
    }; // pos 0x3

    public Texture2D tex;



    public bool step;

    Node n;
    [Range(0,3) ]
    public int f, g, h;
    [Range(0, 10)]
    public int depth;

    public int pDepth;
    
    [Range(0, 3)]
    public int[] pos;

    public List<float> avgRunTime = new List<float>();
    // Start is called before the first frame update
    void Start()
    {


        n = new Node
        {
            isFather = true
        };
        //n = n.CreateNodeTree(); // does  work
        //n = n.CreateNodeTree(depth, 0, null);
        //n = n.CreateNodeTree(depth, 0, null);

        n = new Node();
        n = n.CreateNodeTree(depth, 0, null);

        n.ToBuffer();
    }
    private void Update()
    {
        
        if(step)
        {
            //n = new Node();
            //n = n.CreateNodeTree(depth, 0, null);
            if(pDepth != depth)
            {
                pDepth = depth;
                n = n.CreateNodeTree(depth, 0, null);
            }
            
            Node node = n;
            for (int i = 0; i < depth-1; i ++)
            {
                node = node.c[pos[i]];
            }
            node.rock = new bool[] { true, true };
            tex = n.RenderToTexture2D();
            tex.Apply();
            //step = false;
        }
        
    }

    
}
