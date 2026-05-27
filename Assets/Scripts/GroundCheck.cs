using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float radius = 0.3f;

    void Update()
    {
        CheckGround();
    }
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, radius, groundLayer);
    }
}
