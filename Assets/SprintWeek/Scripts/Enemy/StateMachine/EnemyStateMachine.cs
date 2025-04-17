using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{ get; private set; }
    
    public GameObject Player { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent{ get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float DetectPlayerRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public PerceptionComponent perceptionComponent { get; private set; }
    [field: SerializeField] public Transform[] PatrolPoints { get; private set; }
    [field: SerializeField] public Transform HeadTransform { get; private set; }

    private bool canSeePlayer;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SwitchState(new EnemyPatrolState(this));

        Agent.updatePosition = false;
        Agent.updateRotation = false;
        perceptionComponent.onPerceptionTargetChanged += HandlePerceptionTargetChanged;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectPlayerRange);
    }
    private void OnEnable()
    {
        GrabTrigger.OnPlayerGrabbed += HandlePlayerGrabbed;
    }

    private void OnDisable()
    {
        GrabTrigger.OnPlayerGrabbed -= HandlePlayerGrabbed;
        perceptionComponent.onPerceptionTargetChanged -= HandlePerceptionTargetChanged;
    }
    private void HandlePlayerGrabbed()
    {
       
        StartCoroutine(SwitchToFrozenStateAfterDelay(1f));
    }
    private IEnumerator SwitchToFrozenStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        
        SwitchState(new EnemyFrozenState(this));
    }
    public bool CanSensePlayer()
    {
        return canSeePlayer;
    }
    private void HandlePerceptionTargetChanged(GameObject target, bool sensed)
    {
        if (target == Player)
        {
            canSeePlayer = sensed;
        }
    }


}
