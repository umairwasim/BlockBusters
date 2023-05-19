using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Transform feedbackPopUp;

    [Header("Gameplay UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private int score = 0;
    [SerializeField] private int life = 3;

    [Header("Panels")]
    [SerializeField] private GameObject levelWonPanel;
    [SerializeField] private GameObject levelLostPanel;
    [SerializeField] private GameObject gameCompletedPanel;

    private int parScore;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    #region Helper Functions

    public int GetScore() => score;
    public int GetLife() => life;

    #endregion

    public void SetParScore(int scoreToPar)
    {
        parScore = scoreToPar;
    }

    #region Update UI

    private void UpdateUI()
    {
        scoreText.text = "Score " + GetScore();
        lifeText.text = "Life " + GetLife();
    }

    private void ResetUI()
    {
        scoreText.text = "Score " + 0;
        lifeText.text = "Life " + 0;
    }

    public void UpdateScore(int scoreToAdd)
    {
        //If game is not in play mode, simply return
        if (GameManager.Instance.GameState != GameState.Starting)
            return;

        score += scoreToAdd;
        UpdateUI();

        //if score is less than zerom Level Lost
        if (score < 0)
        {
            ResetUI();
            GameManager.Instance.ChangeState(GameState.Lost);
            return;
        }
        //else if score is above par level, set next level
        else if (score >= parScore)
        {
            GameManager.Instance.SetLevel();
            return;
        }
    }

    public void UpdateLife(int lifeLost)
    {
        life -= lifeLost;
        UpdateUI();

        if (life <= 0)
        {
            ResetUI();
            GameManager.Instance.ChangeState(GameState.Lost);
            return;
        }
    }
    #endregion

    #region Panels

    public void ShowLevelLostPanel()
    {
        levelLostPanel.SetActive(true);
        levelLostPanel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InQuad);
    }

    public void ShowLevelWonPanel()
    {
        levelWonPanel.SetActive(true);
        levelWonPanel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InBounce);
    }

    public void ShowGameCompletedPanel()
    {
        gameCompletedPanel.SetActive(true);
        gameCompletedPanel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InElastic);
    }

    #endregion
}
