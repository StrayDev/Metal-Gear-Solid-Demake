using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 3f;
    [SerializeField]
    private float bulletLifespan = 3f;

    private float lifespanRemaining = 0f;

    public void ResetBullet()
    {
        lifespanRemaining = bulletLifespan;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * bulletSpeed * Time.deltaTime;
        if((lifespanRemaining -= Time.deltaTime) <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Start() {
        if(GameController.Instance == null)
        {
            Debug.LogError("Found no GameController instance, ensure there is a GameController in the scene");
            return;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent.gameObject.GetComponent<PlayerController>() != null) {GameController.Instance.onPlayerShot?.Invoke();}
        gameObject.SetActive(false);
    }
}
