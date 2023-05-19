using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Starting,
    Won,
    Lost,
    GameCompleted,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState GameState { get; private set; }

    [SerializeField] private LevelData[] levelData;

    private const string LEVEL = "Level";

    private int levelNo;

    private void Awake()
    {
        Instance = this;
        levelNo = PlayerPrefs.GetInt(LEVEL, 0);
    }

    private void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.Starting:
                Initialize();
                break;
            case GameState.Won:
                LevelWon();
                break;
            case GameState.Lost:
                LevelLost();
                break;
            case GameState.GameCompleted:
                GameCompleted();
                break;
            default:
                break;
        }
    }

    #region GameState Functions

    private void Initialize()
    {
        ShapesSpawner.Instance.InitializeLevelData(levelData[levelNo]);
        UIManager.Instance.SetParScore(levelData[levelNo].parScore);
    }

    private void LevelWon()
    {
        ShapesSpawner.Instance.StopItemsSpawning();
        UIManager.Instance.ShowLevelWonPanel();
    }

    private void LevelLost()
    {
        ShapesSpawner.Instance.StopItemsSpawning();
        UIManager.Instance.ShowLevelLostPanel();
    }

    private void GameCompleted()
    {
        ShapesSpawner.Instance.StopItemsSpawning();
        UIManager.Instance.ShowGameCompletedPanel();
    }

    #endregion

    #region Set/Get Level

    //Set the level and call game state based on level value
    public void SetLevel()
    {
        levelNo++;
        PlayerPrefs.SetInt(LEVEL, levelNo);

        if (levelNo >= levelData.Length)
            ChangeState(GameState.GameCompleted);
        else
            ChangeState(GameState.Won);
    }

    public int GetLevel()
    {
        return levelNo;
    }

    #endregion

    public void ResetLevels()
    {
        levelNo = 0;
        PlayerPrefs.SetInt(LEVEL, levelNo);
    }

    public void ReloadScene()
    {
        ChangeState(GameState.Starting);
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSfx);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
