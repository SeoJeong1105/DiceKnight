using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.IO;

public class Player : Unit
{
    public override void AddScore()
    {
        base.AddScore();
        UIManager.instance.SetScore(1, Score);
        UIManager.instance.SetScore(2, tmpScore);
        UIManager.instance.SetScore(3, curScore);
    }

    public override void AddTmpScore()
    {
        base.AddTmpScore();
        UIManager.instance.SetScore(2, tmpScore);
        UIManager.instance.SetScore(3, curScore);
    }

    public override void AddCurScore(int score)
    {
        base.AddCurScore(score);
        UIManager.instance.SetScore(3, curScore);
    }

    public override void FailScore()
    {
        base.FailScore();
        UIManager.instance.SetScore(2, tmpScore);
        UIManager.instance.SetScore(3, curScore);
    }

    public override void ShowButton()
    {
        UIManager.instance.SetVisible(true);
    }

    public override void ClearScore()
    {
        base.ClearScore();
        UIManager.instance.SetScore(1, Score);
        UIManager.instance.SetScore(2, tmpScore);
        UIManager.instance.SetScore(3, curScore);
    }
}