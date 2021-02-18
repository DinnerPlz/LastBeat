using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;
using System.Diagnostics;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static byte[] quadLookUp = new byte[] {
            0x1, 0xff ,0x1, 0x1 ,0x2, 0xff ,0x2, 0x3 ,
            0x0, 0x0 ,0x0, 0xff ,0x3, 0xff ,0x3, 0x3 ,
            0x3, 0xff ,0x3, 0x1 ,0x0, 0x2 ,0x0, 0xff ,
            0x2, 0x0 ,0x2, 0xff ,0x1, 0x2 ,0x1, 0xff 
        }; // pos 0x3

    public Text t;


    float deltaTime = 0.0f;
    float fps = 0.0f;


    QuadTree.Node n;
    QuadTree.Node e;
    // Start is called before the first frame update
    void Start()
    {
        n = new QuadTree.Node();
        n = n.CreateNodeTree(); // might work

        
        
        

    }

    // Update is called once per frame
    void Update()
    {
        Stopwatch w = new Stopwatch();

        deltaTime += Time.deltaTime;
        deltaTime /= 2.0f;
        fps = 1.0f / deltaTime;
        w.Start();

        for (int u = 0; u < 1000; u++)
        {
            for (int a = 0; a < 4; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        e = n.c[a].c[b].c[c];
                        for (byte i = 0x0; i != 0x4; i += 0x1)
                        {
                            QuadTree.Node fuck = e.FindNeighbor(i);
                        }
                    }
                }
            }
        }
        t.text = fps.ToString();

        w.Stop();


        /*
        w.Start();

        byte b1;

        for (int i = 0; i < 256000; i++)
        {
            for (int u = 0; u < 4; u++)
            {
                for (int q = 0; q < 4; q++)
                {
                    b1 = quadLookUp[u * 8 + q + 1];
                }
            }
        }

        w.Stop();
        */
        UnityEngine.Debug.Log(w.ElapsedMilliseconds + " ms");
    }
}
