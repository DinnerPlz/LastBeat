using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class tfgs : MonoBehaviour
{

    public Main m;
    RawImage RW;
    
    // Start is called before the first frame update
    void Start()
    {
        RW = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        RW.texture = m.tex;
    }
}
