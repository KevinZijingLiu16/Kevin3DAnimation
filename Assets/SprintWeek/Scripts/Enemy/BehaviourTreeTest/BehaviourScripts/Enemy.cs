using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] PerceptionComponent perceptionComponent;

    GameObject target;

    void Start()
    {
        perceptionComponent.onPerceptionTargetChanged += TargetChanged;
    }

    private void TargetChanged(GameObject target, bool sensed)
    {
        if (sensed)
        {
            this.target = target;
            
        }
        else
        {
            this.target = null;
            
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 drawTargetPos = target.transform.position + Vector3.up;
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(drawTargetPos, 0.7f);
    //    Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
    //}
}
