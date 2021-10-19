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

    private GameObject player;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
