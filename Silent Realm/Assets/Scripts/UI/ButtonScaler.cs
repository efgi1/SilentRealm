using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float xScale = 1.06f;
    [SerializeField] float yScale = 1.03f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Messenger.Broadcast(GameEvent.BUTTON_SELECTED);
        scaleUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleDown();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (Time.timeSinceLevelLoad > 0.02f)
        {
            Messenger.Broadcast(GameEvent.BUTTON_SELECTED);
        }
        scaleUp();
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        scaleDown();
    } 

    private void scaleUp()
    {
        transform.localScale = new Vector3(xScale, yScale, 1.0f);
    }

    private void scaleDown()
    {
        transform.localScale = originalScale;
    }
}
