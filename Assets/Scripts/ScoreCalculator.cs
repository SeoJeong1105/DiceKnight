using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    Dictionary<int, int> scoringDices = new Dictionary<int, int>();

    public int CalculateScore(List<int> diceList)
    {
        int score = 0;
        List<int> dices = new List<int>();
        
        scoringDices.Clear();

        Dictionary<int, int> diceDic = new Dictionary<int, int>();
        foreach(int i in diceList)
        {
            if (!diceDic.ContainsKey(i))
                diceDic[i] = 0;
            diceDic[i]++;
        }

        if (diceDic.Keys.OrderBy(x => x).SequenceEqual(new List<int> { 1, 2, 3, 4, 5, 6 }))
        {
            score += 1500;
            dices.AddRange(diceList);
        }
        else if(diceDic.Keys.OrderBy(x => x).SequenceEqual(new List<int> { 2, 3, 4, 5, 6 }))
        {
            score += 750;
            dices.AddRange(diceList.Distinct().ToList());
            
        }
        else if (diceDic.Keys.OrderBy(x => x).SequenceEqual(new List<int> { 1, 2, 3, 4, 5 }))
        {
            score += 500;
            dices.AddRange(diceList.Distinct().ToList());
        }

        for (int i = 0; i < diceDic.Count; i++)
        {
            int num = diceDic.Keys.ToList()[i];
            int count = diceDic.Values.ToList()[i];
            int m = 1;

            if (dices.Contains(num)) count--;

            if (count >= 3)
            {
                for (int j = 0; j < count - 3; j++)
                {
                    m *= 2;
                }
                score += num * m * 100;

                if (num == 1)
                {
                    score *= 10;
                }
            }
            else
            {
                if (num != 1 && num != 5) continue;
                
                if (num == 1)
                    score += count * 100;
                else if (num == 5)
                    score += count * 50;
            }

            for (int j = 0; j < count; j++)
            {
                dices.Add(num);
            }
        }

        foreach(int i in dices)
        {
            if (!scoringDices.ContainsKey(i))
                scoringDices[i] = 0;
            scoringDices[i]++;
        }

        if (!GameManager.instance.isPlayerTurn())
            return score;

        dices.Sort();
        diceList.Sort();

        if (!dices.SequenceEqual(diceList))
            score = 0;

        return score;
    }

    public List<int> GetScoringDices(List<int> diceList)
    {
        int score = CalculateScore(diceList);
        List<int> dices = new List<int>();

        foreach(var pair in scoringDices)
        {
            for(int i = 0; i < pair.Value; i++)
            {
                dices.Add(pair.Key);
            }
        }

        if (score > 300) return dices;

        if (scoringDices.ContainsKey(1))
        {
            return new List<int> { 1 };
        }
        else if (scoringDices.ContainsKey(5))
        {
            return new List<int> { 5 };
        }

        return dices;
    }
}
