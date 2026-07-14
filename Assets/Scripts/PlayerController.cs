using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool wasGrounded;
    [SerializeField] AudioClip concreteHitSound;
    [SerializeField] AudioClip landingSound;
    public static event Action<GameObject> OnHitObstacle; 
    /// <summary>Percentage of the screen width to consider when swiping to be considered a swipe, else its a jump.</summary> 
    const float X_SWIPE_DEAD_ZONE_PERCENTAGE = 0.05f;

    private bool touchStartedInGame;
    private Vector2 startTouchPosition, endTouchPosition;

    private Dictionary<PowerUpType, float> effectEndTimes = new();

    [SerializeField] private Vector3 magnetSize = new Vector3(15,5,5);

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

        if(!Score.Instance.isDoubleScore && HasEffect(PowerUpType.Multiplier) ||
         Score.Instance.isDoubleScore && !HasEffect(PowerUpType.Multiplier))
            Score.Instance.ToggleDoubleScore();
        

        if (effectEndTimes.Count > 0)
        {
            var keys = new List<PowerUpType>(effectEndTimes.Keys);

            foreach (var key in keys)
            {
                if (Time.time > effectEndTimes[key])
                    effectEndTimes.Remove(key);
            }
        }

        if (InputManager.Instance.CurrentType == InputType.Swipe)
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
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying()) return;
        MovePlayer();
        if(HasEffect(PowerUpType.Magnetic)) Attract();
    }

    void Attract()
    {
        Vector3 size = Vector3.Scale(transform.localScale, magnetSize);
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, size, Quaternion.identity);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].CompareTag("Coin"))
            {
                Transform trans = hitColliders[i].gameObject.transform;
                trans.position = Vector3.MoveTowards(trans.position, gameObject.transform.position, 30 * Time.deltaTime);
            }
            i++;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, magnetSize);
    }

    /// <summary>
    /// Checks if the Effect is active.
    /// </summary>
    public bool HasEffect(PowerUpType type)
    {
        return effectEndTimes.TryGetValue(type, out float endTime)
               && Time.time < endTime;
    }

    /// <summary>
    /// Adds an Effect which ends hwen a certain game time is reached.<br/>
    /// Should respect current duration of effects and override it only if it will extend the duration, as if multiple of they were mutliple instances of the same effect. 
    /// </summary>
    public void AddEffect(PowerUpType type, float duration)
    {
        float newEndTime = Time.time + duration;

        if (effectEndTimes.TryGetValue(type, out float existingEndTime))
            effectEndTimes[type] = Mathf.Max(existingEndTime, newEndTime);
        else
            effectEndTimes[type] = newEndTime;
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

        float totalSpeed = speed * Time.deltaTime * DifficultyManager.Instance.difficulty.movementSpeedMultiplier;

        Vector3 moveForward = transform.forward * totalSpeed;
        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.Lerp(rb.position.x, targetX, totalSpeed);

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
        AddEffect(PowerUpType.Invulnerable, 0.7f);

        Health.Instance.LoseLife();
        if (isDead) yield break;

        anim.SetTrigger("Stumble");
        speed = stumbleSpeed;

        yield return new WaitForSeconds(0.7f);

        if (!isDead) speed = normalSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (isDead) return;

            OnHitObstacle?.Invoke(other.gameObject);

            GameObject effect = effectPooler.GetPooledObject();
            effect.transform.position = other.transform.position;
            effect.GetComponent<ReturnEffectToPool>().pool = effectPooler;
            effect.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySfx(concreteHitSound,0.5f);

            if (!HasEffect(PowerUpType.Invulnerable))
                StartCoroutine(HandleObstacleHit());
        }
    }

    void OnDestroy()
    {
        Health.Instance.OnDeath -= HandleDeath;
    }
}