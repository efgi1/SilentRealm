using UnityEngine;
using UnityEngine.UI;

public class TitleTextChanger : MonoBehaviour
{
    [SerializeField] float colorChangeRate = 2.5f;
    private Text text;
    private float red, blue, green;
    private float timeSinceStart = 0;

    void Awake()
    {
        text = GetComponent<Text>();
        red = text.color.r;
        green = text.color.g;
        blue = text.color.b;
    }

    void Update()
    {
        float delta = Mathf.Sin(timeSinceStart * colorChangeRate) * 0.1f;
        text.color = scaledColor(0.98f + delta);
        timeSinceStart += Time.deltaTime;
    }

    private Color scaledColor(float scale)
    {
        return new Color(red * scale, green * scale, blue * scale, 1.0f);
    }
}
