using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public const int MAINMENU = 0;
    public const int GAME = 1;
    public const int CREDITS = 2;
    public const int GAMEOVER = 3;

    public int Level = 1;
    public int Score = 0;

    [HideInInspector]
    public bool GameOver = true;

    [SerializeField]
    private float m_timeTillStart = 1;
    [HideInInspector]
    public UnityEvent RoundStart;
    [HideInInspector]
    public UnityEvent RoundNext;
    [HideInInspector]
    public UnityEvent RoundEnd;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    [SerializeField]
    private static GameObject m_player;
    [SerializeField]
    private static GameObject m_enemy;

    public Player PlayerScript { get { return m_player.GetComponent<Player>(); } }
    public Enemy EnemyScript { get { return m_enemy.GetComponent<Enemy>(); } }

    public enum States
    {
        Attack,
        Defence
    }
    public enum Identifiers
    {
        Player,
        Enemy
    }

    [System.Obsolete("No longer supported")]
    private Vector2 m_mousePositionLastFrame = Vector2.zero;
    [System.Obsolete("No longer supported")]
    private Vector2 m_mousePositionCurrentFrame = Vector2.zero;
    [System.Obsolete("No longer supported")]
    private Vector2 m_mouseMovementDirection;
    [System.Obsolete("No longer supported")]
    private float m_mouseMovementAngle;
    [System.Obsolete("No longer supported")]
    private float m_mouseMovementVelocity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GameOver = true;
        /*if (SceneManager.GetActiveScene().buildIndex == GAME) // Debugging/In Editor Purposes
        {
            // LoadSceneMode doesn't matter, just so game will still work if I start from game scene, instead of mainmenu.
            GameStart(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }*/
        SceneManager.sceneLoaded += SceneLoad;
    }

    //this isn't being used anywhere...
    private float PosOrNegAngle(Vector2 v1, Vector2 v2)
    {
        var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
        float angle = 0;
        angle = Vector2.Angle(v1, v2) * sign;

        return angle;
    }

    // This isn't needed...
    void Update()
    {
        m_mousePositionCurrentFrame = Input.mousePosition;
        m_mouseMovementAngle = PosOrNegAngle(m_mousePositionCurrentFrame, m_mousePositionLastFrame);
        var angle = Mathf.Deg2Rad * m_mouseMovementAngle;
        m_mouseMovementDirection = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        m_mouseMovementVelocity = Vector2.Distance(m_mousePositionLastFrame, m_mousePositionCurrentFrame);
        m_mousePositionLastFrame = m_mousePositionCurrentFrame;


    }

    // Added to scene load event, gets called any time a scene is loaded. 
    // Only checking to see if in the GAME scene.
    public void SceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == GAME)
        {
            Debug.Log("Let the games begin!");
            if (m_player != null)
            {
                Destroy(m_player);
            }
            SpawnPlayer();
            if (m_enemy != null)
            {
                Destroy(m_enemy);
            }
            SpawnEnemy();
            GameOver = false;
            Level = 1;
            Score = 0;
            RoundEnd.AddListener(EndFight);
            Invoke("StartFight", m_timeTillStart);
        }
        else
        {
            if (m_enemy != null)
            {
                Destroy(m_enemy);
            }
            if (m_player != null)
            {
                Destroy(m_player);
            }
        }
    }

    void SpawnPlayer()
    {
        m_player = Instantiate(PlayerPrefab);
    }

    void SpawnEnemy()
    {
        m_enemy = Instantiate(EnemyPrefab);
    }

    //Invokes all methods listening for the RoundStart event.
    void StartFight()
    {
        RoundStart.Invoke();
    }

    public void NextFight()
    {
        RoundNext.Invoke();
        SpawnEnemy();
        EnemyScript.CanAttack(true);
    }

    // Listens for RoundEnd event. Loads GAMEOVER scene.
    void EndFight()
    {
        //calls method after elapsed time.
        Invoke("LoadGameOver", 2);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene(GAMEOVER);
    }

    #region No Longer Used
    [System.Obsolete("No longer supported")]
    void DetermineCursorVelocity()
    {

    }
    GUIStyle guiStyle = new GUIStyle();
    private float debugging_horizontalMovementBoxPos;
    private float debugging_verticalMovementBoxPos;

    private void OnGUI()
    {
        if (Debug.isDebugBuild && false)
        {
            guiStyle.fontSize = 20;
            guiStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(debugging_horizontalMovementBoxPos, debugging_verticalMovementBoxPos, 250, 110), "Mouse Movement Values");
            GUI.TextArea(new Rect(debugging_horizontalMovementBoxPos + 10, debugging_verticalMovementBoxPos + 20, 200, 90), string.Format("Mouse Vector: {0}", m_mouseMovementDirection), guiStyle);
            GUI.TextArea(new Rect(debugging_horizontalMovementBoxPos + 10, debugging_verticalMovementBoxPos + 40, 200, 90), string.Format("Mouse Angle: {0:0.00}", m_mouseMovementAngle), guiStyle);
            GUI.TextArea(new Rect(debugging_horizontalMovementBoxPos + 10, debugging_verticalMovementBoxPos + 60, 200, 90), string.Format("Mouse Velocity: {0:0.00}", m_mouseMovementVelocity), guiStyle);
            //GUI.TextArea(new Rect(debugging_horizontalMovementBoxPos + 10, debugging_verticalMovementBoxPos + 80, 200, 90), string.Format("current speed: {0:0.00}", ), guiStyle);

            //GUI.TextArea(new Rect(debugging_horizontalRotationalBoxPos + 10, debugging_verticalRotationalBoxPos + 60, 200, 90), string.Format("current speed: {0:0.00}", m_currentSpeed), guiStyle);
        }
    }
    #endregion
}
