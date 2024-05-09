using UnityEngine;

public class RangedUnit : Unit, IUnitAction
{
    protected override void Awake()
    {
        base.Awake();
        Hp = 10;
    }

    //public int Damage = 4;
    public int Damage = 1;

    private const int AttackRange = 9;

    public void Attack()
    {
        FlipX();
        animator.SetTrigger("Attack");

        if (Target != null && !Target.IsTeam(IsMine))
        {
            Target.GetComponent<IDamage>().TakeDamage(Damage);
            Target.ParentTile.ChangeColor(IsMine);
        }
    }

    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }
}
