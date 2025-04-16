using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private Camera mainCamera;
    public List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);

        target.OnDestroyedEvent += RemoveTarget; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        RemoveTarget(target);

    }

    public bool SelectTarget()
    { 
        if(targets.Count == 0) { return false; }
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets)
        { 
         Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
            { 
              continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if(toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }



        }

        if(closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
       
        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f); // 1f is weight, 2f is radius

        return true;



    }
    public void Cancel()
    {
        if (CurrentTarget == null) { return; }
        cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
           cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyedEvent -= RemoveTarget;
        targets.Remove(target);
    }

}
