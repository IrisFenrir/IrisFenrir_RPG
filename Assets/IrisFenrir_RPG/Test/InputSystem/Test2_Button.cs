using UnityEngine;
using UnityEngine.EventSystems;

public class Test2_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool down;
    public bool pressing;
    public bool up;

    public void OnPointerDown(PointerEventData eventData)
    {
        down = true;
        pressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        up = true;
        pressing = false;
    }

    private void LateUpdate()
    {
        down = false;
        up = false;
    }
}

