using System;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    private ObjectPool<GameObject> bulletPool;
    public GameObject bulletPrefab;

    private void Awake()
    {
        bulletPool = new ObjectPool<GameObject>
            (
                createFunc: CreateBullet, // 객체 생성
                actionOnGet: OnGetBullet, // 풀에서 객체 가져올 때 실행
                actionOnRelease: OnReleaseBullet, // 풀에서 객체 반환할 때 실행
                actionOnDestroy: OnDestroyBullet, // 풀이 가득 찼을 때 실행
                defaultCapacity: 10, // 초기 풀 크기
                maxSize: 20  // 최대 풀 크기
            );
    }

    private void OnDestroy()
    {
        if (bulletPool != null)
        {
            bulletPool.Clear();
        }
    }

    private GameObject CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab, transform);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = UnityEngine.Random.rotationUniform;
        bullet.AddComponent<Bullet>().SetPool(bulletPool);
        return bullet;
    }

    private void OnGetBullet(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = UnityEngine.Random.rotationUniform;
    }

    private void OnReleaseBullet(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyBullet(GameObject obj)
    {
        Destroy(obj);
    }

    public void FireBullet()
    {
        var bullet = bulletPool.Get();
        bullet.GetComponent<Bullet>().Init();
    }
}
