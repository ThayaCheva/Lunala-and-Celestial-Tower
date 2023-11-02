using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float maxHealth;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float healthReturn;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] AudioSource damagedSound;
    public bool isStun = false;
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject shardPrefab;
    public NavMeshAgent navigation;
    public bool isDead = false;

    public static EnemyController instance;

    private GameObject target;
    private float currHealth; 
    private float currDamage;
    public bool canDamage = true;

    private bool takingDamage=false;
    private bool isRight;


    private void Awake() {
        instance = this;
        int levels=FindObjectOfType<ManageScene>().levelCount;
        if(levels>1){
            maxHealth+=(levels-1)*4;
        }
    }

    void Start()
    {
        navigation = GetComponent<NavMeshAgent>();
        currHealth = maxHealth * Modifiers.instance.selectedModifier.enemyHealth;

    }

    // Update is called once per frame
    void Update()
    {   
        if (!isDead) {
            if(!isStun && canDamage){
                target = GameObject.FindWithTag("Player");
                if(navigation.isOnNavMesh){
                    navigation.SetDestination(target.transform.position);
                }
                if (navigation.speed != 0) {
                    anim.SetBool("running", true);
                }
                else {
                    anim.SetBool("running", false);
                }

                if (target.transform.position.x > transform.position.x) {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isRight = false;
                }
                else {
                    transform.localScale = new Vector3(1, 1, 1);
                    isRight = true;
                }
            } 
        }
        else {
            navigation.speed = 0;
        }
    }

    private void OnTriggerStay(Collider collider) {
        if(!takingDamage){
            if((collider.GetComponent<Collider>().tag=="Player") && canDamage){
                StartCoroutine(DealDamage(attackCooldown));
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

    public IEnumerator DealDamage(float attackCooldown) {
        canDamage = false;
        if (!isDead) {
            anim.SetTrigger("attack");
            FindObjectOfType<SoundManager>().Play("Enemy Melee");
            anim.SetBool("running", false);
            yield return new WaitForSeconds(attackCooldown);
        }
        canDamage = true;
    }

    public void takeDamage(float damage){
        takingDamage=true;
        if (!isDead) {

            currHealth -= damage * AbilitiesManager.instance.damageMultiplier;

            //Vector3 position= new Vector3(transform.localPosition.x, transform.localPosition.y,transform.localPosition.z);
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
                anim.SetTrigger("isDead");
                FindObjectOfType<SoundManager>().Play("Enemy Death Sound");
                Instantiate(shardPrefab, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.identity);
                if (SceneManager.GetActiveScene().name != "Level3") {
                    FindObjectOfType<WaveManager>().enemyDead(); 
                }
                Destroy(gameObject, 1.0f);  
            }
            else {

                anim.SetTrigger("hurt");
            }
        }
        takingDamage=false;
    }
}
