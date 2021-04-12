﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class FluidSimDispatch : MonoBehaviour
{
    RenderTexture _target;

    ComputeBuffer _data0;
    ComputeBuffer _data1;

    public ComputeShader shader;

    public int buff = 0;

    public int size; // must be multipe of 8 or shit breaks

    public bool render;
    public bool stepRender;
    public bool reset;

    struct cell {
        float2 u; // vector at one pos
        float dens; // desnisty

        float soures; // where some is emitted
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        DispatchShader(destination);
    }
    void DispatchShader(RenderTexture destination)
    {
        size = size - (size % 8); // make sure size is multiple of 8
        if (render)
        {
            buff = buff == 1 ? 0 : 1;
            if (stepRender)
                render = false;
            if (reset == true)
                reset = false;
            ComputeTools.InitRenderTexture(ref _target, new Vector2Int(size + 16, size + 16));
            ComputeTools.InitStructedBuffer<cell>(ref _data0, new Vector2Int(size, size));
            ComputeTools.InitStructedBuffer<cell>(ref _data1, new Vector2Int(size, size));

            shader.SetBuffer(0, "data0", _data0);
            shader.SetBuffer(0, "data1", _data1);
            shader.SetInt("N", size);
            shader.SetInt("buff", buff);
            shader.SetTexture(0, "Result", _target);
            int threadGroupsX = Mathf.CeilToInt((float)size / 8.0f);
            int threadGroupsY = Mathf.CeilToInt((float)size / 8.0f);
            shader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
            Render(destination);
        }
       
        
    }
    private void Render(RenderTexture destination)
    {
        // compute shit

        Graphics.Blit(_target, destination);
    }
    private void OnApplicationQuit()
    {
        _data1.Release();
        _data0.Release();
        _target.Release();
    }

}