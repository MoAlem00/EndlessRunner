using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float radius = 0.3f;
    Vector3 fallGravity = new Vector3(0f, -13f, 0f);
    Vector3 normalGravity = new Vector3(0f, -9.5f, 0f);
    
    void Update()
    {
        CheckGround();
        if (!isGrounded)
        {
            Physics.gravity = fallGravity;
        }
        else
        {
            Physics.gravity = normalGravity;
        }
    }
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, radius, groundLayer);
    }
}
