using UnityEngine;

public class RangedUnit : Unit, IUnitAction
{
    public int MaxHp;
    protected override void Awake()
    {
        base.Awake();
        unitHp.MaxHp = MaxHp;
    }

    public int Damage;

    private const int AttackRange = 9;

    public void Attack()
    {
        FlipX(DecideDir(ParentTile.x, Target.ParentTile.x));
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
