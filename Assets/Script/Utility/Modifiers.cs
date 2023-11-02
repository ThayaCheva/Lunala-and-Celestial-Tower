using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Code with assistant from ChatGPT under the prompt (unity write me list that stores a name and stats)
[Serializable]
public class Modifier {
    public string name;
    public string stats;
    public float enemyHealth = 1;
    public float enemyAttack = 1;
    public float playerCooldown = 1;
    public float playerAttack = 1;
    public float playerSpeed = 1;
    public float pointBonus = 1;

    public Modifier(string name, string stats, float enemyHealth, float enemyAttack, float playerCooldown, float playerAttack, float playerSpeed, float pointBonus)
    {
        this.name = name;
        this.stats = stats;
        this.enemyHealth = enemyHealth;
        this.enemyAttack = enemyAttack;
        this.playerCooldown = playerCooldown;
        this.playerAttack = playerAttack;
        this.playerSpeed = playerSpeed;
        this.pointBonus = pointBonus;
    }
}

// Code with assistant from ChatGPT
public class Modifiers : MonoBehaviour
{
    public static Modifiers instance;
    public List<Modifier> modifiers = new List<Modifier>();
    public Modifier selectedModifier;
    public TextMeshProUGUI modiferText;
    public TextMeshProUGUI HUDmodiferText;
    private float timer = 0f;
    private float interval = 30f;

    void Awake() {
        instance = this;
    }

    void Update() {
        if (SceneManager.GetActiveScene().name == "Level3") {
            timer += Time.deltaTime;

            if (timer >= interval) {
                modiferText.gameObject.SetActive(false);
                timer = 0f;
                int randomIndex = UnityEngine.Random.Range(0, modifiers.Count);
                selectedModifier = modifiers[randomIndex];
                modiferText.text = selectedModifier.name + ": " + selectedModifier.stats;
                if (HUDmodiferText != null) {
                    HUDmodiferText.text = selectedModifier.name + ": " + selectedModifier.stats;
                }
                modiferText.gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Level3") {
            modifiers.Add(new Modifier("Full Moon", "Enemies: +30% Attack, +30% Health", 1.3f, 1.3f, 1f, 1f, 1f, 1f));
            modifiers.Add(new Modifier("New Moon", "Lunala: +25% Attack, +20% Speed",  1f, 1f, 1f, 1.25f, 1.2f, 1f));
            modifiers.Add(new Modifier("Crescent Moon", "Lunala: -35% Cooldown, Enemy: +20% Attack", 1.2f, 0.8f, 0.65f, 1f, 1f, 1f));
            modifiers.Add(new Modifier("Blue Moon", "Enemies: +50% Health, Moon Shard: 2x Points",  1.5f, 1f, 1f, 1f, 1f, 2f));
        }
        else {
            modifiers.Add(new Modifier("Full Moon", "Enemies: +30% Attack, +30% Health", 1.3f, 1.3f, 1f, 1f, 1f, 1f));
            modifiers.Add(new Modifier("New Moon", "Lunala: +25% Attack, +20% Speed",  1f, 1f, 1f, 1.25f, 1.2f, 1f));
            modifiers.Add(new Modifier("Crescent Moon", "Lunala: -35% Cooldown, Enemy: +20% Attack", 1.2f, 0.8f, 0.65f, 1f, 1f, 1f));
        }
        int randomIndex = UnityEngine.Random.Range(0, modifiers.Count);
        selectedModifier = modifiers[randomIndex];
        modiferText.text = selectedModifier.name + ": " + selectedModifier.stats;
        if (HUDmodiferText != null) {
            HUDmodiferText.text = selectedModifier.name + ": " + selectedModifier.stats;
        }
    }
}
