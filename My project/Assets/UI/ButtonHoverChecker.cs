using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void ButtonHoverEventHandler(GameObject obj,bool isHovering); // Delegate for hover event
    public static event ButtonHoverEventHandler OnButtonHover; // Static event for hover

    public Button button; // Reference to the Button component

    // Implement the IPointerEnterHandler interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject gO = eventData.pointerEnter.transform.parent.gameObject;
        if (gO == button.gameObject)
        {
           // Debug.Log("Mouse pointer is hovering over the button.");
            RaiseButtonHoverEvent(true); // Raise hover event with true parameter
        }
    }

    // Implement the IPointerExitHandler interface
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject gO = eventData.pointerEnter.transform.parent.gameObject;
        if (gO == button.gameObject)
        {
           // Debug.Log("Mouse pointer is no longer hovering over the button.");
            RaiseButtonHoverEvent(false); // Raise hover event with false parameter
        }
    }

    // Method to raise the hover event
    private void RaiseButtonHoverEvent(bool isHovering)
    {
        if (OnButtonHover != null)
        {
            OnButtonHover(this.gameObject,isHovering);
        }
    }
}
