using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject submenu;

    private void Start()
    {
        submenu.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        submenu.SetActive(true);
        CancelInvoke(nameof(Hide));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Invoke(nameof(Hide), 0.1f); // delay helps hovering into submenu
    }

    private void Hide()
    {
        submenu.SetActive(false);
    }
}

