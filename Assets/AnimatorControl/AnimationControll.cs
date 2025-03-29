using UnityEngine;

public class AnimationControll : MonoBehaviour
{
    public Animator animator;
    private bool isRunning = false;

    [Header("TransitionLine")]
    int isRunningHash = Animator.StringToHash("IsRunning");
    [Header("BlenderTree")]
    int velocityHash = Animator.StringToHash("Velocity");
    private float velocity = 0f;
    private bool increasing = true;
    public float speed = 0.1f;
    [Header("Hadougen")]
    public GameObject hadougen;
    public Transform hadougenSpawnPoint;
    
    public float hadougenLifeTime = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClickRunning()
    {
        isRunning = !isRunning;
        animator.SetBool(isRunningHash, isRunning);

    }

    public void OnClickHadougen()
    {
       animator.SetTrigger("Hadougen");
    }

    void Update()
    {
        ChangeRunSpeed();
    }

    private void ChangeRunSpeed()
    {
        if (increasing)
        {
            velocity += Time.deltaTime * speed;
            if (velocity >= 1f)
            {
                velocity = 1f;
                increasing = false;
            }
        }
        else
        {
            velocity -= Time.deltaTime * speed;
            if (velocity <= 0f)
            {
                velocity = 0f;
                increasing = true;
            }
        }

        animator.SetFloat(velocityHash, velocity);
    }

    public void Hadougen()
    {
        GameObject hadougenInstance = Instantiate(hadougen, hadougenSpawnPoint.position, hadougenSpawnPoint.rotation);
        
        Destroy(hadougenInstance, hadougenLifeTime);
    }
}
