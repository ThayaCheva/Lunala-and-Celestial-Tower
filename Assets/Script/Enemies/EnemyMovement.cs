using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyController enemyController;
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    public void allowRun() {
        enemyController.canDamage = true;
    }

    public void disableRun() {
        enemyController.canDamage = false;
        StartCoroutine(checkCanDamage());
    }

    IEnumerator checkCanDamage() {
        yield return new WaitForSeconds(1f); 
        if (!enemyController.canDamage)
        {
            enemyController.canDamage = true;
        }
    }
}
