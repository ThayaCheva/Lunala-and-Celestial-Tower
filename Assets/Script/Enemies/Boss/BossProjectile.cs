using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private GameObject sparkPrefab;
    [SerializeField] private float projectileDamage;
    // Update is called once per frame
    void Start() {
        Destroy(gameObject, 5.0f);
        int levels=FindObjectOfType<ManageScene>().levelCount;
        if(levels>1){
            projectileDamage+=(levels-1)*3;
        }
    }
    
    private void OnTriggerEnter(Collider collider){
        if(collider.GetComponent<Collider>().tag == "Wall"){
            GameObject newEffect = Instantiate(sparkPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(collider.GetComponent<Collider>().tag=="Player"){
            collider.SendMessage("takeDamage",projectileDamage * Modifiers.instance.selectedModifier.enemyAttack);
            GameObject newEffect = Instantiate(sparkPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
