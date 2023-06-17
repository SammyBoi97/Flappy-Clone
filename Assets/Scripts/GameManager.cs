using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public AudioSource audioSource;

    public GameObject gameOverPanel;
    public GameObject newGamePanel;
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject pauseButton;
    public GameObject player;

    public enum GameState { Playing, Paused, Over, Ready, Menu };
    public GameState curGameState;

    public float deathCooldownPeriod = 0.75f;
    private float deathTimeStamp = -1f;

    public Sprite pauseSprite;
    public Sprite playSprite;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) // Input.GetMouseButtonDown(0))) //Input.touchCount > 0))
        {
            // Check if finger is over a UI element
            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            // If game is paused, ensure it is now unpaused
            if (curGameState == GameState.Paused)
            {
                StartCoroutine(StartPlayingCoroutine());
                return;
            }
            
            else if (curGameState == GameState.Ready)
            {
                player.GetComponent<PlayerController>().Jump();
                StartCoroutine(StartPlayingCoroutine());
            }

            // If the game is over and we are spacing out of a paused state it means we are starting a new game
            else if (curGameState == GameState.Over)
            {
                SetupNewGame();
            }
        }
    }

    public void TogglePause()
    {
        if (curGameState == GameState.Paused)
        {
            StartCoroutine(StartPlayingCoroutine());
        }
        else if (curGameState == GameState.Playing)
        {
            PauseGame();
        }
    }


    public void PauseGame()
    {
        curGameState = GameState.Paused;

        Time.timeScale = 0;
        player.GetComponent<PlayerController>().canMove = false;

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        newGamePanel.SetActive(false);
        pauseButton.SetActive(true);
        pauseMenuPanel.SetActive(true);    

    }

    public void SetupNewGame()
    {
        Time.timeScale = 1;
        if (deathTimeStamp >= 0 && (deathTimeStamp + deathCooldownPeriod) > Time.time)
        {
            return;
        }

        curGameState = GameState.Ready;

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        newGamePanel.SetActive(true);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);

        ResetDisablePlayer();
        player.GetComponent<PlayerController>().enabled = true;

    }
    
    public void GameOver()
    {
        curGameState = GameState.Over;

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        newGamePanel.SetActive(false);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);

        Time.timeScale = 1;
        EnvironmentController.Instance.movementMultiplier = 0f;
        player.GetComponent<PlayerController>().canMove = false;
        player.GetComponent<Animator>().SetBool("isDead", true);
        ScoreManager.Instance.UpdateScorecard();
        deathTimeStamp = Time.time;
        GetComponent<AudioSource>().Play();
    }

    public void MainMenu()
    {
        curGameState = GameState.Menu;

        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        newGamePanel.SetActive(false);
        pauseButton.SetActive(false);
        pauseMenuPanel.SetActive(false);

        Time.timeScale = 1;
        ResetDisablePlayer();
    }

    public void ResetDisablePlayer()
    {
        player.GetComponent<PlayerController>().enabled = false;

        player.GetComponent<Animator>().SetBool("isIdle", true);
        player.GetComponent<Animator>().SetBool("isDead", false);
        player.GetComponent<Animator>().enabled = false;

        player.transform.position = new Vector2(player.transform.position.x, -0.5f);
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().simulated = false;

        ScoreManager.Instance.ResetScore();
        EnvironmentController.Instance.ResetEnvironment();
        EnvironmentController.Instance.movementMultiplier = 0f;
    }

    public void StartPlayingWrapper()
    {
        StartCoroutine(StartPlayingCoroutine());
    }

    public IEnumerator StartPlayingCoroutine()
    {
        Time.timeScale = 1;

        //yield on a new YieldInstruction that waits for the end of the frame
        yield return new WaitForEndOfFrame();

        curGameState = GameState.Playing;

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        newGamePanel.SetActive(false);
        pauseButton.SetActive(true);
        pauseMenuPanel.SetActive(false);

        EnvironmentController.Instance.movementMultiplier = 1f;
        player.GetComponent<PlayerController>().canMove = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Animator>().SetBool("isIdle", false);
        player.GetComponent<Animator>().SetBool("isDead", false);
    }

}
