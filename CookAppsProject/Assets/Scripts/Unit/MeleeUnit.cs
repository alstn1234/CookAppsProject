using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeUnit : Unit, IUnitAction
{
    protected override void Awake()
    {
        base.Awake();
        unitHp.MaxHp = 30;
    }

    public int Damage;

    private const int AttackRange = 2;


    public void Attack()
    {
        FlipX(DecideDir(ParentTile.x, Target.ParentTile.x));
        animator.SetTrigger("Attack");

        if (Target != null && !Target.IsTeam(IsMine))
        {
            Target.GetComponent<IDamage>().TakeDamage(Damage);
            Target.ParentTile.ChangeColor(IsMine);
        }

        // 공격 처리
    }

    // 공격 범위에 적이 있는지 체크하는 메서드
    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }


}
