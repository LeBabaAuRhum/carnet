using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureTaille : MonoBehaviour
{
    public float factor = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
}

    // Update is called once per frame
    void Update()
    {
        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(scaleX, scaleY);
    }
}
