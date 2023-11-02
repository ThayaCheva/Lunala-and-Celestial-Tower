using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarLance : MonoBehaviour
{
    [SerializeField] private GameObject sparkPrefab;
    // Update is called once per frame
    void Start() {
        GameObject newEffect = Instantiate(sparkPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 5.0f);
    }
    
    private void OnTriggerEnter(Collider collider){
        if(collider.GetComponent<Collider>().tag == "Wall"){
            GameObject newEffect = Instantiate(sparkPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
