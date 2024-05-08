using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeUnit : Unit, IUnitAction
{
    public int Damage = 10;
    private const int AttackRange = 2;


    public void Attack()
    {
        animator.SetBool("IsMove", false);
        animator.SetTrigger("Attack");
        //GameManager.instance.Board[targetX, targetY].Unit.GetComponent<IDamage>().TakeDamage(_unit.Damage);

        // 공격 처리
    }

    // 공격 범위에 적이 있는지 체크하는 메서드
    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }


}
