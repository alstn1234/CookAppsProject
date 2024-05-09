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

    public Tile[,] Board;
    public List<Unit> MyUnits = new List<Unit>();
    public List<Unit> EnemyUnits = new List<Unit>();

    public GameMode Mode = GameMode.Ready;

    public TextMeshProUGUI resultText;
    public GameObject BattleEndPopup;
    public Action OnBattle;
    public Action OnEndBattle;

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
        OnBattle += Battle;
        OnEndBattle += EndBattle;
    }

    private void Init()
    {
        Board = new Tile[_width, _height];
    }

    public void Battle()
    {
        Mode = GameMode.Battle;
    }
    public void Ready()
    {
        Mode = GameMode.Ready;
    }
    public void CheckEndGame()
    {
        if (MyUnits.Count == 0 || EnemyUnits.Count == 0)
        {
            OnEndBattle?.Invoke();
        }
    }
    public void EndBattle()
    {
        string resultStr = "";
        StartCoroutine(End());
        if (MyUnits.Count == 0 && EnemyUnits.Count == 0)
            resultStr = "무승부";
        else if (MyUnits.Count == 0)
            resultStr = "패배";
        else
            resultStr = "승리";

        resultText.text = resultStr;
    }

    IEnumerator End()
    {
        float DelayTime = 0.9f;
        yield return new WaitForSeconds(DelayTime);
        Time.timeScale = 0f;
        BattleEndPopup.SetActive(true);
    }
}
