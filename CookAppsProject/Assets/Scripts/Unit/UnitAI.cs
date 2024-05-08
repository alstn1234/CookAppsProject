using UnityEngine;

public class UnitAI : MonoBehaviour
{
    private int _width = 10, _height = 10;
    private Vector2Int[] dirs = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    public Vector2Int FindNextPos(int startX, int startY, int targetX, int targetY)
    {
        Vector2Int startVec = new Vector2Int(startX, startY);
        Vector2Int targetVec = new Vector2Int(targetX, targetY);
        Vector2Int resultVec = Vector2Int.zero;
        int distance = -1;

        foreach (var dir in dirs)
        {
            Vector2Int nextVec = startVec + dir;
            if (IsValidPos(nextVec))
            {
                int dis = CalDistance(nextVec, targetVec);
                if((dis < distance || distance == -1) && GameManager.instance.Board[nextVec.x, nextVec.y].CheckUnit())
                {
                    distance = dis;
                    resultVec = nextVec;
                }
            }
        }

        return resultVec;
    }

    private int CalDistance(Vector2Int currentVec, Vector2Int targetVec)
    {
        return Mathf.Abs(currentVec.x - targetVec.x) + Mathf.Abs(currentVec.y - targetVec.y);
    }

    private bool IsValidPos(Vector2Int nextVec)
    {
        return nextVec.x >= 0 && nextVec.y >= 0 && nextVec.x < _width && nextVec.y < _height;
    }
}
