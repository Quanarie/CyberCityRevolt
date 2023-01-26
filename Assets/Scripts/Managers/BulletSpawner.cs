using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public void SpawnSingleBullet(Vector2 target, GameObject prefab, int damage, 
        float speed, Vector3 spawnPoint, out Bullet[] toSave)
    {
        toSave = new Bullet[1];
        toSave[0] = Instantiate(prefab, spawnPoint, Quaternion.identity).GetComponent<Bullet>();
        toSave[0].Initialize(target, damage, speed);
    }
    
    public void SpawnCircleOfBullets(Vector2 target, GameObject prefab, int damage, 
        float speed, Vector3 spawnPoint, int quantity, float angle, out Bullet[] toSave)   
    {
        toSave = new Bullet[quantity];
        var radius = Vector2.Distance(spawnPoint, target);
        var angleBetweenMeAndTarget = -Vector2.SignedAngle(Vector2.up, target - (Vector2)spawnPoint);
        var currentAngle = angleBetweenMeAndTarget - angle / 2;
        var differenceAngle = angle / (quantity + 1);
        
        for (int i = 0; i < quantity; i++)
        {
            currentAngle += differenceAngle;
            var x = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius + spawnPoint.x;
            var y = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius + spawnPoint.y;
            var temp = new Vector2(x, y);

            toSave[i] = Instantiate(prefab, spawnPoint, Quaternion.identity).GetComponent<Bullet>();
            toSave[i].Initialize(temp, damage, speed);
        }
    }
}
