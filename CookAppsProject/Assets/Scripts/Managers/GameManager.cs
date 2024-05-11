using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public int Width = 10, Height = 5;

    private const int _maxStage = 5;
    private int _stage;
    public int Stage
    {
        get
        {
            return _stage;
        }
        set
        {
            _stage = value > _maxStage ? 1 : value;
        }
    }

    public Tile[,] Board;
    [HideInInspector]
    public List<Unit> MyUnits = new List<Unit>();
    [HideInInspector]
    public List<Unit> EnemyUnits = new List<Unit>();

    [HideInInspector]
    public GameMode Mode = GameMode.Ready;

    [HideInInspector]
    public TextMeshProUGUI resultText;
    [HideInInspector]
    public GameObject BattleEndPopup;
    public Action OnBattle;
    public Action OnEndBattle;
    public Action OnRestart;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnBattle += BattleMode;
        OnBattle += CheckEnd;
        OnEndBattle += EndBattle;
        OnRestart += ReadyMode;
    }

    private void Init()
    {
        Board = new Tile[Width, Height];
        Stage = 1;
    }

    public void BattleMode()
    {
        Mode = GameMode.Battle;
    }
    public void ReadyMode()
    {
        Mode = GameMode.Ready;
    }
    private void CheckEnd()
    {
        StartCoroutine(CheckEndGame());
    }
    IEnumerator CheckEndGame()
    {
        var wfs = new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (MyUnits.Count == 0 || EnemyUnits.Count == 0)
            {
                OnEndBattle?.Invoke();
                break;
            }
            yield return wfs;
        }
    }
    public void EndBattle()
    {
        string resultStr = "";
        if (MyUnits.Count == 0 && EnemyUnits.Count == 0)
        {
            resultStr = "무승부";
        }
        else if (MyUnits.Count == 0)
        {
            resultStr = "패배";
        }
        else
        {
            resultStr = "승리";
            Stage++;
        }
        Debug.Log(resultStr);

        resultText.text = resultStr;
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        float DelayTime = 0.4f;
        yield return new WaitForSeconds(DelayTime);
        Time.timeScale = 0f;
        BattleEndPopup.SetActive(true);
    }
}
