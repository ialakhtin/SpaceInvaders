using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text winText;
    public TMP_Text gameOverText;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject winMenu;
    public Player player;
    public InvaderFabric fabric;
    public MysteryShip ship;
    private int _score = 0;
    private int _lives = 3;
    private int _shootCount = -6;
    private int _maxShootCount = 17;
    private bool _paused = false;
    private bool _stopped = false;

    private void Awake()
    {
        this.fabric.invaderDestroyed += this.InvaderDestroyed;
        this.fabric.win += this.Success;
        this.fabric.gameOver += this.GameOver;
        this.player.dead += this.Lose;
        this.player.shooting += this.PlayerShooting;
        this.ship.destroyed += this.InvaderDestroyed;
        this.scoreText.text = "Score: " + this._score.ToString();
        this.livesText.text = "Lives: " + this._lives.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            this.ChangePauseCondition();
        }
    }

    public void ChangePauseCondition() {
        if (this._paused) {
            this.Resume();
        } else {
            this.Pause();
        }
    }

    public void Pause() {
        if (this._stopped) {
            return;
        }
        Time.timeScale = 0;
        this._paused = true;
        this.pauseMenu.SetActive(true);
    }

    public void Resume() {
        if (this._stopped) {
            return;
        }
        this.pauseMenu.SetActive(false);
        this._paused = false;
        Time.timeScale = 1;
    }

    private void InvaderDestroyed(int score)
    {
        this._score += score;
        this.scoreText.text = "Score: " + this._score.ToString();
    }

    private void Success()
    {
        this._stopped = true;
        Time.timeScale = 0;
        this.winText.text = "Your score is "+ this._score.ToString();
        this.winMenu.SetActive(true);
    }

    private void Lose(int lives)
    {
        //new WaitForSeconds(2);
        this._lives = System.Math.Max(0, this._lives - lives);
        this.livesText.text = "Lives: " + this._lives.ToString();
        if (this._lives <= 0) {
            this.GameOver();
        } else {
            this.player.Rebuild();
        }
    }

    private void GameOver()
    {
        this._stopped = true;
        Time.timeScale = 0;
        this.gameOverText.text = "Your score is "+ this._score.ToString();
        this.gameOverMenu.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quite()
    {
        Application.Quit();
    }

    private void PlayerShooting()
    {
        ++this._shootCount;
        if (this._shootCount == this._maxShootCount) {
            this.ship.Fly();
            this._shootCount = 0;
        }
    }
}
