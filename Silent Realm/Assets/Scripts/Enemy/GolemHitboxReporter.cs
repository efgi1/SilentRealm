using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHitboxReporter : MonoBehaviour
{
    [SerializeField] GameObject golem;

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            var controller = golem.GetComponent<GolemEnemyController>();
            if (controller != null) {
                Vector3 contactPoint = other.ClosestPoint(golem.transform.position);
                controller.HandlePlayerHit(contactPoint);
            } else {
                Debug.Log("No EnemyController to report to.");
            }
        }
    }
}
