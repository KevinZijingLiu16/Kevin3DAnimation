using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;

    [SerializeField] private float drag = 0.3f;
    [SerializeField] private NavMeshAgent agent;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity; //=> is return


   

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; //using + is because gravity is negative
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag );
        if (agent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }
        controller.Move(Movement * Time.deltaTime);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;

        if(agent != null)
        {
            agent.enabled = false; //set the agent be disabled when apply knockback
        }

    }

}
