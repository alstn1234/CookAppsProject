using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int _width = 10;
    private int _height = 5;
    private Tile[,] _board;
    [SerializeField] private GameObject _myUnitsObj;
    [SerializeField] private GameObject _enemyUnitsObj;
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
        GameObject unitObj;
        foreach (var unit in Resources.LoadAll<GameObject>("Prefabs/Unit/My"))
        {
            unitObj = Instantiate(unit, _myUnitsObj.transform);
            myUnits.Add(unitObj.GetComponent<Unit>());
        }

        foreach (var unit in Resources.LoadAll<GameObject>("Prefabs/Unit/Enemy"))
        {
            unitObj = Instantiate(unit, _enemyUnitsObj.transform);
            enemyUnits.Add(unitObj.GetComponent<Unit>());
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < myUnits.Count; i++)
        {
            int x = i % (_width / 2);
            int y = i / (_width / 2);

            myUnits[i].SetParentTile(_board[x, y]);
            myUnits[i].SetPosToParent();
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            int x = i % (_width / 2) + 5;
            int y = i / (_width / 2);

            enemyUnits[i].SetParentTile(_board[x, y]);
            enemyUnits[i].SetPosToParent();
        }
    }

}
