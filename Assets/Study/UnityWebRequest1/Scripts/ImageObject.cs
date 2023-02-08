using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageObject : MonoBehaviour
{
    private float speed = 10f;
    private float upSideLimit = 5f;

    private Transform baseTransform;
    public MeshRenderer Mesh;

    private void Awake()
    {
        Mesh = GetComponent<MeshRenderer>();
        baseTransform = transform.parent;
    }

    private void Update()
    {
        transform.localPosition += Vector3.up * speed * Time.deltaTime;

        if(transform.localPosition.y > upSideLimit)
        {
            transform.localPosition = baseTransform.position;
        }
    }

    public void SetImage(Texture texture = null) => Mesh.material.mainTexture = texture;

    public void SetSpeed(float speed) => this.speed = speed;
}
