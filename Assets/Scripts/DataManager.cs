using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public string name;
    public int level = 1;
    public int money = 100;
    public int goalScore = 1500;
    public int bet = 10;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public PlayerData playerData = new PlayerData();

    public string path;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
            Destroy(gameObject);

        path = Application.persistentDataPath + "/save";
    }

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path, jsonData);
    }

    public void LoadData()
    {
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
    public void LevelUp()
    {
        playerData.level++;
        if ((playerData.level % 5) == 0)
        {
            playerData.goalScore += 1500;
            playerData.bet += 50;
        }
    }

    public void EarnMoney(bool b)
    {
        int i = b ? 1 : -1;
        playerData.money += i * playerData.bet;
    }
}
