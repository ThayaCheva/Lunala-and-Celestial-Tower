using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] GameObject angelPrefab;
    [SerializeField] GameObject bloodMoonPrefab;
    public float damageMultiplier = 1;
    public float angelCooldown;
    public bool angelOnCooldown = false;
    private GameObject angelObject;
    public float healRate;

    [SerializeField] public GameObject celestialPrefab;
    [SerializeField] public float celestialCooldown;
    public bool celestialOnCooldown = false;
    private GameObject celestialObject;
    public float celestialDamage;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject lancePrefab;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] public float lanceSpeed;
    [SerializeField] public float projectileCooldown;
    [SerializeField] private Transform firePosition;
    public float lanceDamage;
    public bool projectileOnCooldown = false;

    public static AbilitiesManager instance;
    private float soundTimer = 0;
    void Awake() {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0) {
            if (soundTimer > 0) {
                soundTimer -= Time.deltaTime;
                if (soundTimer <= 0.0f)
                {
                    soundTimer = 0.0f;
                    FindObjectOfType<SoundManager>().Enable("Blessing Sound", false);
                }
            }
            if (!LunalaController.instance.isDead) {
                if (Input.GetKeyDown(KeyCode.Q) && !angelOnCooldown) {
                    StartCoroutine(CallAngel());
                    soundTimer = angelCooldown;
                }
                if (Input.GetKeyDown(KeyCode.E) && !celestialOnCooldown) {
                    if (TreeManager.instance.celestialUnlock2_R_level == 1) {
                        StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                    }
                    else if (TreeManager.instance.celestialUnlock2_R_level == 2) {
                        StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,1)));
                    }
                    else if (TreeManager.instance.celestialUnlock2_R_level == 3) {
                        StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,1)));
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,-1)));
                    }
                    else if (TreeManager.instance.celestialUnlock2_R_level == 4) {
                        StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,1)));
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,-1)));
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,0)));
                    }
                    else {
                        StartCoroutine(CallCelestialStrike(new Vector3(0,0,0)));
                    }

                    if (TreeManager.instance.celestialUnlock3_R) {
                        StartCoroutine(CarpetBomb());
                    }
                }
                if (Input.GetMouseButtonDown(1) && !projectileOnCooldown) {
                    if (TreeManager.instance.lanceUnlock3) {
                        StartCoroutine(CallLance(new Vector3(0,0,0), 0));
                        StartCoroutine(CallLance(new Vector3(0,0,0), 45));
                        StartCoroutine(CallLance(new Vector3(0,0,0), -45));
                    }
                    else {
                        StartCoroutine(CallLance(new Vector3(0,0,0), 0));
                    }
                }
            }
        }
    }

    private IEnumerator CarpetBomb()
    {
        for (int i = 0; i < 3; i++)
        {
            if (TreeManager.instance.celestialUnlock2_R_level == 1) {
                StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
            }
            else if (TreeManager.instance.celestialUnlock2_R_level == 2) {
                StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,1)));
            }
            else if (TreeManager.instance.celestialUnlock2_R_level == 3) {
                StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,1)));
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,-1)));
            }
            else if (TreeManager.instance.celestialUnlock2_R_level == 4) {
                StartCoroutine(CallCelestialStrike(new Vector3(1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(-1,0,0)));
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,1)));
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,-1)));
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,0)));
            }
            else {
                StartCoroutine(CallCelestialStrike(new Vector3(0,0,0)));
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator CallCelestialStrike(Vector3 offset) {
        celestialOnCooldown = true;
        // Instantiate Object, play sound and destroy when time is up
        celestialObject = Instantiate(celestialPrefab, new Vector3(transform.localPosition.x + offset.x, transform.localPosition.y + offset.y, transform.localPosition.z + offset.z), Quaternion.identity);
        if (TreeManager.instance.celestialUnlock2) {
            celestialObject.transform.localScale = new Vector3(1 + TreeManager.instance.celestialUnlock2_level/5, 1 + TreeManager.instance.celestialUnlock2_level/5, 1 + TreeManager.instance.celestialUnlock2_level/5);
        }
        if (TreeManager.instance.celestialUnlock3) {
            celestialObject.transform.localScale *= 3;
            FindObjectOfType<SoundManager>().Play("Divine Devastation");
        }
        else {
            FindObjectOfType<SoundManager>().Play("Celestial Strike");
        }
        Destroy(celestialObject, 1.3f);
        if (Modifiers.instance != null) {
            yield return new WaitForSeconds(celestialCooldown * Modifiers.instance.selectedModifier.playerCooldown);
        }
        else {
            yield return new WaitForSeconds(celestialCooldown);
        }
        celestialOnCooldown = false;
    }

    public IEnumerator CallAngel() {
        angelOnCooldown = true;
        FindObjectOfType<SoundManager>().Enable("Blessing Sound", true);
        // Instantiate Object, play sound and destroy when time is up
        GameObject angelObject;
        if (TreeManager.instance.blessingUnlock3_R) {
            angelObject = Instantiate(bloodMoonPrefab, transform.localPosition, Quaternion.identity);
            FindObjectOfType<SoundManager>().Play("Blood Moon");
            StartCoroutine(resetDamage());
        }
        else {
            angelObject = Instantiate(angelPrefab, transform.localPosition, Quaternion.identity);
        }
        if (TreeManager.instance.blessingUnlock2_level > 0) {
            Transform ring = angelObject.transform.Find("Ring");
            ring.localScale = new Vector3(7 + TreeManager.instance.blessingUnlock2_level, 7 + TreeManager.instance.blessingUnlock2_level, 7 + TreeManager.instance.blessingUnlock2_level);
        }
        angelObject.GetComponent<AngelOfDeath>().abilityRadius = TreeManager.instance.blessingUnlock2_level / 5;
        FindObjectOfType<SoundManager>().Play("Angel Of Death");
        StartCoroutine(AngelOfDeath.instance.DestroyAngel(10f));
        Destroy(angelObject, 10f);
        if (Modifiers.instance != null) {
            yield return new WaitForSeconds(angelCooldown * 2 * Modifiers.instance.selectedModifier.playerCooldown);
        }
        else {
            yield return new WaitForSeconds(angelCooldown * 2);
        }
        angelOnCooldown = false;
    }

    public IEnumerator resetDamage() {
        damageMultiplier = 1.5f;
        yield return new WaitForSeconds(10f);
        damageMultiplier = 1;
    }

    private IEnumerator CallLance(Vector3 offset, float angleOffset) {
        projectileOnCooldown = true;
        Vector3 crossPos = new Vector3(crosshair.transform.position.x, 0f, crosshair.transform.position.z);

        // For fan lance upgrade
        if (TreeManager.instance.lanceUnlock3_R) {
            Vector3 targetDirection = crossPos - transform.position;
            float targetZRotation = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            for (int i = -1; i < 2; i++) {
                // Calculate the fan angle
                float fanAngle = i * 15f; 
                // Calculate the direction to shoot at
                Vector3 firePos = new Vector3(firePosition.position.x, 0f, firePosition.position.z);
                Vector3 shootDir = (crossPos - firePos).normalized;
                Quaternion desiredRotation;
            
                if (i == -1) {
                    desiredRotation = Quaternion.Euler(90f, 0, targetZRotation - (75f));
                }
                else if (i == 1) {
                    desiredRotation = Quaternion.Euler(90f, 0, targetZRotation - (105f));
                }
                else {
                    desiredRotation = Quaternion.Euler(90f, 0, targetZRotation - (90f  + angleOffset));
                }

                // Fan projectile if not center
                if (i != 0) {
                    shootDir = Quaternion.Euler(0, fanAngle, 0) * shootDir;
                }
                // Make y 0 so projectile don't move up
                shootDir.y = 0;

                // Instantiate a new lance with the calculated direction
                GameObject newLance = Instantiate(lancePrefab, firePosition.position, desiredRotation);
                if (TreeManager.instance.lanceUnlock1_R) {
                    newLance.transform.localScale = new Vector3(1 + TreeManager.instance.lanceUnlock1_R_level/5, 1 + TreeManager.instance.lanceUnlock1_R_level/5 ,1 + TreeManager.instance.lanceUnlock1_R_level/5);
                }
                FindObjectOfType<SoundManager>().Play("Lunar Lance");
                
                // Move the projectile
                Rigidbody projectileRigidbody = newLance.GetComponent<Rigidbody>();
                projectileRigidbody.velocity = shootDir * lanceSpeed;
            }
        }
        else {
            // Calculate rotation of projectile
            Vector3 targetDirection = crosshair.transform.position - transform.position;
            float targetZRotation = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion desiredRotation = Quaternion.Euler(90f, 0, targetZRotation - (90f + angleOffset));

            GameObject newLance = Instantiate(lancePrefab, firePosition.position, desiredRotation);
            if (TreeManager.instance.lanceUnlock1_R) {
                newLance.transform.localScale = new Vector3(1 + TreeManager.instance.lanceUnlock1_R_level/5,1 + TreeManager.instance.lanceUnlock1_R_level/5 ,1 + TreeManager.instance.lanceUnlock1_R_level/5);
            }
            FindObjectOfType<SoundManager>().Play("Lunar Lance");
            // Calculate direction to shoot at
            Vector3 firePos = new Vector3(firePosition.position.x, 0f, firePosition.position.z);
            Vector3 shootDir = (crossPos - firePos).normalized + offset;
            // Move the projectile
            Rigidbody projectileRigidbody = newLance.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootDir * lanceSpeed;
        }
        if (Modifiers.instance != null) {
            yield return new WaitForSeconds(projectileCooldown * Modifiers.instance.selectedModifier.playerCooldown);
        }
        else {
            yield return new WaitForSeconds(projectileCooldown);
        }
        projectileOnCooldown = false;
    }


}
