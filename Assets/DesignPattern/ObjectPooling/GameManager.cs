using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BulletPoolManager bulletPoolManager;
    public Button createBulletButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        createBulletButton.onClick.AddListener(() => bulletPoolManager.FireBullet());
    }
}
