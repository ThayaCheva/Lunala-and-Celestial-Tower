using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.parent.gameObject;
    }

    public void FireProjectileInAnimation() {
        if (enemy != null) {
            StartCoroutine(enemy.GetComponent<Enemy2Controller>().fireProjectile());
        }
    }
}
