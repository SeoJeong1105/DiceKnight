using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    //public int GoalScore { get; protected set; } = 1500;
    //public int Level { get; protected set; } = 1;
    //public int Bet { get; protected set; } = 10;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LevelUp()
    {
        DataManager.instance.playerData.level++;
        if ((DataManager.instance.playerData.level % 5) == 0)
        {
            DataManager.instance.playerData.goalScore += 1500;
            DataManager.instance.playerData.bet += 50;
        }
    }
}
