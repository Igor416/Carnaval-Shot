using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public delegate void DisplayEvent();
public delegate void OpenCloseEvent(bool status);

public class Game : MonoBehaviour
{
    private class StoredData
    {
        public int scoreRecord = 0;
    }

    public GameObject bestScorePrefab;
    public GameObject pauseButtonPrefab;

    public Sprite[] buttonSprites = new Sprite[2];

    public static Vector3 screenBounds;
    public static bool pause = false;
    public static bool gameOver = false;
    public static string endGameStatus;
    public static string reviveStatus;
    public static int record;

    private Coroutine _waitForRevive;
    private readonly string[] _statuses = new string[4] { "", ".", "..", "..."};
    private const int _reviveSeconds = 5;
    private const string _path = "Assets/Stats.json";
    private bool _isRevived = false;

    public static event DisplayEvent Lose;

    public static void OnLose()
    {
        pause = true;
        gameOver = true;
        StoredData stats = JsonUtility.FromJson<StoredData>(File.ReadAllText(_path));
        stats.scoreRecord = record;
        File.WriteAllText(_path, JsonUtility.ToJson(stats, true));
        Lose();
    }

    private IEnumerator WaitForRevive()
    {
        for (int k = _reviveSeconds; k > 0; k--)
        {
            for (int i = 0; i < _statuses.Length; i++)
            {
                reviveStatus = k + _statuses[i];
                UI.OnRevivePanelSecondsChanged();
                yield return new WaitForSeconds(1f / _statuses.Length);
            }
        }
        UI.OnRevivePanelShowed(false);
        endGameStatus = "Oh no!\nYou have losed!";
        StopCoroutine(_waitForRevive);
        UI.OnGamePaused(true);
    }

    void Awake()
    {
        screenBounds = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(screenBounds);
    }

    void Start()
    {
        Lose += StartReviving;
        StoredData stats = JsonUtility.FromJson<StoredData>(File.ReadAllText(_path));
        record = stats.scoreRecord;
        bestScorePrefab.GetComponent<Text>().text = "Best: " + stats.scoreRecord.ToString();
        UI.OnRevivePanelShowed(false);
        UI.OnGamePaused(false);
    }

    private void Update()
    {
        if (_isRevived)
        {
            StopCoroutine(_waitForRevive);
            _isRevived = false;
            pause = false;
            gameOver = false;
            Crossbow.isCharged = true;
            UI.OnRevivePanelShowed(false);
            Crossbow.ammo = Crossbow.allAmmo;
            if (BallonSpawn.score < 0)
            {
                BallonSpawn.score = 0;
            }
            Crossbow.rechargeStatus = "Charged";
            UI.OnStatsChanged();
        }
    }

    public void Restart()
    {
        BallonSpawn.PopUpAll();
        pauseButtonPrefab.GetComponent<Image>().sprite = buttonSprites[0];
        Crossbow.ammo = Crossbow.allAmmo;
        BallonSpawn.score = 0;
        gameOver = false;
        pause = false;
        Crossbow.isCharged = true;
        UI.OnGamePaused(false);
        UI.OnStatsChanged();
    }

    public void Quit()
    {
        gameObject.SetActive(false);
    }

    public void Revive()
    {
        _isRevived = true;
    }

    public void Pause()
    {
        if (!gameOver)
        {
            if (UI.menuOpened)
            {
                UI.menuOpened = false;
                pause = false;
                UI.OnGamePaused(false);
            }
            else
            {
                UI.menuOpened = true;
                pause = true;
                endGameStatus = "Good game!\n You may win!";
                UI.OnGamePaused(true);
            }
            ChangePauseButtonSprite();
        }
    }

    private void StartReviving()
    {
        UI.OnRevivePanelShowed(true);
        _waitForRevive = StartCoroutine(WaitForRevive());
    }

    private void ChangePauseButtonSprite()
    {
        Sprite currentSprite = pauseButtonPrefab.GetComponent<Image>().sprite;
        int i = 0;
        for (; i < buttonSprites.Length; i++)
        {
            if (currentSprite == buttonSprites[i])
            {
                break;
            }
        }
        pauseButtonPrefab.GetComponent<Image>().sprite = buttonSprites[(i - 1) * -1]; //algoritm for swithcing 0 and 1
    }
}
