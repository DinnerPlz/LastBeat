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

    public List<float> avgRunTime = new List<float>();
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

        byte[] add0 = new byte[5];
        byte pos = 0x3;

        for (int i = 0; i < 256000; i++)
        {
            for (int u = 0; u < 4; u++)
            {
                for (int q = 0; q < 4; q++)
                {
                    add0[0] = (byte)(pos == 0x0 ? 0x1 : pos == 0x1 ? 0x0 : pos == 0x2 ? 0x3 : 0x2);
                    add0[1] = add0[0];
                    add0[2] = (byte)(pos == 0x0 ? 0x2 : pos == 0x1 ? 0x3 : pos == 0x2 ? 0x0 : 0x1);
                    add0[3] = add0[1];
                }
            }
        }
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
}
