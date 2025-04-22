using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Components")]
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameUIManager uiManager;
    [SerializeField] private ExitGateController exitGateController;

    [Header("Player Reference")]
    [SerializeField] private GameObject player;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GrabTrigger.OnPlayerGrabbed += TriggerFailureAfterGrab;
    }

    private void OnDisable()
    {
        GrabTrigger.OnPlayerGrabbed -= TriggerFailureAfterGrab;
    }

    private void Start()
    {
        gameTimer.OnTimeout.AddListener(TriggerFailure);
    }

    public void TriggerVictory()
    {
        if (gameEnded) return;
        gameEnded = true;

        float finishTime = gameTimer.RemainingTime;
        StartCoroutine(ShowVictoryDelayed(finishTime));
    }

    public void TriggerFailure()
    {
        if (gameEnded) return;
        gameEnded = true;

        StartCoroutine(ShowFailureDelayed(1f)); 
    }

    public void TriggerFailureAfterGrab()
    {
        if (gameEnded) return;
        gameEnded = true;

        StartCoroutine(ShowFailureDelayed(3f)); 
    }

    private IEnumerator ShowVictoryDelayed(float time)
    {
        yield return new WaitForSeconds(1f);
        uiManager.ShowVictory(time);
        Time.timeScale = 0f;
    }

    private IEnumerator ShowFailureDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.ShowFailure();
        Time.timeScale = 0f;
    }

    public void OnObjectiveCompleted()
    {
        exitGateController.UnlockGate();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
