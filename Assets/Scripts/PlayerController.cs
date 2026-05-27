using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private float speed;
    private Rigidbody rb;
    [SerializeField] private float laneDistance = 3f;
    private int currentLane = 1;
    [SerializeField] private float jumpForce = 5f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void MoveLeft()
    {
        MoveLane(-1);
    }

    public void MoveRight()
    {
        MoveLane(1);
    }

    public void Jump()
    {
        if (!groundCheck.isGrounded) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void MovePlayer()
    {
        Vector3 moveForward = transform.forward * (speed *  Time.deltaTime);
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(rb.position.x,targetX,speed * Time.deltaTime);
        Vector3 targetPosition = new Vector3(newX, rb.position.y, rb.position.z);
        rb.MovePosition(targetPosition + moveForward);
    }

    private void MoveLane(int lane)
    {
        currentLane += lane;
        currentLane = Mathf.Clamp(currentLane, 0, 2);
    }

    
}
