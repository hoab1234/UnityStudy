using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageObject : MonoBehaviour
{
    private float speed;
    private float upSideLimit = 5f;


    //필요없이 더 큰 자식 클래스를 생성할 필요없으므로 올바른 데이터 타입을 가지고 왔는지 확인할 것
    private Renderer[] Renderers;

    // 코드 길이, 가독성과 캐싱을 위한 변수 추가, 데이터 할당 뭐가 더 나은지 생각해야하고 그냥 캐싱이 좋겟지같은 단순하게 생각하고 끝내지 말기
    private void Update()
    {
        transform.localPosition += Vector3.up * speed * Time.deltaTime;

        if (transform.localPosition.y > transform.parent.position.y + upSideLimit)
        {
            transform.localPosition = transform.parent.position;
        }
    }

    public void Init(Texture texture, float speed)
    {
        if (Renderers == null)
        {
            Renderers = GetComponentsInChildren<Renderer>();
        }

        foreach (var renderer in Renderers)
            renderer.material.mainTexture = texture;

        this.speed = speed;
        transform.localScale = new Vector3(texture.width * 0.001f, texture.height * 0.001f, 1f);
    }
}
