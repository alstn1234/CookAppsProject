using UnityEngine;

public class RangedUnit : Unit, IUnitAction
{
    protected override void Awake()
    {
        base.Awake();
        Hp = 10;
    }

    public int Damage = 6;

    private const int AttackRange = 9;

    public void Attack()
    {
        animator.SetBool("IsMove", false);
        animator.SetTrigger("Attack");
        if (Target != null)
            Target.GetComponent<IDamage>().TakeDamage(Damage);
        // 공격 처리
    }

    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }
}
