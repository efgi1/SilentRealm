using UnityEngine;

public class DissolveMaterials : MonoBehaviour
{
    private enum Action
    {
        Materialize,
        Dematerialize
    }

    private const float DISSOLVE_SPEED = 0.6f;
    
    private Material[] materials;
    private Action action = Action.Materialize;
    private float dissolveFraction = 1.0f;

    void Start()
    {
        materials = GetComponent<Renderer>().materials;
        SetDissolveAmount(dissolveFraction);
    }

    void Update()
    {
        float smoothing = DISSOLVE_SPEED * Time.deltaTime;
        if (action == Action.Dematerialize)
        {
            dissolveFraction = Mathf.Min(1.0f, dissolveFraction + smoothing);
        }
        else if (action == Action.Materialize)
        {
            dissolveFraction = Mathf.Max(0.0f, dissolveFraction - smoothing);
        }

        SetDissolveAmount(dissolveFraction);
    }

    private void SetDissolveAmount(float amount)
    {
        foreach (Material material in materials)
        {
            material.SetFloat("_DissolveAmount", amount);
        }
    }

    private void OnDissolvePlayer()
    {
        action = Action.Dematerialize;
    }

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.DISSOLVE_PLAYER, OnDissolvePlayer);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.DISSOLVE_PLAYER, OnDissolvePlayer);
    }
}
