using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject player_go;
    [SerializeField] private Transform player_parent;
    [SerializeField] private Transform spawn_point;
    [SerializeField] private CinemachineVirtualCamera starting_virtual_cam;
    [SerializeField] private CinemachineConfiner2D confiner2D;
    [SerializeField] private ProgressBar stamina_bar;

    private GameObject player;

    private float health = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = Instantiate(player_go,spawn_point.position, Quaternion.identity, player_parent);
        starting_virtual_cam.Follow = player.transform;
    }

    public void ChangeCameraConstraint(PolygonCollider2D _contraint) {
        confiner2D.m_BoundingShape2D = _contraint;
    }

    public void ToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void ToEndScreen() {
        SceneManager.LoadScene(3);
    }

    public void ToWinScreen() {
        SceneManager.LoadScene(2);
    }

    public void VentPlayerToLoaction(Transform _new_position) {
        player.transform.position = new Vector3(_new_position.position.x, _new_position.position.y, player.transform.position.z);
    }

    public void Heal() {
        health += 0.20f;
        if (health > 1) {health = 1;}
    }

    public void TakeDamage() {
        health -= 0.25f;
        if (health <= 0) {
            ToEndScreen();
        }
    }

    public float getPlayerHealth() => health;

    // Update is called once per frame
    void Update()
    {
        health -= (0.008f*Time.deltaTime);
        stamina_bar.SetProgressBarValue(health);
        if (health <= 0) {
            ToEndScreen();
        }
    }
}
