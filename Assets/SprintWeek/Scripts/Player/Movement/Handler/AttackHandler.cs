using UnityEngine;
public class AttackHandler :MonoBehaviour,IAttackHandler
{
    [Header("Hadougen")]
    public GameObject hadougenPrefab;
    public Transform hadougenSpawnPoint;
    
    public float hadougenLifeTime = 10f;

    public GameObject inkPrefab;
    public Transform inkSpawnPoint;
    public float inkLifeTime = 3f;
  
    //Hadougen() and Ink() will be called as animation events
    public void Hadougen()
    {
        if (hadougenPrefab == null || hadougenSpawnPoint == null)
        {
            Debug.LogWarning("Hadougen prefab or spawn point is missing!");
            return;
        }
        GameObject hadougenInstance = Object.Instantiate(hadougenPrefab, hadougenSpawnPoint.position, hadougenSpawnPoint.rotation);
        Object.Destroy(hadougenInstance, hadougenLifeTime);

        WeaponDamage weaponDamage = hadougenInstance.GetComponent<WeaponDamage>();
        weaponDamage.SetAttacker(this.gameObject);
      
    }

    public void Ink()
    {
        if (inkPrefab == null || inkSpawnPoint == null)
        {
            Debug.LogWarning("Ink prefab or spawn point is missing!");
            return;
        }
        GameObject inkInstance = Object.Instantiate(inkPrefab, inkSpawnPoint.position, inkSpawnPoint.rotation);
        Object.Destroy(inkInstance, inkLifeTime);
    }

}
