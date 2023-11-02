using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialStrike : MonoBehaviour
{
    [SerializeField] GameObject celestialPrefab;
    [SerializeField] float celestialCooldown;
    public bool celestialOnCooldown = false;
    private GameObject celestialObject;
    public static CelestialStrike instance;

    private void Awake() {
        instance = this;
    }
}
