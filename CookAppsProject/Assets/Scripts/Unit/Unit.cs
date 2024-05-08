using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour, IDamage
{
    private int _hp = 10;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value < 0 ? 0 : value;
        }
    }

    [HideInInspector]
    public Tile ParentTile;
    public Unit Target;

    protected Animator animator;
    private UnitAI _unitAI;

    private void Awake()
    {
        _unitAI = GetComponent<UnitAI>();
        animator = GetComponent<Animator>();
    }

    public void SetParentTile(Tile newParent)
    {
        if (!newParent.CheckUnit())
        {
            SetPosToParent();
        }
        newParent.SetUnit(this);
        ParentTile = newParent;
    }

    public void SetPosToParent()
    {
        gameObject.transform.position = ParentTile.transform.position;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log($"{damage}의 데미지를 입음");
    }

    // 가까운 적을 찾아서 target으로 설정하는 메서드
    public void SetTarget()
    {
        int distance = -1;
        int x, y, nDistance;
        List<Unit> enemyUnits = gameObject.layer == LayerMask.NameToLayer("MyUnit") ? GameManager.instance.EnemyUnits : GameManager.instance.MyUnits;
        foreach (var enemy in enemyUnits)
        {
            x = ParentTile.x - enemy.ParentTile.x;
            y = ParentTile.y - enemy.ParentTile.y;
            nDistance = x * x + y * y;

            if (nDistance < distance || distance == -1)
            {
                Target = enemy;
                distance = nDistance;
                if (x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public bool IsInAttackRange(int AttackRange)
    {
        SetTarget();
        int x = ParentTile.x - Target.ParentTile.x;
        int y = ParentTile.y - Target.ParentTile.y;
        if (x * x + y * y <= AttackRange) return true;
        return false;
    }

    public Vector2Int NextPos()
    {
        return _unitAI.FindNextPos(ParentTile.x, ParentTile.y, Target.ParentTile.x, Target.ParentTile.y);
    }
    public void Move()
    {
        Debug.Log("move");
        animator.SetBool("IsMove", true);
        var nextPos = NextPos();
        Debug.Log(nextPos.x + nextPos.y);
        var nextTile = GameManager.instance.Board[nextPos.x, nextPos.y];
        StartCoroutine(MoveNextPos(nextTile));
        SetParentTile(nextTile);
    }

    IEnumerator MoveNextPos(Tile nextTile)
    {
        float finalTime = 0.8f;
        float time = 0f;
        Vector3 vec = nextTile.transform.position - ParentTile.transform.position;
        float distance = Mathf.Abs(vec.x) + Mathf.Abs(vec.y);
        var dir = Vector3.Normalize(vec);
        while (true)
        {
            if (time > finalTime) break;
            var deltaTime = Time.deltaTime;
            transform.position += distance * dir / finalTime * deltaTime;
            time += deltaTime;
        }
        yield return null;
    }
}
