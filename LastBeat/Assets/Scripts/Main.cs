﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;
using static Trees.QuadTree;
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


    Node n;
    Node e;

    public List<float> avgRunTime = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        n = new Node();
        n = n.CreateNodeTree(); // might work

        
        
        

    }

    // Update is called once per frame
    void Update()
    {
        Stopwatch w = new Stopwatch();
        /*
        deltaTime += Time.deltaTime;
        deltaTime /= 2.0f;
        fps = 1.0f / deltaTime;
        w.Start();

        UnityEngine.Debug.Log(n.ChildNodeCount());
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
                            Node fuck = e.FindNeighbor(i);
                        }
                        
                    }
                }
            }
        }
        t.text = fps.ToString();

        w.Stop();
        
        */
        
        
        
        w.Stop();


        avgRunTime.Add(w.ElapsedMilliseconds);

        if (avgRunTime.Count != 200 )
           return;

        float time = 0;
        {
            foreach(float i in avgRunTime)
            {
                if (i > 150)
                    continue;
                time += i;

            }
            time /= avgRunTime.Count;
        }


        UnityEngine.Debug.Log(time.ToString() + " ms " + avgRunTime.Count) ;
        w.Reset();
    }
    private void OnDrawGizmos()
    {

        n = new Node();
        n = n.CreateNodeTree(); // might work
        int i = 0;
        // throwaway function for drawing tree
        for (int a = 0; a < 4; a++) {
            Gizmos.color = new Color(255, 0, 0);
            n.c[a].GetPos();
            for (int b = 0; b < 4; b++)
            {
                Gizmos.color = new Color(0, 0,0 );
                n.c[a].c[b].GetPos();
                for (int c = 0; c < 4; c++)
                {
                    
                    Gizmos.color = new Color((float)(a * 64) / 255, (float)(b * 64) / 255, (float)(c * 64) / 255);
                    UnityEngine.Debug.Log((float)(i * 4) / 255);
                    n.c[a].c[b].c[c].GetPos();
                    i++;
                }
            }
        }
    }
}
