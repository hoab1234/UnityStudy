using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    IObjectPool<GameObject> pool;
    private float lifeTime = 5f;

    public void SetPool(IObjectPool<GameObject> bulletPool)
    {
        pool = bulletPool;
    }

    public void Init()
    {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        pool.Release(gameObject);
    }
}
