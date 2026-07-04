using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }
    
    [SerializeField] private int _playerLives = 3;
    [SerializeField] private int _playerScore = 0;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetLivesText(_playerLives);
        SetScoreText(_playerScore);
    }

    private void SetLivesText(int lives)
    {
        _livesText.text = $"Lives: {lives}";
    }

    private void SetScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void HandlePlayerDeath()
    {
        _playerLives--;
        SetLivesText(_playerLives);
        if (_playerLives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddScore(int score)
    {
        _playerScore += score;
        SetScoreText(_playerScore);
    }

    private void ResetGameSession()
    {
        PersistentSceneObjects.Instance.ResetPersistentSceneObjects();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
