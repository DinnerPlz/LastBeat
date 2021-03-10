using Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// used to set a all objects under it matirials to normal, and back to original state
// the functions in this script are called by other scripts


public class MatControl : MonoBehaviour
{
    [SerializeField]
    List<Material> mats;
    [SerializeField]
    List<Transform> trans;
    [SerializeField]
    List<Renderer> rens;
    public Material normMat;
    public void SetNormal()
    {
        // converts all mats underneth to a normal render
        for (int i = 0; i < mats.Count; i++)
        {
            rens[i].material = normMat;
        }
    }
    public void SetOriginal()
    {
        // resets all objects to original matiral
        for (int i = 0; i < mats.Count; i++)
        {
            rens[i].material = mats[i];
        }
    }
    public void ReadMats()
    {
        
        int i = 0;
        mats = new List<Material>();
        trans = new List<Transform>();
        rens = new List<Renderer>();
        // reads all mats under to a list
        trans = transform.GetAllChildren(null);
        foreach(Transform tran in trans)
        {
            Renderer ren = tran.GetComponent<Renderer>();
            if (ren == null)
                continue;
            if (ren.material.shader.name == "Standard")
            {
                // create unlit material for object
                Material mat;
                mat = new Material(Shader.Find("Unlit/Color"));
                mat.color = ren.material.color;
                ren.material = mat;
            }
            rens.Add(ren);
            mats.Add(ren.material);

        }
            /*
        foreach (Transform cchi in transform)
        {
            
            foreach (Transform child in cchi)
            {
                Renderer ren = child.gameObject.GetComponent<Renderer>();
                if (ren == null)
                {
                    return;
                }

                if (ren.material.shader.name == "Standard")
                {
                    // create unlit material for object
                    Material mat;
                    mat = new Material(Shader.Find("Unlit/Color"));
                    mat.color = ren.material.color;
                    ren.material = mat;

                }

                rens.Add(ren);
                mats.Add(ren.material);
                gms.Add(child.gameObject);
                i++;
            }
            

        }
        */

    }
    
}
