using UnityEngine;

public class MeleeAOEUnit : Unit, IUnitAction
{
    protected override void Awake()
    {
        base.Awake();
        Hp = 15;
    }

    //public int Damage = 8;
    public int Damage = 1;

    private const int AttackRange = 2;
    public void Attack()
    {
        FlipX();
        animator.SetTrigger("Attack");

        // 광역 딜 처리
        var board = GameManager.instance.Board;
        var attackRangeList = unitAI.FindAttackRange(ParentTile.x, ParentTile.y);
        foreach (var AttackRange in attackRangeList)
        {
            var attackTile = board[AttackRange.x, AttackRange.y];
            attackTile.ChangeColor(IsMine);
            if (!attackTile.CheckUnit() && !attackTile.Unit.IsTeam(IsMine))
            {
                attackTile.Unit.TakeDamage(Damage);
            }
        }

        // 공격 처리
    }

    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }
}
