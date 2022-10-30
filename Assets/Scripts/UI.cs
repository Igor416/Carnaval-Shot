using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject pauseButtonPrefab;
    public GameObject arrowCountPrefab;
    public GameObject rechargeStatusPrefab;
    public GameObject currentScorePrefab;
    public GameObject bestScorePrefab;
    public GameObject menuPrefab;
    public GameObject revivePanelPrefab;
    public GameObject revivePanelSecondsPrefab;
    public GameObject endGameStatusPrefab;
    public GameObject endGameScorePrefab;
    public GameObject endGameAmmoPrefab;

    public static bool menuOpened = false;

    //Updating Score, Ammo etc
    public static event DisplayEvent StatsChanged;
    public static void OnStatsChanged()
    {
        StatsChanged();
    }
    public static event OpenCloseEvent GamePaused;
    public static void OnGamePaused(bool status)
    {
        GamePaused(status);
    }
    public static event OpenCloseEvent RevivePanelShowed;
    public static void OnRevivePanelShowed(bool status)
    {
        RevivePanelShowed(status);
    }
    public static event DisplayEvent RevivePanelSecondsChanged;
    public static void OnRevivePanelSecondsChanged()
    {
        RevivePanelSecondsChanged();
    }

    void Start()
    {
        StatsChanged += UpdateAmmo;
        StatsChanged += UpdateRechargeStatus;
        StatsChanged += UpdateScore;

        GamePaused += OpenCloseMenu;
        RevivePanelShowed += OpenCloseRevivePanel;
        RevivePanelSecondsChanged += UpdateRevivePanelSeconds;
    }

    public void UpdateAmmo()
    {
        arrowCountPrefab.GetComponent<Text>().text = Crossbow.ammo + " / " + Crossbow.allAmmo;
    }

    public void UpdateRechargeStatus()
    {
        rechargeStatusPrefab.GetComponent<Text>().text = Crossbow.rechargeStatus;
    }

    public void UpdateScore()
    {
        if (BallonSpawn.score > Game.record)
        {
            Game.record = BallonSpawn.score;
            bestScorePrefab.GetComponent<Text>().text = "Best: " + BallonSpawn.score;
        }
        if (BallonSpawn.score < 0)
        {
            Game.OnLose();
        }
        currentScorePrefab.GetComponent<Text>().text = "Score: " + BallonSpawn.score;
    }

    public void OpenCloseMenu(bool status)
    {
        menuPrefab.SetActive(status);
        endGameStatusPrefab.GetComponent<Text>().text = Game.endGameStatus;
        if (Game.gameOver)
        {
            endGameScorePrefab.GetComponent<Text>().text = "Score: " + BallonSpawn.score;
            endGameAmmoPrefab.GetComponent<Text>().text = "Ammo: " + Crossbow.ammo;
        }
    }

    public void OpenCloseRevivePanel(bool status)
    {
        revivePanelPrefab.SetActive(status);
    }

    public void UpdateRevivePanelSeconds()
    {
        revivePanelSecondsPrefab.GetComponent<Text>().text = Game.reviveStatus;
    }
}
