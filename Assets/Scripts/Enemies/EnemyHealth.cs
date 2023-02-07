public class EnemyHealth : Health
{
    // When bullets simultaneously kill enemy then death gets called more than once
    // And that invokes Dying couple of times, which is not wanted for EnemySpawner stage isDone detector
    private bool isDead = false;
    
    protected override void Death()
    {
        if (isDead) return;
        
        isDead = true;
        base.Death();
        Destroy(gameObject);
    }
}
