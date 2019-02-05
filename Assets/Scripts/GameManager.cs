using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] ghosts = new GameObject[4];
    private GhostBehavior[] ghostBehaviors = new GhostBehavior[4];
    // 0 = Blinky
    // 1 = Pinky
    // 2 = Clyde
    // 3 = Inky

    private float timer;
    public int pelletCount = 240;
    public int lives = 3;
    public int score;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverScore;
    public GameObject victoryScreen;
    public GameObject HUD;
    public TextMeshProUGUI HUDLives;
    public TextMeshProUGUI HUDScore;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Vector3 playerStartPosition;

    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerStartPosition = player.transform.position;
        playerMovement = player.GetComponent<PlayerMovement>();

        for (int g = 0; g < ghosts.Length; g++)
        {
            ghostBehaviors[g] = ghosts[g].GetComponent<GhostBehavior>();
        }
        ghostBehaviors[0].waiting = ghostBehaviors[1].waiting = false;
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (pelletCount < 170 || timer > 10.0f)
        {
            ghostBehaviors[2].waiting = false;
            if (pelletCount < 100 || timer > 20.0f)
            {
                ghostBehaviors[3].waiting = false;
                if (pelletCount <= 0)
                {
                    Victory();
                }
            }
        }
    }

    // Increase score by amount and update HUD
    public void IncreaseScore(int amount)
    {
        score += amount;
        HUDScore.text = "Score:\n" + score.ToString();
    }

    // Cause ghosts to scatter
    public void Scatter()
    {
        foreach (GhostBehavior ghost in ghostBehaviors)
        {
            ghost.ScatterMode();
        }
    }

    // Change ghost appearance and cause them to scatter
    public void Frighten()
    {
        foreach (GhostBehavior ghost in ghostBehaviors)
        {
            if (!ghost.waiting)
            {
                ghost.frightened = true;
                ghost.myAgent.speed = 0.5f;
                ghost.ScatterMode();
                ghost.ghostParticles.startColor = Color.gray;
            }
        }
    }

    // Subtract a life, update HUD, reset characters
    public void LoseLife()
    {
        lives--;
        HUDLives.text = "Lives:\n" + lives.ToString();
        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            ResetCharacters();
        }
    }

    // Set characters back to starting positions
    public void ResetCharacters()
    {
        for (int g = 0; g < ghosts.Length; g++)
        {
            ghosts[g].SetActive(false);
            ghosts[g].transform.position = ghostBehaviors[g].startPosition;
            ghosts[g].SetActive(true);
        }
        player.transform.position = playerStartPosition;
    }

    // Show Game Over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        gameOverScore.text = "Score: " + score.ToString();
        if (playerMovement.topDown == false)
        {
            playerMovement.TogglePerspective();
        }
        player.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    // Show Victory screen
    public void Victory()
    {
        victoryScreen.SetActive(true);
        if (playerMovement.topDown == false)
        {
            playerMovement.TogglePerspective();
        }
        player.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    // Reload scene
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load Main Menu scene
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
