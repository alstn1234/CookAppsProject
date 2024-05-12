using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private int _width;
    private int _height;
    private Tile[,] _board;
    private int boardPadding;
    private GameObject _tilePrefab;

    private UnitObjectPool _pool;
    private List<Unit> myUnits;
    private List<Unit> enemyUnits;

    private void Awake()
    {
        _board = GameManager.instance.Board;
        _tilePrefab = Resources.Load<GameObject>("Prefabs/Board/Tile");
        myUnits = GameManager.instance.MyUnits;
        enemyUnits = GameManager.instance.EnemyUnits;
    }

    private void Start()
    {
        _pool = UnitObjectPool.instance;
        _width = GameManager.instance.Width;
        _height = GameManager.instance.Height;
        GameManager.instance.OnRestart += SetUnit;
        StartCoroutine(Init());
    }
    private void OnDisable()
    {
        GameManager.instance.OnRestart -= SetUnit;
    }

    IEnumerator Init()
    {
        // 1754 -> 보드판의 좌표상 가로 길이
        boardPadding = (Screen.width - 1754) / 2;
        GetComponent<GridLayoutGroup>().padding.left = boardPadding;
        // 보드판 세팅
        GameObject tileObject;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                tileObject = Instantiate(_tilePrefab, transform);
                var tile = tileObject.GetComponent<Tile>();
                tile.Init(x, y);

                _board[x, y] = tile;
            }
        }
        yield return new WaitForEndOfFrame();

        SetUnit();
    }

    // 유닛 세팅
    public void SetUnit()
    {
        UnitObjectPool.instance.ResetPool();
        myUnits.Clear();
        enemyUnits.Clear();

        // 아군
        var data = CSVReader.Read($"MyUnitData/Stage{GameManager.instance.Stage}");
        int idx = 0;
        foreach (var item in data)
        {
            var unitName = item["Name"];
            var count = int.Parse(item["Count"]);
            for (int i = 0; i < count; i++)
            {
                var unit = _pool.Pop(unitName);
                int x = idx % (_width / 2);
                int y = idx++ / (_width / 2);

                unit.SetParentTile(_board[x, y]);
                myUnits.Add(unit);
            }
        }

        // 적군
        data = CSVReader.Read($"UnitData/Stage{GameManager.instance.Stage}");
        foreach (var item in data)
        {
            var unitName = item["Name"];
            var x = int.Parse(item["x"]);
            var y = int.Parse(item["y"]);
            var unit = _pool.Pop(unitName);

            unit.SetParentTile(_board[x, y]);
            enemyUnits.Add(unit);
        }
    }

}
