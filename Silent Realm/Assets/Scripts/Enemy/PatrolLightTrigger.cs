using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLightTrigger : MonoBehaviour
{
    public int rayDensity = 30;
    public float maxHeightVisibility = 10f;
    private LayerMask layerMask;
    private Vector3[] coneValues;

    void Start()
    {
        layerMask = LayerMask.GetMask("Player");
        layerMask |= LayerMask.GetMask("LightObstructor");
        coneValues = new Vector3[rayDensity];
        for (int i = 0; i < rayDensity; ++i)
        {
            coneValues[i].x = Mathf.Sin(Mathf.PI * 2f * (i / (float)rayDensity)) * 0.25f;
            coneValues[i].y = -0.5f;
            coneValues[i].z = Mathf.Cos(Mathf.PI * 2f * (i / (float)rayDensity)) * 0.25f;
        }
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < rayDensity; ++i)
        {
            Ray ray = new Ray(transform.position, coneValues[i]);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxHeightVisibility, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 0, true);

                if (hit.collider.CompareTag("Player"))
                {
                    GameManager.Instance.TimeRemaining = 0;
                    break;
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * maxHeightVisibility, Color.red, 0, true);
            }
        }
    }

}
