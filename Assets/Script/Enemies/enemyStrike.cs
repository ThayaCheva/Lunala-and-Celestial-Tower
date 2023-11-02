using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyStrike : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float Damage;

   

     private void OnTriggerEnter(Collider collider){
        if(collider.GetComponent<Collider>().tag == "Player"){
            collider.SendMessage("takeDamage",Damage * Modifiers.instance.selectedModifier.enemyAttack);
        }
    }
}
