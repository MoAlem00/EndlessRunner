using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float speed;
    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * (speed * Time.fixedDeltaTime));
    }
}
