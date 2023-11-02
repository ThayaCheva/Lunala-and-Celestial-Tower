using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEffects : MonoBehaviour
{
    [SerializeField] private float duration;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }
}
