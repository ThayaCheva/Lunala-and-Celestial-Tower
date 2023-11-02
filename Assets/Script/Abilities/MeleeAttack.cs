using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    // Altered code from https://www.youtube.com/watch?v=8rTK68omQow&t=5696s;

    public int meleeDamage = 10;
    [SerializeField] private Collider meleeHitBox;
    public Vector3 right = new Vector3(0.15f, 0f, 0f);
    public Vector3 left = new Vector3(-0.15f, 0f, 0f);

    void OnTriggerEnter(Collider col) {
        //EnemyController enemyController = col.gameObject.GetComponent<EnemyController>();
        if (col.gameObject.tag=="Enemy") {
            col.SendMessage("takeDamage", meleeDamage * Modifiers.instance.selectedModifier.playerAttack);
        }
    }

    void setDirectionRight(bool isFacingRight) {
        if (isFacingRight) {
            gameObject.transform.localPosition = right;
        }
        else {
            gameObject.transform.localPosition = left;
        }
    }

}
