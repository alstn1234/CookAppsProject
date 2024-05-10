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

    private int _width = 10, _height = 5;

    private int _stage;
    public int Stage
    {
        get
        {
            return _stage;
        }
        set
        {
            _stage = value > 3 ? 1 : value;
            OnUpdateStage?.Invoke();
        }
    }

    public Dictionary<string, GameObject> UnitPrefab;
    public Tile[,] Board;
    public List<Unit> MyUnits = new List<Unit>();
    public List<Unit> EnemyUnits = new List<Unit>();

    public GameMode Mode = GameMode.Ready;

    public TextMeshProUGUI resultText;
    public GameObject BattleEndPopup;
    public Action OnBattle;
    public Action OnEndBattle;
    public Action OnUpdateStage;

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
    }

    private void Init()
    {
        Board = new Tile[_width, _height];
        UnitPrefab = new Dictionary<string, GameObject>();
        foreach (var unit in Resources.LoadAll<GameObject>("Prefabs/Unit"))
        {
            UnitPrefab[unit.name] = unit;
        }
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
