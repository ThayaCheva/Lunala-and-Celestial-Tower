using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroy[] objects = FindObjectsOfType<DontDestroy>();

        foreach (DontDestroy obj in objects)
        {
            Destroy(obj.gameObject);
        }
    }
}
