using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LunalaController : MonoBehaviour
{
    // Start is called before the first frame update
     public Rigidbody rb;
    public float moveSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public Animator anim;

    [SerializeField] private float dashTime;
    [SerializeField] private float dashVal;
    [SerializeField] private GameObject dashSmokePrefab;
    private float dashSpeed;

    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isFlipped;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isDead = false;
    private bool isMoving;

    [HideInInspector] public float inputH;
    [HideInInspector] public float inputV;

    private bool canDash = true;
    private float dx, dz;
    private Vector3 dir;
    private Vector3 dashDir;

    public HealthBar healthBar;
    public float currenthealth;
    public float maxHealth;
    public float points;
    public float defense = 0;
    [SerializeField] public float damage;

    public static LunalaController instance;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        speed = moveSpeed;
        healthBar.SetMaxHealth(maxHealth);
        currenthealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        // if(SceneManager.GetActiveScene().name != "Tutorial")
        // {
        //     InvokeRepeating("damageOverTime", 0.0f, 1.0f);
        // }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level3") {
            if (Timer.instance != null) {
                if (Timer.instance.currentTime == 0) {
                    currenthealth = 0;
                    healthBar.SetHealth(currenthealth);
                }
            }
        }
        if (Time.timeScale != 0) {
            if (!isDead) {
                if (!isHurt) {
                    if (!isAttacking)
                    {
                        footSteps();
                        Movement();

                        if (isDashing) {
                            // altered code from 
                            // https://www.youtube.com/watch?v=Bf_5qIt9Gr8&t=35s
                            float dashDrop = 5f;
                            dashSpeed -= dashSpeed * dashDrop * Time.deltaTime;
                            float minDashSpeed = 5f;
                            if (dashSpeed < minDashSpeed) {
                                isDashing = false; 
                                anim.SetBool("dashing", false);
                            }
                        }

                        if (Input.GetKeyDown(KeyCode.Space) && canDash && isMoving)
                        {
                            StartCoroutine(Dash());
                        }
                    }
                    MeleeAttack();
                }
            }
        }
        if (Time.timeScale == 0) {
            FindObjectOfType<SoundManager>().Enable("Foot Step", false);
        }
    }

    private void footSteps() {
        if (isMoving)
        {
            FindObjectOfType<SoundManager>().Enable("Foot Step", true);
        }
        else
        {
            FindObjectOfType<SoundManager>().Enable("Foot Step", false);
        }
    }

    // Perform the dash
    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        anim.SetBool("dashing", true);
        FindObjectOfType<SoundManager>().Play("Dash");
        dashDir = dir;
        dashSpeed = dashVal;    

        Vector3 cloudPos;
        GameObject dashCloud;
        // Create dust cloud when dash
        if (isFlipped) {
            cloudPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            dashCloud = Instantiate(dashSmokePrefab, cloudPos, Quaternion.Euler(45, 0, 0));
            dashCloud.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            cloudPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            dashCloud = Instantiate(dashSmokePrefab, cloudPos, Quaternion.Euler(45, 0, 0));
            dashCloud.transform.localScale = new Vector3(-1, 1, 1);
        }

        Destroy(dashCloud, 0.7f);
        yield return new WaitForSeconds(dashTime);
        canDash = true;
    }

    // Manages any physics e.g. movement by changing velocity
    private void FixedUpdate() {
        if (!isAttacking) {
            if (!isDashing) {
                if (Modifiers.instance == null) {
                    rb.velocity = dir * speed; 
                }
                else {
                    rb.velocity = dir * speed * Modifiers.instance.selectedModifier.playerSpeed; 
                }
            
            }
            else {
                rb.velocity = dashDir * dashSpeed;
            }
        }
        if (currenthealth <= 0 || isHurt) {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    // Perform basic movement that accounts for isometric camera
    private void Movement()
    {
        float offsetX = 0f;
        float offsetZ = 0f;
        inputV = Input.GetAxisRaw("Vertical");
        inputH = Input.GetAxisRaw("Horizontal");    
        // Normalized to avoid diagonal movement being faster
        dir = new Vector3(inputH + offsetX, 0.0f, inputV + offsetZ).normalized;

        if ((inputV != 0 || inputH != 0) && rb.velocity != new Vector3(0f, 0f, 0f)) {
            isMoving = true;
            anim.SetBool("running", true);
        }
        else {
            isMoving = false;
            anim.SetBool("running", false);
        }
    }

    // Perform basic melee attack
    private void MeleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking) {
            if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))) {
                isAttacking = true;
                FlipSprite.instance.flipToMouse();
                FindObjectOfType<SoundManager>().Play("Melee");
                anim.SetTrigger("hit1");
                speed = 0;
            }
            
            if (SpriteManager.instance.isTransitioning) {
                isAttacking = true;
                FlipSprite.instance.flipToMouse();
                FindObjectOfType<SoundManager>().Play("Melee");
            }
        }
    }

    public void damageOverTime() {
        currenthealth -= 1;
        healthBar.SetHealth(currenthealth);
    }

    public void takeDamage(int damage) {
        if (!isDashing && !isDead && !isHurt) {
            currenthealth -= (damage - defense);
            healthBar.SetHealth(currenthealth);
            FindObjectOfType<SoundManager>().Play("Player Hit");
            if (currenthealth - damage > 0) {
                // anim.SetTrigger("isDamaged");
            }
        }
    }

    public bool getIsMoving() {
        return isMoving;
    }

    public bool getIsDashing() {
        return isDashing;
    }

    public bool getIsAttacking() {
        return isAttacking;
    }

    public void setIsAttacking(bool isAttacking) {
        this.isAttacking = isAttacking;
    }
}
