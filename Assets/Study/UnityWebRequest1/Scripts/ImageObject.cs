using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageObject : MonoBehaviour
{
    private float speed = 10f;
    private float upSideLimit = 5f;

    private Transform baseTransform;

    private void Awake()
    {
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

    public void SetSpeed(float speed) => this.speed = speed;
}
