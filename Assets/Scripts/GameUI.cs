using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    public Button rollButton;
    public Button stopButton;
    public GameObject endPanel;
    public GameObject rulePanel;
    public Text resultText;
    public Text failText;

    public List<Text> textList = new List<Text>();

    private void Start()
    {
        endPanel.SetActive(false);
    }

    public void SetScore(int index, int score)
    {
        textList[index].text = score.ToString();
    }

    public void FailScore()
    {
        DOTweenAnimation[] anims = failText.GetComponents<DOTweenAnimation>();
        foreach(DOTweenAnimation anim in anims)
        {
            anim.DOPlay();
        }
    }

    public void SetVisible(bool b)
    {
        rollButton.gameObject.SetActive(b);
        stopButton.gameObject.SetActive(b);
    }

    public void OnClickRuleButton()
    {
        rulePanel.SetActive(true);
    }

    public void OnExitButton()
    {
        rulePanel.SetActive(false);
    }
}
