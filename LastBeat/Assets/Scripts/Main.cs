using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;

public class Main : MonoBehaviour
{
    QuadTree.Node n;
    QuadTree.Node e;
    // Start is called before the first frame update
    void Start()
    {
        n = new QuadTree.Node();
        n = n.CreateNodeTree(); // might work

        e = n.c[1].c[2].c[2];
        for (byte i = 0x0; i != 0x8; i +=0x1)
        {
          
        }
        QuadTree.Node fuck = e.FindNeighbor(0x1);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
