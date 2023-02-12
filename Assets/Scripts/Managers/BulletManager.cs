using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public void DestroyAllBullets()
    {
        Bullet[] allBullets = FindObjectsOfType<Bullet>();
        for (int i = 0; i < allBullets.Length; i++)
        {
            allBullets[i].DestroyBullet();
        }
    }
    
    public void ConfigureBullets(Bullet[] bullets, bool isOnPlayer)
    {
        SetLayerBullets(bullets, isOnPlayer);
        OrientBullets(bullets);
    }
    
    private void SetLayerBullets(Bullet[] bullets, bool isOnPlayer)
    {
        if (bullets == null) return;

        foreach (Bullet bullet in bullets)
        {
            if (isOnPlayer)
            {
                bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
            }
            else
            {
                bullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
            }
        }
    }

    private void OrientBullets(Bullet[] bullets)
    {
        if (bullets == null) return;
        
        foreach (Bullet bullet in bullets)
        {
            Vector3 aimDirection = bullet.GetComponent<Rigidbody2D>().velocity;
            float aimAngle = Mathf.Atan(aimDirection.y / aimDirection.x) * Mathf.Rad2Deg;
            
            Vector3 rot = bullet.transform.rotation.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(rot.x, rot.y, aimAngle);

            if (aimDirection.x > 0f) continue;
            
            Vector3 bScale = bullet.transform.localScale;
            bullet.transform.localScale = new Vector3(-Mathf.Abs(bScale.x), bScale.y, bScale.z);
        }
    }
}