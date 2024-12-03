using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class WizardController : MonoBehaviour
{ 
 // Start is called before the first frame update
    [SerializeField] public float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float lanceSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float healthReturn;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] AudioSource damagedSound;
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject shardPrefab;
    public UnityEngine.AI.NavMeshAgent navigation;

    [SerializeField] private float knockbackForce = 2.5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    //Constantly Changing Values 
    private Vector3 target;

    private float destinationTime;
    private float currHealth; 
    private float currDamage;

    //Booleans for managing behavior 
    public bool canDamage = true;
    private bool targetting=false;
    private bool takingDamage=false;
    private bool isRight;
    public bool isStun=false;
    public bool isDead = false;

    // Start is called before the first frame update
    private void Awake() {
        int levels = FindObjectOfType<ManageScene>().levelCount;
        if(levels > 1){
            maxHealth += (levels - 1) * 4;
        }
    }

    void Start()
    {
        navigation = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currHealth = maxHealth * Modifiers.instance.selectedModifier.enemyHealth;
        target= GenerateNavMeshPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            destinationTime += Time.deltaTime;
            if(!targetting && !isStun){
                if(Vector3.Distance(this.transform.position,LunalaController.instance.transform.position)<=range){
                    targetting=true;
                }
                //Allow the Enemy to roam for 5 seconds and then change destination or change if they reach the destination
                if(destinationTime>=10 ||Vector3.Distance(this.transform.position,target)<=range){
                    target= GenerateNavMeshPosition();
                    destinationTime=0;
                }
                if(navigation.isOnNavMesh){
                    navigation.SetDestination(target);
                }
                if (navigation.speed != 0) {
                    anim.SetBool("running", true);
                }
                else {
                    anim.SetBool("running", false);
                }

                if (target.x > transform.position.x) {
                    transform.localScale = new Vector3(1, 1, 1);
                    isRight = false;
                }
                else {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isRight = true;
                }
            } 
            //Enemy Behavior when the Player comes into range
            if(targetting && !isStun){
                navigation.speed=0;
                anim.SetBool("running", false);
                if (LunalaController.instance.transform.position.x > transform.position.x) {
                    transform.localScale = new Vector3(1, 1, 1);
                    isRight = false;
                }
                else {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isRight = true;
                }

                if(canDamage && !takingDamage){
                    canDamage=false;
                    FindObjectOfType<SoundManager>().Play("Enemy2 Attack");
                    anim.SetTrigger("attack");
                }

                if(Vector3.Distance(this.transform.position,LunalaController.instance.transform.position) > range){
                    targetting=false;
                    navigation.speed=1;
                    anim.SetBool("running",true);
                }       
            }
        }
        else {
            navigation.speed = 0;
        }
    }

    private void OnTriggerEnter(Collider collider){
        if(collider.GetComponent<Collider>().tag == "Area"){
            takeDamage(AbilitiesManager.instance.celestialDamage * AbilitiesManager.instance.damageMultiplier * Modifiers.instance.selectedModifier.playerAttack);
        }
        if(collider.GetComponent<Collider>().tag == "Projectile"){
            takeDamage(AbilitiesManager.instance.lanceDamage * AbilitiesManager.instance.damageMultiplier * Modifiers.instance.selectedModifier.playerAttack);
            target=LunalaController.instance.transform.position;
        }
        if(collider.GetComponent<Collider>().tag=="Wall"){
            target=GenerateNavMeshPosition();
        }
    }

    public void takeDamage(float damage)
    {
        takingDamage = true;

        if (!isDead)
        {
            // Apply damage to current health
            currHealth -= damage * AbilitiesManager.instance.damageMultiplier;

            // Display damage numbers
            ShowDamageNumbers(damage);

            // Play hit effects
            PlayHitEffects();

            // Apply knockback
            StartCoroutine(Knockback());

            // Check if health is below or equal to zero
            if (currHealth <= 0)
            {
                HandleDeath();
            }
            else
            {
                anim.SetTrigger("hurt");
            }
        }

        takingDamage = false;
    }

    public void takeMeleeDamage(float damage)
    {
        takingDamage = true;

        if (!isDead)
        {
            // Apply damage to current health
            currHealth -= damage * AbilitiesManager.instance.damageMultiplier;

            // Display damage numbers
            ShowDamageNumbers(damage);

            // Play hit effects
            PlayHitEffects();

            // Apply knockback
            StartCoroutine(KnockbackMelee());

            // Check if health is below or equal to zero
            if (currHealth <= 0)
            {
                HandleDeath();
            }
            else
            {
                anim.SetTrigger("hurt");
            }
        }

        takingDamage = false;
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
            Vector3 direction = transform.position - target;
            direction.y = 0; // Ignore vertical movement
            direction.Normalize();
            navigation.Move(direction * knockbackForce * Time.deltaTime);
            yield return null;
        }
        navigation.speed = originalSpeed; // Restore original speed
    }

    // Display damage numbers above the enemy
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

    // Handle death logic and effects
    private void HandleDeath()
    {
        isDead = true;

        anim.SetTrigger("isDead");
        FindObjectOfType<SoundManager>().Play("Enemy2 Death Sound");
        Instantiate(shardPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.identity);

        if (SceneManager.GetActiveScene().name != "Level3")
        {
            FindObjectOfType<WaveManager>().enemyDead();
            FindObjectOfType<WaveManager>().enemy2Dead();
        }

        Destroy(gameObject, 1.3f);
    }


    public IEnumerator fireProjectile(){
        canDamage=false;
        if(!isDead){
            Vector3 playerLocation = LunalaController.instance.transform.position;
            Vector3 targetDirection = playerLocation-transform.position;
            float targetZRotation = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(90f, 0, targetZRotation - (90f));
            GameObject enemyProjectile = Instantiate(projectile,transform.position,desiredRotation);
            Vector3 firePos= new Vector3(transform.position.x,0f,transform.position.z);
            Vector3 targetPos=new Vector3(playerLocation.x,0f,playerLocation.z);
            Vector3 shootDir=(playerLocation-firePos).normalized;

            //Vector3 shootDir= targetDirection.normalized;
            shootDir.y=0f;
            Rigidbody projectileRigidbody = enemyProjectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootDir * lanceSpeed;
            yield return new WaitForSeconds(attackCooldown);
        }
        canDamage=true;

    }

     private Vector3 GeneratePosition(){
        //NavMeshHit hit;
        float x = Random.Range(-30.0f,-10.0f);
        float y= Random.Range(-1.0f,1.0f);
        float z= Random.Range(-10.0f,10.0f);
        Vector3 position = new Vector3(x,y,z);
        return position;
    }

    public Vector3 GenerateNavMeshPosition(){
        NavMeshHit hit;
        Vector3 position=GeneratePosition();
        while(NavMesh.SamplePosition(position,out hit, Mathf.Infinity, NavMesh.AllAreas)== false){
            position=GeneratePosition();
        }
        Vector3 randomPosition= hit.position;
        return randomPosition;
    }

}
