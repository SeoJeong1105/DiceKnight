using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State { Start, PlayerTurn, EnemyTurn, Win, Lose }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ScoreCalculator calculator;
    public Unit unit;
    public Player player;
    public Enemy enemy;

    public State state;
    
    List<int> scoringDices = new List<int>();
    List<int> dices = new List<int>();

    private int selectedCount = 0;
    private bool isReady = false;
    private Coroutine enemyTurn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        DiceManager.instance.OnGetResults += OnGetResults;
        DiceManager.instance.OnGetScore += OnGetScore;

        player = new Player();
        enemy = new Enemy();

        UIManager.instance.LoadData();
        SoundManager.instance.PlayBackgroundMusic();
    }

    public IEnumerator StartBattle()
    {
        ResetBattle();
        yield return new WaitForSeconds(1f);
        
        state = State.PlayerTurn;
        UIManager.instance.SetScore(0, DataManager.instance.playerData.goalScore);
        StartTurn();
    }

    void ResetBattle()
    {
        player.ClearScore();
        enemy.ClearScore();
        ClearDice();
    }

    void StartTurn()
    {
        if (state == State.PlayerTurn)
        {
            unit = player;
            unit.ShowButton();
            PlayerTurn();
        }
        else
        {
            unit = enemy;
            unit.ShowButton();
            DiceManager.instance.SetSelectable(false);

            enemyTurn = StartCoroutine(EnemyTurn());
        }
    }

    void PlayerTurn()
    {
        UIManager.instance.SetInteractable(2);
    }

    IEnumerator EnemyTurn()
    { 
        yield return new WaitForSeconds(1f);
        RollDices();

        yield return new WaitUntil(() => isReady);
        dices = DiceManager.instance.GetResult();

        scoringDices = calculator.GetScoringDices(dices);
        int newScore = calculator.CalculateScore(scoringDices);

        selectedCount += scoringDices.Count;
        unit.AddCurScore(newScore);

        yield return StartCoroutine(DiceManager.instance.SetSelected(scoringDices));
        
        if ((unit.Score + unit.tmpScore + unit.curScore) >= DataManager.instance.playerData.goalScore || enemy.ShouldStop())
        {
            EndTurn();
            yield break;
        }
        else
        {
            unit.AddTmpScore();
            yield return StartCoroutine(EnemyTurn());
        }
    }

    void OnGetResults()
    {
        if (!DiceManager.instance.IsScorable())
        {
            if(state == State.EnemyTurn)
                StopCoroutine(enemyTurn);
            EndTurn();
            return;
        }

        if (state == State.PlayerTurn)
        {
            DiceManager.instance.SetSelectable(true);
        }

        isReady = true;
    }

    public void OnGetScore(bool isSelected, int num)
    {
        selectedCount += isSelected ? 1 : -1;

        if (isSelected) dices.Add(num);
        else dices.Remove(num);

        int newScore = calculator.CalculateScore(dices);
        unit.AddCurScore(newScore);

        if (newScore == 0)
        {
            UIManager.instance.SetInteractable(0);
            return;
        }

        UIManager.instance.SetInteractable(1);
    }

    void EndTurn()
    {
        if (unit.curScore > 0)
        {
            unit.AddScore();
        }
        else
        {
            unit.FailScore();
            UIManager.instance.FailScore();
        }

        if (unit.Score >= DataManager.instance.playerData.goalScore)
        {
            if (state == State.PlayerTurn)
                state = State.Win;
            else
                state = State.Lose;
            EndBattle();
            return;
        }
        ClearDice();

        state = (state == State.PlayerTurn) ? State.EnemyTurn : State.PlayerTurn;
        StartTurn();
    }

    void EndBattle()
    {
        bool bWin = false;
        if (state == State.Win)
        {
            bWin = true;
            LevelManager.instance.LevelUp();
            DataManager.instance.playerData.money += DataManager.instance.playerData.bet;
        }
        else if (state == State.Lose)
        {
            bWin = false;
            DataManager.instance.playerData.money -= DataManager.instance.playerData.bet;
        }
        DataManager.instance.SaveData();
        UIManager.instance.EndBattle(bWin);
    }

    public void StopTurn()
    {
        EndTurn();
    }

    public void RollDices()
    {
        isReady = false;
        if (selectedCount > 0)
        {
            DiceManager.instance.TakeoutDice();
            dices.Clear();
            unit.AddTmpScore();

            if (selectedCount == 6)
                ClearDice();
        }

        DiceManager.instance.RollAllDice();
        SoundManager.instance.PlaySoundEffect(SoundManager.instance.diceClip);
    }

    void ClearDice()
    {
        dices.Clear();
        selectedCount = 0;
        DiceManager.instance.SetDiceMax();
    }

    public bool isPlayerTurn()
    {
        return state == State.PlayerTurn;
    }
}
