using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    int money = 100;
    public int Score { get; protected set; }
    public int tmpScore { get; protected set; }
    public int curScore { get; protected set; }

    public Dictionary<int, int> remainingDices = new Dictionary<int, int>();
    List<Dice> dices;

    public virtual void AddScore() { Score += curScore + tmpScore; tmpScore = 0; curScore = 0; }
    public virtual void AddTmpScore() { tmpScore += curScore; curScore = 0; }
    public virtual void AddCurScore(int score) { curScore = score; }

    public virtual void ShowButton() { }

    public virtual void FailScore()
    {
        tmpScore = 0;
        curScore = 0;
    }

    public virtual void ClearScore()
    {
        Score = 0;
        tmpScore = 0;
        curScore = 0;
    }

    public void AddMoney(int count)
    {
        money += count;
    }
}
