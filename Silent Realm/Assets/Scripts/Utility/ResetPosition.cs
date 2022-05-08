using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
    }

    void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.ENEMY_ACTIVE_CHANGED, OnEnemyActiveChanged);
    }
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnEnemyActiveChanged(bool enemyActive)
    {
        if (!enemyActive)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            Quaternion rotation = Quaternion.Euler(new Vector3(90, 0, 0));

            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }
    }
}
