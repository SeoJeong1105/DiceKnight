using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mainUI;
    public GameObject gameUI;

    public GameObject popupPanel;
    public Text popupText;
    public Text levelText;
    public Text nameText;
    public Text moneyText;
    public Text betText;


    GameUI gameUIClass;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
        DontDestroyOnLoad(this.gameObject);
        gameUIClass = gameUI.GetComponent<GameUI>();        
    }

    private void Start()
    {
        popupPanel.SetActive(false);
    }

    public void OnClickMultiButton()
    {
        ShowPopup("¾÷µ¥ÀÌÆ® ¿¹Á¤ÀÔ´Ï´Ù");
    }

    public void OnClickSingleButton()
    {
        SetMainUI(false);
        StartCoroutine(GameManager.instance.StartBattle());
    }
    
    public void OnClickStartUI()
    {
        SetMainUI(true);
    }

    public void OnBackButton()
    {
        SetMainUI(true);
        gameUIClass.endPanel.SetActive(false);
    }

    public void EndBattle(bool b)
    {
        if (b)
        {
            gameUIClass.resultText.text = "½Â¸®";
            levelText.text = "Level " + DataManager.instance.playerData.level;
            betText.text = DataManager.instance.playerData.bet.ToString() + " È¹µæ";
        }
        else
        {
            gameUIClass.resultText.text = "ÆÐ¹è";
        }
        gameUIClass.endPanel.SetActive(true);
        moneyText.text = DataManager.instance.playerData.money.ToString();
        betText.text = DataManager.instance.playerData.bet.ToString() + " ÀÒÀ½";
    }

    public void OnRollButton()
    {
        GameManager.instance.RollDices();
        SetInteractable(0);
    }

    public void OnStopButton()
    {
        GameManager.instance.StopTurn();
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    public void SetScore(int index, int score)
    {
        gameUIClass.SetScore(index, score);
    }

    public void SetInteractable(int i)
    {
        switch(i)
        {
            case 0:
                gameUIClass.rollButton.interactable = false;
                gameUIClass.stopButton.interactable = false;
                break;
            case 1:
                gameUIClass.rollButton.interactable = true;
                gameUIClass.stopButton.interactable = true;
                break;
            default:
                gameUIClass.rollButton.interactable = true;
                gameUIClass.stopButton.interactable = false;
                break;
        }
    }

    public void SetVisible(bool b)
    {
        gameUIClass.SetVisible(b);
    }

    public void FailScore()
    {
        gameUIClass.FailScore();
    }

    void SetMainUI(bool b)
    {
        mainUI.SetActive(b);
        gameUI.SetActive(!b);
    }

    public void LoadData()
    {
        nameText.text = DataManager.instance.playerData.name;
        levelText.text = "Level " + DataManager.instance.playerData.level.ToString();
        moneyText.text = DataManager.instance.playerData.money.ToString();
    }
}
