using System.Collections.Generic;
using UnityEngine;

public class MeleeAOEUnit : Unit, IUnitAction
{
    public int MaxHp;
    protected override void Awake()
    {
        base.Awake();
        unitHp.MaxHp = MaxHp;
    }

    public int Damage;

    private const int AttackRange = 2;
    public void Attack()
    {
        FlipX(DecideDir(ParentTile.x, Target.ParentTile.x));
        animator.SetTrigger("Attack");

        // 광역 딜
        var board = GameManager.instance.Board;
        var attackRangeList = FindAttackRange(ParentTile.x, ParentTile.y);
        foreach (var AttackRange in attackRangeList)
        {
            var attackTile = board[AttackRange.x, AttackRange.y];
            attackTile.ChangeColor(IsMine);
            if (!attackTile.CheckUnit() && !attackTile.Unit.IsTeam(IsMine))
            {
                attackTile.Unit.TakeDamage(Damage);
            }
        }
    }

    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }

    public List<Vector2Int> FindAttackRange(int x, int y)
    {
        List<Vector2Int> resultVec = new List<Vector2Int>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && i == j) continue;
                Vector2Int vec = new Vector2Int(x + i, y + j);
                if (IsValidPos(vec))
                {
                    resultVec.Add(vec);
                }
            }
        }
        return resultVec;
    }

    private bool IsValidPos(Vector2Int nextVec)
    {
        return nextVec.x >= 0 && nextVec.y >= 0 && nextVec.x < GameManager.instance.Width && nextVec.y < GameManager.instance.Height;
    }
}
