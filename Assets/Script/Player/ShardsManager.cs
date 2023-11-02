using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void MoveToPlayer() {
        Vector3 dir = (LunalaController.instance.transform.position - transform.position).normalized;
        transform.position += dir * speed; 
    }

    private void OnTriggerEnter(Collider collider) {
        LunalaController lunalaController = collider.GetComponent<LunalaController>();
        if (collider.GetComponent<Collider>().tag == "Player") {
            int levels=FindObjectOfType<ManageScene>().levelCount;
            LunalaController.instance.points += levels * 5 * Modifiers.instance.selectedModifier.pointBonus;
            FindObjectOfType<SoundManager>().Play("Pick Up");
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.GetComponent<Collider>().tag == "ShardCollider") {
            MoveToPlayer();
        }
    }
}
