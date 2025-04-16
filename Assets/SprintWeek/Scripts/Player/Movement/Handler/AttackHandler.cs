using UnityEngine;
public class AttackHandler :MonoBehaviour,IAttackHandler
{
    [Header("Hadougen")]
    public GameObject hadougenPrefab;
    public Transform hadougenSpawnPoint;

    public float hadougenLifeTime = 10f;
    public void Attack()
    {
        Hadougen();
    }

    public void Hadougen()
    {
        if (hadougenPrefab == null || hadougenSpawnPoint == null)
        {
            Debug.LogWarning("Hadougen prefab or spawn point is missing!");
            return;
        }
        GameObject hadougenInstance = Object.Instantiate(hadougenPrefab, hadougenSpawnPoint.position, hadougenSpawnPoint.rotation);
        Object.Destroy(hadougenInstance, hadougenLifeTime);
    }

}
