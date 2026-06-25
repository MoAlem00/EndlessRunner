using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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
    /// <summary>Percentage of the screen width to consider when swiping to be considered a swipe, else its a jump.</summary> 
    const float X_SWIPE_DEAD_ZONE_PERCENTAGE = 0.05f;
    /// <summary> If True, validate touches. </summary>
    private bool touchStartedInGame;
    private Vector2 startTouchPosition, endTouchPosition;
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


private void OnEnable()
{
    EnhancedTouchSupport.Enable();
#if UNITY_EDITOR
    TouchSimulation.Enable();
#endif
}

private void OnDisable()
{
#if UNITY_EDITOR
    TouchSimulation.Disable();
#endif
    EnhancedTouchSupport.Disable();
}



private void Update()
{
    foreach (var touch in Touch.activeTouches)
    {
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            if (!GameManager.Instance.IsPlaying()) continue;

            touchStartedInGame = true;
            startTouchPosition = touch.screenPosition;
        }

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            if (!touchStartedInGame) continue;

            touchStartedInGame = false;

            endTouchPosition = touch.screenPosition;

            float deadZone = Screen.width * X_SWIPE_DEAD_ZONE_PERCENTAGE;
            float distance = Mathf.Abs(endTouchPosition.x - startTouchPosition.x);

            if (distance <= deadZone)
                Jump();
            else if (endTouchPosition.x < startTouchPosition.x)
                MoveLeft();
            else
                MoveRight();
        }
    }
}

    private void FixedUpdate()
    {
        if(!GameManager.Instance.IsPlaying()) return;
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
        StartCoroutine(GameManager.Instance.ShowGameOverAfterDelay());
        jumpForce = 0;
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
