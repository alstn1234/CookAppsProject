using UnityEngine;

public class Board : MonoBehaviour
{
    private int _width = 10;
    private int _height = 5;
    private Tile[,] _board;

    [SerializeField] private GameObject _tilePrefab;

    private void Start()
    {
        _board = new Tile[_width, _height];
        Init();
    }

    private void Init()
    {
        GameObject tileObject;
        for(int y = 0; y < _height; y++)
        {
            for(int x = 0; x < _width; x++)
            {
                tileObject = Instantiate(_tilePrefab, gameObject.transform);
                var tile = tileObject.GetComponent<Tile>();
                tile.Init(x, y);

                _board[x, y] = tile;
            }
        }
    }
}
