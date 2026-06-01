using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float leftX = -3.5f;
    [SerializeField] private float rightX = 3.6f;
    private Vector3 direction = Vector3.right;

    void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        if (transform.position.x >= rightX)      { SetX(rightX); direction = Vector3.left; }
        else if (transform.position.x <= leftX)  { SetX(leftX);  direction = Vector3.right; }
    }

    void SetX(float x)
    {
        Vector3 p = transform.position;
        p.x = x;
        transform.position = p;
    }
}
