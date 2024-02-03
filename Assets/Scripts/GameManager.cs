using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Home[] homes;
    [SerializeField] private Frogger frogger;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Text timeText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text scoreText;

    public AudioSource bgm;
    public AudioSource sfx;
    [SerializeField] private AudioClip home, hop, plunk, squash, timeUp;

    private int lives;
    private int score;
    private int time;

    public int Lives => lives;
    public int Score => score;
    public int Time => time;

    private bool endGame;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            Application.targetFrameRate = 60;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        endGame = false;
        NewGame();
    }

    private void NewGame()
    {
        if (endGame)
        {
            SceneManager.LoadScene("Main Menu");
        }

        else
        {
            gameOverMenu.SetActive(false);

            SetScore(0);
            SetLives(3);
            NewLevel();
        }
    }

    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++) {
            homes[i].enabled = false;
        }

        Respawn();
    }

    private void Respawn()
    {
        frogger.Respawn();
        bgm.Play();

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private IEnumerator Timer(int duration)
    {
        time = duration;
        timeText.text = time.ToString();

        while (time > 0)
        {
            yield return new WaitForSeconds(1);

            time--;
            timeText.text = time.ToString();
        }

        frogger.Death();

        sfx.clip = timeUp;
        sfx.Play();
    }

    public void Died()
    {
        SetLives(lives - 1);
        bgm.Stop();

        if (lives > 0) {
            Invoke(nameof(Respawn), 1f);
        } else {
            Invoke(nameof(GameOver), 1f);
        }
    }

    private void GameOver()
    {
        frogger.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(CheckForPlayAgain());
    }

    private IEnumerator CheckForPlayAgain()
    {
        bool playAgain = false;

        while (!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                playAgain = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                playAgain = true;
                endGame = true;
            }

            yield return null;
        }

        NewGame();
    }

    public void AdvancedRow()
    {
        SetScore(score + 10);
    }

    public void HomeOccupied()
    {
        frogger.gameObject.SetActive(false);
        ReachHome();

        int bonusPoints = time * 20;
        SetScore(score + bonusPoints + 50);

        if (Cleared())
        {
            SetLives(lives + 1);
            SetScore(score + 1000);
            Invoke(nameof(NewLevel), 1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            if (!homes[i].enabled) {
                return false;
            }
        }

        return true;
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

    public void ReachHome()
    {
        sfx.clip = home;
        sfx.Play();
        bgm.Stop();
    }


    public void Hop()
    {
        sfx.clip = hop;
        sfx.Play();
    }

    public void DeathPlunk()
    {
        sfx.clip = plunk;
        sfx.Play();
    }

    public void DeathSquash()
    {
        sfx.clip = squash;
        sfx.Play();
    }

}
