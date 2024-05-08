using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public Action OnBattle;
    public Action OnEndBattle;

    private void Awake()
    {
        if(instance == null)
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
        OnEndBattle += Ready;
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
}
