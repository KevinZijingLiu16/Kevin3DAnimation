using System;
using UnityEngine;

public class GrabTrigger : MonoBehaviour
{
    
    public static event Action OnPlayerGrabbed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ragdoll ragdoll = other.GetComponent<Ragdoll>();
            if (ragdoll != null)
            {
                ragdoll.ToggleRagdoll(true);
                OnPlayerGrabbed?.Invoke();
                
            }
           
        }
    }

   

}
