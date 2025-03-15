using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;

    public Action OnGetResults;
    public Action<bool, int> OnGetScore;

    [SerializeField] List<Dice> _dice = new List<Dice>();
    
    int maxDice = 6;
    int diceRemaining;
    int diceStillRolling;
    
    // 주사위 결과 - 1이 나온 주사위가 2개면 diceNum[0] = 2
    int[] diceNum = new int[6];

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        foreach(Dice dice in _dice)
        {
            dice.OnDiceStopped += OnDiceStopped;
            dice.OnDiceSelected += OnDiceSelected;
        }

        diceRemaining = maxDice;
    }

    void OnDisable()
    {
        foreach(Dice dice in _dice)
        {
            dice.OnDiceStopped -= OnDiceStopped;
            dice.OnDiceSelected -= OnDiceSelected;
        }   
    }

    public void RollAllDice()
    {
        diceStillRolling = diceRemaining;

        for (int i = 0; i < diceNum.Length; i++)
        {
            diceNum[i] = 0;
        }

        foreach (Dice dice in _dice)
        {
            if (dice.IsSelected()) continue;
            dice.RollDice();
        }
    }

    public void TakeoutDice()
    {
        diceRemaining = maxDice;

        foreach (Dice dice in _dice)
        {
            if (!dice.IsSelected()) continue;
            
            diceRemaining--;
        }
    }

    public void SetDiceMax()
    {
        diceRemaining = maxDice;

        foreach (Dice dice in _dice)
        {
            dice.SetDice();
        }
    }

    public bool IsScorable()
    {
        foreach (int i in diceNum)
        {
            if (i >= 3) return true;
        }
        if (diceNum[0] == 0 && diceNum[4] == 0) return false;

        return true;
    }

    void OnDiceStopped(int result)
    {
        diceStillRolling--;
        diceNum[result - 1]++;

        if (diceStillRolling != 0) return;
        OnGetResults.Invoke();
    }

    void OnDiceSelected(bool isSelected, int result)
    {
        OnGetScore.Invoke(isSelected, result);
    }

    public List<int> GetResult()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < diceNum.Length; i++)
        {
            for (int j = 0; j < diceNum[i]; j++)
                list.Add(i + 1);
        }
        return list;
    }

    public void SetSelectable(bool b)
    {
        foreach(Dice dice in _dice)
        {
            dice.SetSelectable(b);
        }
        return;
    }

    public IEnumerator SetSelected(List<int> dices)
    {
        foreach (Dice dice in _dice)
        {
            if (dice.IsSelected()) continue;

            int num = dice.GetResult();
            if (!dices.Contains(num)) continue;

            dices.Remove(num);
            dice.SelectDice();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public int RemainingCount()
    {
        int count = 0;
        foreach (Dice dice in _dice)
        {
            if (!dice.IsSelected())
                count++;
        }
        return count;
    }
}

