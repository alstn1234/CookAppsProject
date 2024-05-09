using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour, IDamage
{
    private int _hp;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value < 0 ? 0 : value;
            if (_hp <= 0) Die();
        }
    }

    [HideInInspector]
    public Tile ParentTile;
    protected Unit Target;
    protected bool IsMine;

    protected Animator animator;
    protected UnitAI unitAI;

    protected virtual void Awake()
    {
        unitAI = new UnitAI();
        animator = GetComponent<Animator>();
        IsMine = gameObject.layer == LayerMask.NameToLayer("MyUnit");
    }

    public bool IsTeam(bool isMine)
    {
        return isMine == this.IsMine;
    }
    protected void Die()
    {
        if (IsMine)
        {
            GameManager.instance.MyUnits.Remove(this);
        }
        else
        {
            GameManager.instance.EnemyUnits.Remove(this);
        }
        Destroy(gameObject, 0.5f);

        GameManager.instance.CheckEndGame();
    }

    public void SetParentTile(Tile newParent)
    {
        if (newParent.CheckUnit())
        {
            if(ParentTile != null)  ParentTile.DeleteUnit();
            newParent.SetUnit(this);
            ParentTile = newParent;
        }
        SetPosToParent();
        FlipX();
    }

    public void SetPosToParent()
    {
        gameObject.transform.position = ParentTile.transform.position;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;

        Debug.Log(gameObject.name + "가" + damage +"입음   남은 체력:" + Hp);
    }

    // 가까운 적을 찾아서 target으로 설정하는 메서드
    public void SetTarget()
    {
        int distance = int.MaxValue;
        Unit closestEnemy = null;

        int x, y, nDistance;
        List<Unit> enemyUnits = IsMine ? GameManager.instance.EnemyUnits : GameManager.instance.MyUnits;
        foreach (var enemy in enemyUnits)
        {
            x = ParentTile.x - enemy.ParentTile.x;
            y = ParentTile.y - enemy.ParentTile.y;
            nDistance = x * x + y * y;

            if (nDistance < distance)
            {
                closestEnemy = enemy;
                distance = nDistance;
            }
        }
        Target = closestEnemy;
    }

    // target방향을 바라보는 메서드
    public void FlipX()
    {
        if (Target == null) return;
        if (ParentTile.x > Target.ParentTile.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    // 공격 범위안에 드는지 확인하는 메서드
    public bool IsInAttackRange(int AttackRange)
    {
        SetTarget();
        if (Target == null) return false;
        int x = ParentTile.x - Target.ParentTile.x;
        int y = ParentTile.y - Target.ParentTile.y;
        if (x * x + y * y <= AttackRange) return true;
        return false;
    }

    // 다음 움직일 위치를 반환하는 메서드
    public Vector2Int NextPos()
    {
        if (Target == null) return Vector2Int.zero;
        return unitAI.FindNextPos(ParentTile.x, ParentTile.y, Target.ParentTile.x, Target.ParentTile.y);
    }

    // 다음 위치로 움직이는 메서드
    public void Move()
    {
        animator.SetBool("IsMove", true);
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
            transform.position = Vector2.Lerp(ParentTile.transform.position, nextTile.transform.position, per);
            yield return null;
        }
        SetParentTile(nextTile);
        animator.SetBool("IsMove", false);
    }
}
