using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHitboxReporter : MonoBehaviour
{
    [SerializeField] GameObject ghost;

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            var controller = ghost.GetComponent<GhostEnemyController>();
            if (controller != null) {
                controller.HandlePlayerHit(other.ClosestPoint(ghost.transform.position));
            } else {
                Debug.Log("No EnemyController to report to.");
            }
        }
    }
}
