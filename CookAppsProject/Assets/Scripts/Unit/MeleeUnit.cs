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

        // ���� ó��
    }

    // ���� ������ ���� �ִ��� üũ�ϴ� �޼���
    public bool CanAttack()
    {
        return IsInAttackRange(AttackRange);
    }


}
