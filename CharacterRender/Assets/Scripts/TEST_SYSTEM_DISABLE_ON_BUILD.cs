#define DEBUG

#if DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;



public class TEST_SYSTEM_DISABLE_ON_BUILD : MonoBehaviour
{
    public RenderTexture ren;
    public Texture2D tex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tex = ren.ToTexture2D();
        tex.Apply();
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0 , tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}

#endif