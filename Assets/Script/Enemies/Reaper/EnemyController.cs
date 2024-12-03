using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private float maxHealth;
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private AudioSource damagedSound;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float knockbackForce = 2.5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    // Public variables
    public bool isStun = false;
    public Animator anim;
    public NavMeshAgent navigation;
    public bool isDead = false;

    // Private variables
    private GameObject target;
    private float currHealth;
    private bool canDamage = true;
    private bool takingDamage = false;
    private bool isRight;

    void Start()
    {
        navigation = GetComponent<NavMeshAgent>();
        currHealth = maxHealth; // Initialize current health
    }

    void Update()
    {
        if (!isDead)
        {
            // Handle movement and animation
            HandleMovementAndAnimation();
        }
        else
        {
            navigation.speed = 0; // Stop navigation when dead
        }
    }

    // Handle enemy movement towards player and animation
    private void HandleMovementAndAnimation()
    {
        if (!isStun && canDamage)
        {
            // Find player and set destination
            SetPlayerAsTarget();
            
            // Check if moving and flip sprite accordingly
            CheckMovementAndFlipSprite();
        }
    }

    // Find and set player as target for navigation
    private void SetPlayerAsTarget()
    {
        target = GameObject.FindWithTag("Player");
        if (navigation.isOnNavMesh)
        {
            navigation.SetDestination(target.transform.position);
        }
    }

    // Check movement and flip sprite based on player position
    private void CheckMovementAndFlipSprite()
    {
        anim.SetBool("running", navigation.velocity.magnitude > 0.1f); // Check if moving

        if (target.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip left
            isRight = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // Flip right
            isRight = true;
        }
    }

    // Deal damage to player on collision
    private void OnTriggerStay(Collider collider)
    {
        if (!takingDamage && collider.CompareTag("Player") && canDamage)
        {
            StartCoroutine(DealDamage(attackCooldown));
        }
    }

    // Handle damage from celestial area and projectile
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Area"))
        {
            takeDamage(AbilitiesManager.instance.celestialDamage * AbilitiesManager.instance.damageMultiplier * Modifiers.instance.selectedModifier.playerAttack);
        }
        if (collider.CompareTag("Projectile"))
        {
            takeDamage(AbilitiesManager.instance.lanceDamage * AbilitiesManager.instance.damageMultiplier * Modifiers.instance.selectedModifier.playerAttack);
        }
    }

    // Coroutine to handle attack cooldown
    public IEnumerator DealDamage(float attackCooldown)
    {
        canDamage = false;
        if (!isDead)
        {
            anim.SetTrigger("attack");
            FindObjectOfType<SoundManager>().Play("Enemy Melee");
            anim.SetBool("running", false);
            yield return new WaitForSeconds(attackCooldown);
        }
        canDamage = true;
    }

    // Take damage and handle effects
    public void takeDamage(float damage)
    {
        if (!isDead)
        takingDamage = true;
        {
            // Reduce health
            currHealth -= damage * AbilitiesManager.instance.damageMultiplier;

            // Instantiate damage numbers
            ShowDamageNumbers(damage);

            // Play hit sound and instantiate hit effect
            PlayHitEffects();

            // Apply knockback
            StartCoroutine(Knockback());

            // Check if dead
            CheckIfDead();
        }
        takingDamage = false;
    }

    // Take damage and handle effects
    public void takeMeleeDamage(float damage)
    {
        takingDamage = true;
        
        if (!isDead)
        {
            // Reduce health
            currHealth -= damage * AbilitiesManager.instance.damageMultiplier;

            // Instantiate damage numbers
            ShowDamageNumbers(damage);

            // Play hit sound and instantiate hit effect
            PlayHitEffects();

            // Apply knockback
            StartCoroutine(KnockbackMelee());

            // Check if dead
            CheckIfDead();
        }
        takingDamage = false;
    }

    // Show damage numbers above enemy
    private void ShowDamageNumbers(float damage)
    {
        var damageNumbers = Instantiate(damagePopup, new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity);
        DamageIndicator indicator = damageNumbers.GetComponent<DamageIndicator>();
        indicator.indicateDamage(damage * AbilitiesManager.instance.damageMultiplier);
    }

    // Play hit sound and instantiate hit effect
    private void PlayHitEffects()
    {
        FindObjectOfType<SoundManager>().Play("Enemy Hit");
        GameObject hitEffect = Instantiate(hitPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.identity);
        if (!isRight)
        {
            hitEffect.transform.localScale = new Vector3(-2, 2, 2);
        }
        Destroy(hitEffect, 0.5f);
    }

    // Coroutine to apply knockback effect
    private IEnumerator KnockbackMelee()
    {
        float timer = 0;
        float originalSpeed = navigation.speed;
        navigation.speed = 0; // Stop navigation during knockback

        // Determine knockback direction based on player's facing direction
        Vector3 direction;
        if (LunalaController.instance.transform.localScale.x < 0)
        {
            // Player is facing right, knockback left
            direction = Vector3.left;
        }
        else
        {
            // Player is facing left, knockback right
            direction = Vector3.right;
        }

        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;
            navigation.Move(direction * knockbackForce * Time.deltaTime);
            yield return null;
        }
        navigation.speed = originalSpeed; // Restore original speed
    }

    private IEnumerator Knockback()
    {
        float timer = 0;
        float originalSpeed = navigation.speed;
        navigation.speed = 0; // Stop navigation during knockback
        while (timer < knockbackDuration)
        {
            timer += Time.deltaTime;
            Vector3 direction = transform.position - target.transform.position;
            direction.y = 0; // Ignore vertical movement
            direction.Normalize();
            navigation.Move(direction * knockbackForce * Time.deltaTime);
            yield return null;
        }
        navigation.speed = originalSpeed; // Restore original speed
    }

    // Check if enemy is dead and handle death effects
    private void CheckIfDead()
    {
        if (currHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("isDead");
            FindObjectOfType<SoundManager>().Play("Enemy Death Sound");
            Instantiate(shardPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.identity);
            if (SceneManager.GetActiveScene().name != "Level3")
            {
                FindObjectOfType<WaveManager>().enemyDead();
            }
            Destroy(gameObject, 1.0f); // Destroy enemy object
        }
        else
        {
            anim.SetTrigger("hurt");
        }
    }
}
