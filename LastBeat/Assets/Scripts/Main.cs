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


    public bool reBuildTree;
    public bool step;

    Node n;
    [Range(0,3) ]
    public int f, g, h;
    [Range(0, 100)]
    public int depth;

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
    }
    private void Update()
    {
        if(reBuildTree)
        {
            n = new Node();
            n = n.CreateNodeTree(depth, 0, null);
            reBuildTree = false;
            Node DownTree = new Node();
            while(true)
            {
                if (DownTree.c[0] == null)
                    break;
                DownTree = DownTree.c[1];
            }
            DownTree.rock = new bool[] { true, true };
            Debug.Log(DownTree.rock[0]);
        }
        if(step)
        {
            tex = n.RenderToTexture2D();
            tex.Apply();
            step = false;
        }
        
        n.RenderToTexture2D();
    }

    private void OnDrawGizmos()
    {



        


        n = new Node();
        //n = n.CreateNodeTree(); // does  work
         //n = n.CreateNodeTree(depth, 0 , null);
        //n.Expand(0x1);

        int i = 0;
        
        //n.DrawTree();
        /*
        n.GetPos(false, 0);
        // throwaway function for drawing tree
        for (int a = 0; a < 4; a++) {
            Gizmos.color = new Color((float)(a*64)/255, 0, 0);
            n.c[a].GetPos(false, 0);
            for (int b = 0; b < 4; b++)
            {
                Gizmos.color = new Color(0, (float)(b * 64) / 255, 0);
                n.c[a].c[b].GetPos(false, 0);
                for (int c = 0; c < 4; c++)
                {
                    
                    Gizmos.color = new Color(0,0,(float)((int)c * 64) / 255);
                    n.c[a].c[b].c[c].GetPos(false, 0);
                    i++;
                }
            }
        }
        */
        /*
        Node j = n.c[f].c[g].c[h];
        Gizmos.color = Color.white;
        j.GetPos(true, -0.25f);
        for (byte e = 0x0; e < 0x4; e++)
        {

            switch (e)
            {
                case 0x0:
                    Gizmos.color = Color.green;
                    break;
                case 0x1:
                    Gizmos.color = Color.blue;
                    break;
                case 0x2:
                    Gizmos.color = Color.black;
                    break;
                case 0x3:
                    Gizmos.color = Color.cyan;
                    break;
            }
            Node k;
            
            
            k = j.FindNeighborO(e);
            
            

            if (k != null)
                k.GetPos(true, (float)e * 0.1f );
        }
        */
    }
    
}
