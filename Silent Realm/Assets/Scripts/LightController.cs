using UnityEngine;

public class LightController : MonoBehaviour
{
    Light mLight;
    Color originalColor;
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
        mLight = GetComponent<Light>();
        originalColor = mLight.color;
    }

    void Update()
    {
    }

    private void OnEnemyActiveChanged(bool enemyActive)
    {
        mLight.color = enemyActive ? Color.black : originalColor;
    }
}
