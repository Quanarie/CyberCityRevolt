public class EnemyHealth : Health
{
    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
}
