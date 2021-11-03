using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [SerializeField] private GameObject player_go;
    [SerializeField] private Transform player_parent;
    [SerializeField] private Transform spawn_point;
    [SerializeField] private CinemachineVirtualCamera starting_virtual_cam;
    [SerializeField] private CinemachineConfiner2D confiner2D;
    [SerializeField] private ProgressBar stamina_bar;
    
    [Header("Difficulity Settings")]
    private ChallengeModifier challenge_controller;
    [SerializeField] private List<DifficultyModiferObject> hide_list;

    private NodeGrid _grid = null;
    private Pathfinding _ai = null; 

    private GameObject player;

    private float health = 1;

    public void SetGrid(NodeGrid newGrid)
    {
        _grid = newGrid;
    }

    public int getDifficultyLevel() {
        if (challenge_controller == null) {throw new ArgumentException("Challenge Controller is null!");}
        return challenge_controller.currentGameDifficulty;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        List<ChallengeModifier> list = FindObjectsOfType<ChallengeModifier>().ToList();
        ChallengeModifier active;
        if (list.Count > 1) {
            if (list.Aggregate(true,(_b,_c) => _b = _c.currentGameDifficulty == 0 ? false : _b)) {
                active = list.Where(_e=>_e.currentGameDifficulty != 0).ToList()[0];
            } else {
                active = list[0];
            }
            List<ChallengeModifier> kill_list = list.Where(_t => _t != active).ToList();
            kill_list.ForEach(_e => Destroy(_e.gameObject));
        } else { active = list[0];}

        challenge_controller = active;

    }

    // Start is called before the first frame update
    void Start()
    {
        //Hide objects based on difficulty level
    
        hide_list.Where(_e => challenge_controller.currentGameDifficulty >= _e.targetDifficultyLevel).ToList().ForEach(_s=>_s.targetObject.SetActive(false));
       

        player = Instantiate(player_go,spawn_point.position, Quaternion.identity, player_parent);
        starting_virtual_cam.Follow = player.transform;
        
        // Load the Grid
        SceneManager.LoadScene("Grid", LoadSceneMode.Additive);
    }

    public Pathfinding GetPathAI()
    {
        if (_ai == null && _grid != null)
        {
            _ai = new Pathfinding(GetGrid().Nodes);
        }
        
        return _ai;
    }
    
    public NodeGrid GetGrid()
    {
        if (_grid == null) 
        {
            _grid = FindObjectOfType<NodeGrid>();
            if(_grid == null) Debug.Log("Failed to load Grid in LevelController");
        }
        return _grid;
    }

    public void ChangeCameraConstraint(PolygonCollider2D _contraint) {
        confiner2D.m_BoundingShape2D = _contraint;
    }

    public void ToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void ToEndScreen() {
        challenge_controller.reset();
        SceneManager.LoadScene(3);
    }

    public void ToWinScreen() {
        SceneManager.LoadScene(2);
    }

    public void UberEndingCheck() {
        if (challenge_controller.currentGameDifficulty == 4) {
            challenge_controller.reset();
            return;
        }
        challenge_controller.increase();
        ToWinScreen();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMainMenu();
        }
    }
}
