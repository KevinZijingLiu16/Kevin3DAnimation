using UnityEngine;

public class PlayerNoiseTest : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           
            audioSource.Play();

           
            NoiseSystem.EmitNoise(transform.position, this.gameObject);
            Debug.Log("Player emitted noise");
        }
    }

}
