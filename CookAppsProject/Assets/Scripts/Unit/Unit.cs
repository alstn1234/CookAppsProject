using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    protected Animator animator;
    private UnitAI _unitAI;

    protected virtual void Awake()
    {
        _unitAI = new UnitAI();
        animator = GetComponent<Animator>();
    }
    protected void Die()
    {
        if (gameObject.layer == LayerMask.NameToLayer("MyUnit"))
        {
            GameManager.instance.MyUnits.Remove(this);
        }
        else
        {
            GameManager.instance.EnemyUnits.Remove(this);
        }
        Destroy(gameObject, 1f);

        GameManager.instance.CheckEndGame();
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
        Debug.Log(Hp);
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
        animator.SetBool("IsMove", true);
        var nextPos = NextPos();
        var nextTile = GameManager.instance.Board[nextPos.x, nextPos.y];
        StartCoroutine(MoveNextPos(nextTile));
    }

    IEnumerator MoveNextPos(Tile nextTile)
    {
        float finalTime = 0.5f;
        float time = 0f;
        float per = 0f;
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
