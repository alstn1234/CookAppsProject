using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class UnitAI
{
    private int _width = 10, _height = 5;
    private Vector2Int[] dirs = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    // 상하좌우중 맨해튼 거리가 작은 위치를 반환하는 메서드
    public Vector2Int FindNextPos2(Vector2Int startVec, Vector2Int targetVec)
    {
        Vector2Int resultVec = Vector2Int.zero;
        int distance = CalDistance(startVec, targetVec);

        foreach (var dir in dirs)
        {
            Vector2Int nextVec = startVec + dir;
            if (IsValidPos(nextVec))
            {
                int dis = CalDistance(nextVec, targetVec);
                if (dis < distance && GameManager.instance.Board[nextVec.x, nextVec.y].CheckUnit())
                {
                    distance = dis;
                    resultVec = nextVec;
                }
            }
        }

        return resultVec;
    }


    // A*알고리즘을 이용하여 최적의 길찾기 메서드
    public Vector2Int FindNextPos(int startX, int startY, int targetX, int targetY)
    {
        Vector2Int startVec = new Vector2Int(startX, startY);
        Vector2Int targetVec = new Vector2Int(targetX, targetY);

        List<Vector2Int> searchList = new List<Vector2Int>();  // 방문 할 위치 리스트
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();   // 방문 위치 저장
        Dictionary<Vector2Int, int> gCost = new Dictionary<Vector2Int, int>();   // 초기 위치 부터 현재 위치까지의 비용
        Dictionary<Vector2Int, int> fCost = new Dictionary<Vector2Int, int>();   // 초기 위치 부터 현재 위치 + 현재 위치 부터 최종 위치까지 비용
        Dictionary<Vector2Int, Vector2Int> beforePos = new Dictionary<Vector2Int, Vector2Int>();  // 이전 위치를 저장

        searchList.Add(startVec);
        gCost[startVec] = 0;
        fCost[startVec] = gCost[startVec] + CalDistance(startVec, targetVec);

        while (searchList.Count > 0)
        {
            Vector2Int curPos = GetMinFCost(searchList, fCost);

            searchList.Remove(curPos);
            visited.Add(curPos);

            foreach (var dir in dirs)
            {
                var nextPos = curPos + dir;

                if (nextPos == targetVec)
                {
                    while (beforePos[curPos] != startVec)
                    {
                        curPos = beforePos[curPos];
                    }
                    return curPos;
                }

                if (!IsValidPos(nextPos) || visited.Contains(nextPos) || !GameManager.instance.Board[nextPos.x, nextPos.y].CheckUnit()) continue;

                int nextGCost = gCost[curPos] + 1;

                if (!gCost.ContainsKey(nextPos) || nextGCost < gCost[nextPos])
                {
                    gCost[nextPos] = nextGCost;
                    fCost[nextPos] = gCost[nextPos] + CalDistance(nextPos, targetVec);
                    beforePos[nextPos] = curPos;

                    if (!searchList.Contains(nextPos)) searchList.Add(nextPos);
                }
            }
        }
        return FindNextPos2(startVec, targetVec);
    }

    private Vector2Int GetMinFCost(List<Vector2Int> searchList, Dictionary<Vector2Int, int> fCost)
    {
        var minCostPos = searchList[0];
        var minCost = fCost[minCostPos];
        foreach (Vector2Int pos in searchList)
        {
            if (fCost[pos] < minCost)
            {
                minCostPos = pos;
                minCost = fCost[pos];
            }
        }
        return minCostPos;
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
