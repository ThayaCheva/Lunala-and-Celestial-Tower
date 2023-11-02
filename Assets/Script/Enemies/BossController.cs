using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update
     [SerializeField] public float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float lanceSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float strikeCooldown;
    [SerializeField] private float moonStrikeRange;
    [SerializeField] private float healthReturn;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject moonStrike;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] AudioSource damagedSound;
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject shardPrefab;
    public UnityEngine.AI.NavMeshAgent navigation;

    public static BossController instance;
    private GameObject ring;
    
    //private GameObject target;

    //Constantly Changing Values 
    private Vector3 target;

    private float currHealth; 
    private float currDamage;

    private GameObject strike;
    private bool strikeOnCooldown=false;

    //Booleans for managing behavior 
    public bool canDamage = true;
    private bool targetting=false;
    private bool takingDamage=false;
    private bool isRight;
    public bool isStun=false;
    public bool isDead = false;
    private bool gameOver = false;

     private void Awake() {
        instance = this;

    }
    void Start()
    {
        navigation = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (TreeManager.instance.unlockCount > 0) {
            currHealth = maxHealth * TreeManager.instance.unlockCount; 
        }
        else {
            currHealth = maxHealth; 
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (ring != null) {
            ring.transform.position = this.transform.position;
        }
        if (Timer.instance.currentTime == 0 && !gameOver) {
            gameOver = true;
            strike = Instantiate(moonStrike, new Vector3(LunalaController.instance.transform.position.x, LunalaController.instance.transform.position.y, LunalaController.instance.transform.position.z), Quaternion.identity);
            FindObjectOfType<SoundManager>().Play("Celestial Strike");
            Destroy(strike, 1.3f);
        }
        if (!isDead) {
                target=LunalaController.instance.transform.position;

                if(navigation.isOnNavMesh){
                    navigation.SetDestination(target);
                }

                if (target.x > transform.position.x) {
                    transform.localScale = new Vector3(1, 1, 1);
                    isRight = false;
                }
                else {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isRight = true;
                }
                if(Vector3.Distance(this.transform.position,LunalaController.instance.transform.position) <= moonStrikeRange && !strikeOnCooldown && !takingDamage){
                    StartCoroutine(bloodMoonStrike());
                }
                else if(canDamage && !takingDamage){
                    FindObjectOfType<SoundManager>().Play("Enemy2 Attack");
                    StartCoroutine(fireProjectile());
                }
   
        }    
    }

    private void OnTriggerEnter(Collider collider){
        if(collider.GetComponent<Collider>().tag == "Area"){
            takeDamage(AbilitiesManager.instance.celestialDamage * AbilitiesManager.instance.damageMultiplier * Modifiers.instance.selectedModifier.playerAttack);
        }
        if(collider.GetComponent<Collider>().tag == "Projectile"){
            takeDamage(AbilitiesManager.instance.lanceDamage * AbilitiesManager.instance.damageMultiplier * Modifiers.instance.selectedModifier.playerAttack);
        }
    }

    public void takeDamage(float damage){
        Vector3 LunalaPosition= LunalaController.instance.transform.position;
        takingDamage=true;
        if (!isDead) {  
            currHealth -= damage * AbilitiesManager.instance.damageMultiplier;

            var damageNumbers=Instantiate(damagePopup,new Vector3(transform.position.x, 1.5f, transform.position.z),Quaternion.identity);
            damageIndicator indicator= damageNumbers.GetComponent<damageIndicator>();
            indicator.indicateDamage(damage*AbilitiesManager.instance.damageMultiplier);

            FindObjectOfType<SoundManager>().Play("Enemy Hit");
            GameObject hitEffect = Instantiate(hitPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.identity);
            if (!isRight) {
                hitEffect.transform.localScale = new Vector3(-2, 2, 2);
            }
            Destroy(hitEffect, 0.5f);

            if(currHealth <= 0){
                isDead = true;
                // if(LunalaController.instance.currenthealth + maxHealth > LunalaController.instance.maxHealth){
                //     LunalaController.instance.currenthealth = LunalaController.instance.maxHealth;
                // }
                // else{
                //     LunalaController.instance.currenthealth += healthReturn;
                // }
                FindObjectOfType<SoundManager>().Play("Boss Death");
                Instantiate(shardPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.identity);
                gameObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(ClearGame());
            }
            else{
                anim.SetTrigger("hurt");
            }
        }
        takingDamage=false;
    }

    public IEnumerator ClearGame(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(2);
    }

    public IEnumerator bloodMoonStrike(){
        //navigation.speed=0f;
        strikeOnCooldown = true;
        canDamage=false;
        ring = Instantiate(ringPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(90, 0, 90));
        FindObjectOfType<SoundManager>().Play("Angel Of Death");
        Destroy(ring, 1.5f);
        StartCoroutine(deployStrike());
        yield return new WaitForSeconds(strikeCooldown);
        strikeOnCooldown=false;
        canDamage=true;
    }

    private IEnumerator deployStrike(){
        yield return new WaitForSeconds(1.5f);
        strike = Instantiate(moonStrike, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        FindObjectOfType<SoundManager>().Play("Celestial Strike");
        Destroy(strike, 1.3f);
    }

    public IEnumerator fireProjectile(){
        canDamage=false;
        //navigation.speed=0f;
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
        //navigation.speed=2.5f;

    }
}
