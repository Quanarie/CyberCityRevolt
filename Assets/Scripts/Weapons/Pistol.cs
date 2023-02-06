public class Pistol : Weapon
{
    protected override void Shoot()
    {
        Singleton.Instance.BulletSpawner.SpawnSingleBullet(input.GetTarget(), info, out spawnedBullets);
    }
}