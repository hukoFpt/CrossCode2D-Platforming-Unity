namespace CrossCode2D.Enemies
{
    public interface IEnemy
    {
        EnemyStats Stats { get; }
        HealthBar HealthBar { get; }
        void TakeDamage(float attackerAttack);
        void Die();
    }
}