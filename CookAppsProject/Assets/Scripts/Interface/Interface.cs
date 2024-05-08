
public interface IDamage
{
    void TakeDamage(int damage);
}

public interface IUnitAction
{
    bool CanAttack();
    void Attack();
}