using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour, IDamage
{
    [HideInInspector]
    public Tile ParentTile;
    public Unit Target;
    protected bool IsMine;

    protected Animator animator;
    protected UnitHp unitHp;

    protected virtual void Awake()
    {
        unitHp = GetComponent<UnitHp>();
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
        Destroy(gameObject, 0.1f);
    }

    public void SetParentTile(Tile newParent)
    {
        if (newParent.CheckUnit())
        {
            if (ParentTile != null) ParentTile.DeleteUnit();
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
        unitHp.ChangeHp(damage);
        if (unitHp.IsDie()) Die();
        Debug.Log(gameObject.name + "가" + damage + "입음   남은 체력:" + unitHp.Hp);
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
        int x = 1;
        if (Target == null) return;
        if (ParentTile.x > Target.ParentTile.x) 
            x = -1;
        transform.localScale = new Vector3(x, 1, 1);
        unitHp.FlipXSlider(x);
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
}
