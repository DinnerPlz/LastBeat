using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;


/*
 * No idea where to start with this project tbh
 * 
 * Quadtree using N space filing curve
 * 
 * oh boy here we go pseudo code time
 * 
 * The purpose of this implementaion is to made a quadtree, with varying levels of complexity
 * all methods used in this implentation must be possible on a GPU meaning no classes or pointers
 * Of course this limitaion only includes the non initialization and looping code, as those will be CPU bound 
 * in the final project
 * 
 * 
 * The goal of this system is to create a quadtree data sructure that the GPU and CPU can share,
 * can be expanded during run time, hold all data required, can asses if a spit is nessiary,
 * 
 * 
 * 
 * 
 */








public class QuadTreeCPU : MonoBehaviour
{
    struct node
    {
        // represents one node of a system, and id n stuff idk
        static float3 i;





    };

    ComputeBuffer Buff1;
    ComputeBuffer Buff2;

    // Start is called before the first frame update
    void Start()
    {
        unsafe
        {
            Buff1 = new ComputeBuffer(1, sizeof(node));
            Buff2 = new ComputeBuffer(1, sizeof(node));
            // this creats a single leaf node0
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
