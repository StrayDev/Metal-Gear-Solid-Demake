using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    private ItemPool bulletPool;
    // Start is called before the first frame update
    void Start()
    {
        if(GameController.Instance == null)
        {
            Debug.LogError("Found no GameController instance, ensure there is a GameController in the scene");
            return;
        }
        bulletPool = GameController.Instance.bulletPool;
        if(bulletPool == null)
        {
            Debug.LogWarning("Could not find BulletPool, bullets will be instantiated independently which is not efficient");
        }
    }

    public void Fire(Vector2 origin, Vector2 direction, float pz)
    {
        GameObject bullet;
        if(bulletPool == null)
        {
            bullet = Instantiate(bulletPrefab);
        }
        else
        {
            bullet = bulletPool.GetFirstAvailableItem();
        }
        bullet.transform.position = new Vector3(origin.x,origin.y,pz);
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        bullet.GetComponent<Bullet>().ResetBullet();
        bullet.SetActive(true);
    }

}
