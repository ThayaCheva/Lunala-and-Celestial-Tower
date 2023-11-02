using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelOfDeath : MonoBehaviour
{
    [SerializeField] public Animator anim;
    private Transform firePoint;
    private float nextFireTime = 0f;
    public float abilityRadius;
    public float fireRate;
    public GameObject attackPrefab1;
    public GameObject attackPrefab2;
    public CapsuleCollider capsuleCollider;
    public Transform target;

    public static AngelOfDeath instance;

    private void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (LunalaController.instance.currenthealth <= 100) {
            LunalaController.instance.currenthealth += AbilitiesManager.instance.healRate;
            LunalaController.instance.healthBar.SetHealth(LunalaController.instance.currenthealth);;
        }

        firePoint = LunalaController.instance.transform;
        transform.position = new Vector3(firePoint.transform.localPosition.x, firePoint.transform.localPosition.y - 0.15f, firePoint.transform.localPosition.z - 0.2f);
        if (TreeManager.instance.blessingUnlock3) {
            if (Time.time >= nextFireTime) {
                FindEnemies();
                if (target != null) {
                    HandleShoot(target);
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
    }

    // Adapted from https://www.youtube.com/watch?v=QKhn2kl9_8I
    private void FindEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestEnemyDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestEnemyDistance) {
                closestEnemyDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && closestEnemyDistance <= abilityRadius) {
            target = nearestEnemy.transform;
        }
        if (closestEnemyDistance >= abilityRadius) {
            target = null;
        }
    }

    private void HandleShoot(Transform target)
    {
        int num = Random.Range(0, 2);
        if (num == 0) {
            GameObject newProjectile = Instantiate(attackPrefab1, target.position, attackPrefab1.transform.rotation);
            FindObjectOfType<SoundManager>().Play("Angel Of Death");
            Destroy(newProjectile, 0.5f);
        }
        else {
            GameObject newProjectile = Instantiate(attackPrefab2, target.position, attackPrefab2.transform.rotation);
            FindObjectOfType<SoundManager>().Play("Angel Of Death");
            Destroy(newProjectile, 0.5f);
        }
    }

    public IEnumerator DestroyAngel(float angelCooldown) {
        yield return new WaitForSeconds(angelCooldown - 0.5f);
        if (this.gameObject != null) {
            anim.SetTrigger("destroy");
        }
    }
}
