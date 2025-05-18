using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ImageOnOff : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image bloodImage;
   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bloodImage != null) bloodImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bloodImage != null) bloodImage.enabled = false;
    }
}
