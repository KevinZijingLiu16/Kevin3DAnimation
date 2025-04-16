using UnityEngine;

public class InventoryMovementTest : MonoBehaviour
{
    public float moveSpeed = 5f;

    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogWarning("Missing CharacterController! Please add one to your character.");
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");     

       
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        moveDirection = (forward * vertical + right * horizontal).normalized;

        if (controller != null)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
           
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
