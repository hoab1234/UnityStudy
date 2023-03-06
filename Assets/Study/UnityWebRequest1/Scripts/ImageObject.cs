using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageObject : MonoBehaviour
{
    private float speed;
    private float upSideLimit = 5f;

    private Renderer[] Renderers;

    private void Update()
    {
        transform.localPosition += Vector3.up * speed * Time.deltaTime;

        if(transform.localPosition.y > transform.parent.position.y + upSideLimit)
        {
            transform.localPosition = transform.parent.position;
        }
    }

    public void Init(Texture texture, float speed) {
        if (Renderers == null)
        {
            Renderers = GetComponentsInChildren<Renderer>();
        }

        foreach(var renderer in Renderers)
            renderer.material.mainTexture = texture;
        
        this.speed = speed;
        transform.localScale = new Vector3(texture.width * 0.001f, texture.height * 0.001f, 1f);
    }
}
