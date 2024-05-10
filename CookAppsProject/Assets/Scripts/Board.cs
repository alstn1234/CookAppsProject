using System;
using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int _width = 10;
    private int _height = 5;
    private Tile[,] _board;
    [SerializeField] private GameObject _unitsObj;
    private GameObject _tilePrefab;

    private void Awake()
    {
        _board = GameManager.instance.Board;
        _tilePrefab = Resources.Load<GameObject>("Prefabs/Board/Tile");
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // 보드판 세팅
        GameObject tileObject;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                tileObject = Instantiate(_tilePrefab, gameObject.transform);
                var tile = tileObject.GetComponent<Tile>();
                tile.Init(x, y);

                _board[x, y] = tile;
            }
        }
        // 초기 유닛 세팅
        StartCoroutine(SetUnit());
    }

    IEnumerator SetUnit()
    {
        var myUnits = GameManager.instance.MyUnits;
        var enemyUnits = GameManager.instance.EnemyUnits;
        myUnits.Clear();
        enemyUnits.Clear();

        yield return new WaitForEndOfFrame();


        // 아군
        var data = CSVReader.Read($"MyUnitData/Stage{GameManager.instance.Stage}");
        int idx = 0;
        foreach (var item in data)
        {
            var unitName = item["Name"];
            var count = int.Parse(item["Count"]);
            for(int i = 0; i < count; i++)
            {
                var unit = GetUnit(unitName);
                int x = idx % (_width / 2);
                int y = idx++ / (_width / 2);

                unit.SetParentTile(_board[x, y]);
                unit.SetPosToParent();
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

            var unit = GetUnit(unitName);

            unit.SetParentTile(_board[x, y]);
            unit.SetPosToParent();
            enemyUnits.Add(unit);
            
        }
    }

    private Unit GetUnit(string unitName)
    {
        var unitPrefab = GameManager.instance.UnitPrefab[unitName];
        var unitObj = Instantiate(unitPrefab, _unitsObj.transform);
        return unitObj.GetComponent<Unit>();
    }
}
