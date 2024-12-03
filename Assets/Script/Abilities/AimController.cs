using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Transform firePosition;

    [SerializeField] private Vector3 offset;
    public Vector3 hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAim();
    }

    private void HandleAim() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxRayLength = 100.0f;
        if (Physics.Raycast(ray, out hit, maxRayLength, groundMask))
        {
            Vector3 hitPoint = hit.point;
            crosshair.transform.position = new Vector3(hitPoint.x + offset.x, 0.7f, hitPoint.z + offset.z);
        }
    }
}
