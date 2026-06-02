using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private float speed;
    private Animator anim;
    [SerializeField] private BasicObjectPooler effectPooler;
    private Rigidbody rb;
    [SerializeField] private float laneDistance = 3f;
    private int currentLane = 1;
    [SerializeField] private float jumpForce = 5f;
    private bool isDead = false;
    private float stumbleSpeed;
    private float normalSpeed;
    private bool isInvulnerable = false;
    public static event Action<GameObject> OnHitObstacle; 
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Health.Instance.OnDeath += HandleDeath;
        stumbleSpeed = speed / 2;
        normalSpeed = speed;
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
        anim.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void MovePlayer()
    {
        anim.SetBool("IsRunning", true);
        float totalSpeed =  speed * Time.deltaTime * DifficultyManager.Instance.difficulty.movementSpeedMultiplier;
        Vector3 moveForward = transform.forward * totalSpeed;
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(rb.position.x,targetX, totalSpeed);
        Vector3 targetPosition = new Vector3(newX, rb.position.y, rb.position.z);
        rb.MovePosition(targetPosition + moveForward);
    }

    private void HandleDeath()
    {
        speed = 0;
        isDead = true;
        anim.SetBool("IsRunning", false);
        anim.SetTrigger("IsDead");
    }

    private void MoveLane(int lane)
    {
        currentLane += lane;
        currentLane = Mathf.Clamp(currentLane, 0, 2);
    }

    private IEnumerator HandleObstacleHit()
    {
        isInvulnerable = true;
        Health.Instance.LoseLife();
        if (isDead) yield break;
        anim.SetTrigger("Stumble");
        speed = stumbleSpeed;
        yield return new WaitForSeconds(0.7f);
        if (!isDead) speed = normalSpeed;
        isInvulnerable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if(isDead|| isInvulnerable) return;
            OnHitObstacle?.Invoke(other.gameObject);
            GameObject effect = effectPooler.GetPooledObject();
            effect.transform.position = other.transform.position;
            effect.GetComponent<ReturnEffectToPool>().pool = effectPooler; 
            effect.GetComponent<ParticleSystem>().Play();
            StartCoroutine(HandleObstacleHit());
        }
    }
    void OnDestroy()
    {
        Health.Instance.OnDeath -= HandleDeath;
    }
}
