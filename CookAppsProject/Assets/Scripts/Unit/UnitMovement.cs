using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitMovement : MonoBehaviour
{
    private Unit _unit;
    private UnitAI _unitAI;
    private Animator _animator;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _animator = GetComponent<Animator>();
        _unitAI = new UnitAI();
    }
    // 다음 움직일 위치를 반환하는 메서드
    public Vector2Int NextPos()
    {
        if (_unit.Target == null) return Vector2Int.zero;
        return _unitAI.FindNextPos(_unit.ParentTile.x, _unit.ParentTile.y, _unit.Target.ParentTile.x, _unit.Target.ParentTile.y);
    }

    // 다음 위치로 움직이는 메서드
    public void Move()
    {
        _animator.SetBool("IsMove", true);
        var nextPos = NextPos();
        if (nextPos == Vector2Int.zero) return;
        var nextTile = GameManager.instance.Board[nextPos.x, nextPos.y];
        StartCoroutine(MoveNextPos(nextTile));
    }

    IEnumerator MoveNextPos(Tile nextTile)
    {
        float finalTime = 0.8f;
        float moveDelayTime = 0f;
        float time = 0f;
        float per = 0f;
        yield return new WaitForSeconds(moveDelayTime);
        while (per < 1)
        {
            time += Time.deltaTime;
            per = time / finalTime;
            transform.position = Vector2.Lerp(_unit.ParentTile.transform.position, nextTile.transform.position, per);
            yield return null;
        }
        _unit.SetParentTile(nextTile);
        _animator.SetBool("IsMove", false);
    }
}
