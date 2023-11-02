using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    // Altered code from https://www.youtube.com/watch?v=8rTK68omQow&t=5696s;

    public int meleeDamage = 10;
    [SerializeField] private Collider meleeHitBox;
    public Vector3 right = new Vector3(0.15f, 0f, 0f);
    public Vector3 left = new Vector3(-0.15f, 0f, 0f);

    void Start() {
        int levels=FindObjectOfType<ManageScene>().levelCount;
        if(levels > 1){
            meleeDamage += (levels-1) * 3;
            Debug.Log(meleeDamage);
        }
    }

    void OnTriggerEnter(Collider col) {
        LunalaController lunalaController = col.gameObject.GetComponent<LunalaController>();
        if (lunalaController != null && !lunalaController.isDead) {
            col.SendMessage("takeDamage", meleeDamage * Modifiers.instance.selectedModifier.enemyAttack);
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
