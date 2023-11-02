using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconCooldown : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider lance_slider;
    public Slider celestial_slider;
    public Slider moon_slider;
    private float moon_time;
    private float celestial_time;
    private float lance_time;
    void Update()
    {
        if (Time.timeScale != 0) {
            if (!LunalaController.instance.isDead) {
                if (Input.GetKeyDown(KeyCode.Q) && !AbilitiesManager.instance.angelOnCooldown) {
                    if (Modifiers.instance != null) {
                        moon_time = AbilitiesManager.instance.angelCooldown * 2  * Modifiers.instance.selectedModifier.playerCooldown;
                    }
                    else {
                        moon_time = AbilitiesManager.instance.angelCooldown * 2;
                    }
                    moon_slider.maxValue = 100; 
                    moon_slider.value = moon_time / AbilitiesManager.instance.angelCooldown * 100;
                }

                if (Input.GetKeyDown(KeyCode.E) && !AbilitiesManager.instance.celestialOnCooldown) {
                    if (Modifiers.instance != null) {
                        celestial_time = AbilitiesManager.instance.celestialCooldown * Modifiers.instance.selectedModifier.playerCooldown;
                    }
                    else {
                        celestial_time = AbilitiesManager.instance.celestialCooldown;
                    }
                    celestial_slider.maxValue = 100; 
                    celestial_slider.value = celestial_time / AbilitiesManager.instance.celestialCooldown * 100;
                }

                if (Input.GetMouseButtonDown(1) && !AbilitiesManager.instance.projectileOnCooldown) {
                    if (Modifiers.instance != null) {
                        lance_time = AbilitiesManager.instance.projectileCooldown * Modifiers.instance.selectedModifier.playerCooldown;
                    }
                    else {
                        lance_time = AbilitiesManager.instance.projectileCooldown;
                    }
                    lance_slider.maxValue = 100; 
                    lance_slider.value = lance_time / AbilitiesManager.instance.projectileCooldown * 100;
                }
            }
            if (moon_time > 0)
            {
                moon_time -= Time.deltaTime;
                float sliderValue = moon_time / AbilitiesManager.instance.angelCooldown * 100;
                moon_slider.value = sliderValue;
            }
            if (celestial_time > 0)
            {
                celestial_time -= Time.deltaTime;
                float sliderValue = celestial_time / AbilitiesManager.instance.celestialCooldown * 100;
                celestial_slider.value = sliderValue;
            }
            if (lance_time > 0)
            {
                lance_time -= Time.deltaTime;
                float sliderValue = lance_time / AbilitiesManager.instance.projectileCooldown  * 100;
                lance_slider.value = sliderValue;
            }
        }
    }
}
