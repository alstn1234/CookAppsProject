using UnityEngine;

public class MeleeAOEUnit : Unit, IUnitAction
{
    protected override void Awake()
    {
        base.Awake();
        Hp = 15;
    }

    public int Damage = 8;

    private const int AttackRange = 2;
    public void Attack()
    {
        animator.SetBool("IsMove", false);
        animator.SetTrigger("Attack");

        // 광역 딜 처리
        if (Target != null)
            Target.GetComponent<IDamage>().TakeDamage(Damage);

        // 공격 처리
    }

    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }
}
