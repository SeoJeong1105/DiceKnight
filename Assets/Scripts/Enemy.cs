using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : Unit
{
    public override void AddScore()
    {
        base.AddScore();
        UIManager.instance.SetScore(4, Score);
        UIManager.instance.SetScore(5, tmpScore);
        UIManager.instance.SetScore(6, curScore);
    }

    public override void AddTmpScore()
    {
        base.AddTmpScore();
        UIManager.instance.SetScore(5, tmpScore);
        UIManager.instance.SetScore(6, curScore);
    }

    public override void AddCurScore(int score)
    {
        base.AddCurScore(score);
        UIManager.instance.SetScore(6, curScore);
    }
    public override void FailScore()
    {
        base.FailScore();
        UIManager.instance.SetScore(5, tmpScore);
        UIManager.instance.SetScore(6, curScore);
    }

    public override void ShowButton()
    {
        UIManager.instance.SetVisible(false);
    }

    public override void ClearScore()
    {
        base.ClearScore();
        UIManager.instance.SetScore(4, Score);
        UIManager.instance.SetScore(5, tmpScore);
        UIManager.instance.SetScore(6, curScore);
    }

    public bool ShouldStop()
    {
        float probability = 1;
        int playerScore = GameManager.instance.player.Score;

        switch (playerScore - Score - tmpScore - curScore)
        {
            case < -500:
                probability = 40;
                break;
            case <= 500:
                probability = 60;
                break;
            default:
                probability = 80;
                break;
        }

        switch (DiceManager.instance.RemainingCount())
        {
            case 1:
                probability *= 0.2f;
                break;
            case 2:
                probability *= 0.4f;
                break;
            case 3:
                probability *= 0.6f;
                break;
            case 4:
                probability *= 0.8f;
                break;
            default:
                probability = 100;
                break;
        }

        if (probability < Random.Range(1, 100))
            return true;
        else
            return false;
    }
}
