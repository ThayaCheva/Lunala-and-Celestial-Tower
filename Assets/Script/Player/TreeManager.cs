 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreeManager : MonoBehaviour
{
    public Canvas skillMenu;
    public static TreeManager instance;
    public TextMeshProUGUI unlockCountText;
    public int unlockCount = 0;
    public int maxUnlock;
    public GameObject starParticles;

    private CursorManager cursorManager;

    [HideInInspector] public bool lanceUnlock1 = false;
    [HideInInspector] public float lanceUnlock1_level = 0;

    [HideInInspector] public bool lanceUnlock2 = false;
    [HideInInspector] public float lanceUnlock2_level = 0;
    
    [HideInInspector] public bool lanceUnlock3 = false;
    [HideInInspector] public float lanceUnlock3_level = 0;

    [HideInInspector] public bool lanceUnlock1_R = false;
    [HideInInspector] public float lanceUnlock1_R_level = 0;

    [HideInInspector] public bool lanceUnlock2_R = false;
    [HideInInspector] public float lanceUnlock2_R_level = 0;

    [HideInInspector] public bool lanceUnlock3_R = false;
    [HideInInspector] public float lanceUnlock3_R_level = 0;


    public int level1_cost;
    public int level2_cost;
    public int level3_cost;


    private Image img;

    // Update is called once per frame
    void Awake() {
        instance = this;
        cursorManager = GameObject.Find("Cursor Manager").GetComponent<CursorManager>();
        starParticles.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            skillMenu.enabled = !skillMenu.enabled;
            if (skillMenu.enabled) {
                Time.timeScale = 0;
                starParticles.SetActive(true);
                cursorManager.ResetCursor();
            }
            else {
                Time.timeScale = 1;
                starParticles.SetActive(false);
                cursorManager.SetCombatCursor();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            skillMenu.enabled = false;
            starParticles.SetActive(false);
            Time.timeScale = 1;
        }
        unlockCountText.text = unlockCount + "/" + maxUnlock + " Upgrades Unlocked";
    }

    // Lower Cooldown
    public void unlockLance1() {
        if (lanceUnlock1) {
            if (lanceUnlock1_level < 5) {   
                if (LunalaController.instance.points >= level1_cost) { 
                    LunalaController.instance.points -= level1_cost;
                    lanceUnlock1_level += 1;
                    AbilitiesManager.instance.projectileCooldown -= 0.2f;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (lanceUnlock1_level == 5) {
                    img = GameObject.Find("Tier 1 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (!lanceUnlock1) {
                if (LunalaController.instance.points >= level1_cost) {
                    LunalaController.instance.points -= level1_cost;
                    lanceUnlock1 = true;
                    lanceUnlock1_level += 1;
                    AbilitiesManager.instance.projectileCooldown -= 0.2f;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Tier 1 Left").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase speed
    public void unlockLance2() {
        if (lanceUnlock2) {
            if (lanceUnlock2_level < 5) {
                if (LunalaController.instance.points >= level2_cost) { 
                    LunalaController.instance.points -= level2_cost;
                    lanceUnlock2_level += 1;
                    AbilitiesManager.instance.lanceSpeed += 1;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (lanceUnlock2_level == 5) {
                    img = GameObject.Find("Tier 2 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (lanceUnlock1 && !lanceUnlock2) {
                if (LunalaController.instance.points >= level2_cost) {
                    LunalaController.instance.points -= level2_cost;
                    lanceUnlock2 = true;
                    lanceUnlock2_level += 1;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Tier 2 Left").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Divine Arrow
    public void unlockLance3() {
        if (unlockCount < maxUnlock) {
            if (lanceUnlock1 && lanceUnlock2 && !lanceUnlock3) {
                if (LunalaController.instance.points >= level3_cost) {
                    LunalaController.instance.points -= level3_cost;
                    lanceUnlock3 = true;
                    lanceUnlock3_level += 1;
                    img = GameObject.Find("Tier 3 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase size
    public void unlockLance1_R() {
        if (lanceUnlock1_R) {
            if (lanceUnlock1_R_level < 5) {
                if (LunalaController.instance.points >= level1_cost) { 
                    LunalaController.instance.points -= level1_cost;
                    lanceUnlock1_R_level += 1;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (lanceUnlock1_R_level == 5) {
                    img = GameObject.Find("Tier 1 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (!lanceUnlock1_R) {
                if (LunalaController.instance.points >= level1_cost) {
                    LunalaController.instance.points -= level1_cost;
                    lanceUnlock1_R = true;
                    lanceUnlock1_R_level += 1;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Tier 1 Right").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase Damage
    public void unlockLance2_R() {
        if (lanceUnlock2_R) {
            if (lanceUnlock2_R_level < 5) {
                if (LunalaController.instance.points >= level2_cost) { 
                    LunalaController.instance.points -= level2_cost;
                    lanceUnlock2_R_level += 1;
                    AbilitiesManager.instance.lanceDamage += 2;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (lanceUnlock2_R_level == 5) {
                    img = GameObject.Find("Tier 2 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (lanceUnlock1_R && !lanceUnlock2_R) {
                if (LunalaController.instance.points >= level2_cost) {
                    LunalaController.instance.points -= level2_cost;
                    lanceUnlock2_R = true;
                    lanceUnlock2_R_level += 1;
                    AbilitiesManager.instance.lanceDamage += 2;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Tier 2 Right").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Moonlight Bloom
    public void unlockLance3_R() {
        if (unlockCount < maxUnlock) {
            if (lanceUnlock1_R && lanceUnlock2_R && !lanceUnlock3_R) {
                if (LunalaController.instance.points >= level3_cost) {
                    LunalaController.instance.points -= level3_cost;
                    lanceUnlock3_R = true;
                    lanceUnlock3_R_level += 1;
                    img = GameObject.Find("Tier 3 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    unlockCount += 1;
                    
                }
            }
        }
    }

    [HideInInspector] public bool blessingUnlock1 = false;
    [HideInInspector] public float blessingUnlock1_level = 0;

    [HideInInspector] public bool blessingUnlock2 = false;
    [HideInInspector] public float blessingUnlock2_level = 0;
    
    [HideInInspector] public bool blessingUnlock3 = false;
    [HideInInspector] public float blessingUnlock3_level = 0;

    [HideInInspector] public bool blessingUnlock1_R = false;
    [HideInInspector] public float blessingUnlock1_R_level = 0;

    [HideInInspector] public bool blessingUnlock2_R = false;
    [HideInInspector] public float blessingUnlock2_R_level = 0;

    [HideInInspector] public bool blessingUnlock3_R = false;
    [HideInInspector] public float blessingUnlock3_R_level = 0;

    // Lower Cooldown
    public void unlockBlessing1() {
        if (blessingUnlock1) {
            if (blessingUnlock1_level < 5) {
                if (LunalaController.instance.points >= level1_cost) { 
                    LunalaController.instance.points -= level1_cost;
                    blessingUnlock1_level += 1;
                    AbilitiesManager.instance.angelCooldown -= blessingUnlock1_level / 5;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (blessingUnlock1_level == 5) {
                    img = GameObject.Find("Blessing Tier 1 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (!blessingUnlock1) {
                if (LunalaController.instance.points >= level1_cost) {
                    LunalaController.instance.points -= level1_cost;
                    blessingUnlock1 = true;
                    blessingUnlock1_level += 1;
                    AbilitiesManager.instance.angelCooldown -= blessingUnlock1_level / 5;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Blessing Tier 1 Left").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase speed
    public void unlockBlessing2() {
        if (blessingUnlock2) {
            if (blessingUnlock2_level < 5) {
                if (LunalaController.instance.points >= level2_cost) { 
                    LunalaController.instance.points -= level2_cost;
                    blessingUnlock2_level += 1; 
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (blessingUnlock2_level == 5) {
                    img = GameObject.Find("Blessing Tier 2 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (blessingUnlock1 && !blessingUnlock2) {
                if (LunalaController.instance.points >= level2_cost) {
                    LunalaController.instance.points -= level2_cost;
                    blessingUnlock2 = true;
                    blessingUnlock2_level += 1;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Blessing Tier 2 Left").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Divine Arrow
    public void unlockBlessing3() {
        if (unlockCount < maxUnlock) {
            if (blessingUnlock1 && blessingUnlock2 && !blessingUnlock3) {
                if (LunalaController.instance.points >= level3_cost) {
                    LunalaController.instance.points -= level3_cost;
                    blessingUnlock3 = true;
                    blessingUnlock3_level += 1;
                    img = GameObject.Find("Blessing Tier 3 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase size
    public void unlockBlessing1_R() {
        if (blessingUnlock1_R) {
            if (blessingUnlock1_R_level < 5) {
                if (LunalaController.instance.points >= level1_cost) { 
                    LunalaController.instance.points -= level1_cost;
                    blessingUnlock1_R_level += 1;
                    AbilitiesManager.instance.healRate += 0.002f;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (blessingUnlock1_R_level == 5) {
                    img = GameObject.Find("Blessing Tier 1 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (!blessingUnlock1_R) {
                if (LunalaController.instance.points >= level1_cost) {
                    LunalaController.instance.points -= level1_cost;
                    blessingUnlock1_R = true;
                    blessingUnlock1_R_level += 1;
                    AbilitiesManager.instance.healRate += 0.002f;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Blessing Tier 1 Right").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase Damage to skill 
    public void unlockBlessing2_R() {
        if (blessingUnlock2_R) {
            if (blessingUnlock2_R_level < 5) {
                if (LunalaController.instance.points >= level2_cost) { 
                    LunalaController.instance.points -= level2_cost;
                    blessingUnlock2_R_level += 1;
                    LunalaController.instance.defense += blessingUnlock2_R_level / 3;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (blessingUnlock2_R_level == 5) {
                    img = GameObject.Find("Blessing Tier 2 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (blessingUnlock1_R && !blessingUnlock2_R) {
                if (LunalaController.instance.points >= level2_cost) {
                    LunalaController.instance.points -= level2_cost;
                    blessingUnlock2_R = true;
                    blessingUnlock2_R_level += 1;
                    LunalaController.instance.defense += blessingUnlock2_R_level / 3;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Blessing Tier 2 Right").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                }
            }
        }
    }

    // Moonlight Bloom
    public void unlockBlessing3_R() {
        if (unlockCount < maxUnlock) {
            if (blessingUnlock1_R && blessingUnlock2_R && !blessingUnlock3_R) {
                if (LunalaController.instance.points >= level3_cost) {
                    LunalaController.instance.points -= level3_cost;
                    blessingUnlock3_R = true;
                    blessingUnlock3_R_level += 1;
                    img = GameObject.Find("Blessing Tier 3 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    unlockCount += 1;
                    
                }
            }
        }
    }

    [HideInInspector] public bool celestialUnlock1 = false;
    [HideInInspector] public float celestialUnlock1_level = 0;

    [HideInInspector] public bool celestialUnlock2 = false;
    [HideInInspector] public float celestialUnlock2_level = 0;
    
    [HideInInspector] public bool celestialUnlock3 = false;
    [HideInInspector] public float celestialUnlock3_level = 0;

    [HideInInspector] public bool celestialUnlock1_R = false;
    [HideInInspector] public float celestialUnlock1_R_level = 0;

    [HideInInspector] public bool celestialUnlock2_R = false;
    [HideInInspector] public float celestialUnlock2_R_level = 0;

    [HideInInspector] public bool celestialUnlock3_R = false;
    [HideInInspector] public float celestialUnlock3_R_level = 0;

    // Celestial Assault
    public void unlockCelestial1() {
        if (celestialUnlock1) {
            if (celestialUnlock1_level < 5) {
                if (LunalaController.instance.points >= level1_cost) { 
                    LunalaController.instance.points -= level1_cost;
                    celestialUnlock1_level += 1;
                    AbilitiesManager.instance.celestialDamage += 2; 
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (celestialUnlock1_level == 5) {
                    img = GameObject.Find("Celestial Tier 1 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (!celestialUnlock1) {
                if (LunalaController.instance.points >= level1_cost) {
                    LunalaController.instance.points -= level1_cost;
                    celestialUnlock1 = true;
                    celestialUnlock1_level += 1;
                    AbilitiesManager.instance.celestialDamage += 2; 
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Celestial Tier 1 Left").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Increase Size
    public void unlockCelestial2() {
        if (celestialUnlock2) {
            if (celestialUnlock2_level < 5) {
                if (LunalaController.instance.points >= level2_cost) { 
                    LunalaController.instance.points -= level2_cost;
                    celestialUnlock2_level += 1; 
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (celestialUnlock2_level == 5) {
                    img = GameObject.Find("Celestial Tier 2 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (celestialUnlock1 && !celestialUnlock2) {
                if (LunalaController.instance.points >= level2_cost) {
                    LunalaController.instance.points -= level2_cost;
                    celestialUnlock2 = true;
                    celestialUnlock2_level += 1;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Celestial Tier 2 Left").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Devastation
    public void unlockCelestial3() {
        if (unlockCount < maxUnlock) {
            if (celestialUnlock1 && celestialUnlock2 && !celestialUnlock3) {
                if (LunalaController.instance.points >= level3_cost) {
                    LunalaController.instance.points -= level3_cost;
                    celestialUnlock3 = true;
                    celestialUnlock3_level += 1;
                    AbilitiesManager.instance.celestialCooldown = 45;
                    AbilitiesManager.instance.celestialDamage *= 5;
                    img = GameObject.Find("Celestial Tier 3 Left").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Celestial Storm
    public void unlockCelestial1_R() {
        if (celestialUnlock1_R) {
            if (celestialUnlock1_R_level < 5) {
                if (LunalaController.instance.points >= level1_cost) { 
                    LunalaController.instance.points -= level1_cost;
                    celestialUnlock1_R_level += 1;
                    AbilitiesManager.instance.celestialCooldown -= 0.2f;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (celestialUnlock1_R_level == 5) {
                    img = GameObject.Find("Celestial Tier 1 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (!celestialUnlock1_R) {
                if (LunalaController.instance.points >= level1_cost) {
                    LunalaController.instance.points -= level1_cost;
                    celestialUnlock1_R = true;
                    celestialUnlock1_R_level += 1;
                    AbilitiesManager.instance.celestialCooldown -= 0.2f;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Celestial Tier 1 Right").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                    
                }
            }
        }
    }

    // Quad Strike
    public void unlockCelestial2_R() {
        if (celestialUnlock2_R) {
            if (celestialUnlock2_R_level < 4) {
                if (LunalaController.instance.points >= level2_cost) { 
                    LunalaController.instance.points -= level2_cost;
                    celestialUnlock2_R_level += 1;
                    FindObjectOfType<SoundManager>().Play("Pick Up");
                }
                if (celestialUnlock2_R_level == 4) {
                    img = GameObject.Find("Celestial Tier 2 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Max Upgrade");
                }
            }
        }
        if (unlockCount < maxUnlock) {
            if (celestialUnlock1_R && !celestialUnlock2_R) {
                if (LunalaController.instance.points >= level2_cost) {
                    LunalaController.instance.points -= level2_cost;
                    celestialUnlock2_R = true;
                    celestialUnlock2_R_level += 1;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    img = GameObject.Find("Celestial Tier 2 Right").GetComponent<Image>();
                    img.color = Color.white;
                    unlockCount += 1;
                }
            }
        }
    }

    // Carpet Bomb
    public void unlockCelestial3_R() {
        if (unlockCount < maxUnlock) {
            if (celestialUnlock1_R && celestialUnlock2_R && !celestialUnlock3_R) {
                if (LunalaController.instance.points >= level3_cost) {
                    LunalaController.instance.points -= level3_cost;
                    celestialUnlock3_R = true;
                    celestialUnlock3_R_level += 1;
                    img = GameObject.Find("Celestial Tier 3 Right").GetComponent<Image>();
                    img.color = Color.cyan;
                    FindObjectOfType<SoundManager>().Play("Upgrade Unlocked");
                    unlockCount += 1;
                    
                }
            }
        }
    }

}   