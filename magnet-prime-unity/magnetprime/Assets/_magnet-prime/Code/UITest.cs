using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using StarterAssets;

public class UITest : MonoBehaviour
{
    int UILayer;
    [SerializeField] StarterAssetsInputs sai;

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        sai.click += ClickListener;
    }

    private void Update()
    {
        //print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
        IsPointerOverUIElement();
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        foreach(RaycastResult rcr in eventSystemRaysastResults)
        {
            if(rcr.gameObject.TryGetComponent(out Button butt))
            {
                butt.Select();
            }
        }

        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Pointer.current.position.ReadValue();
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    void ClickListener()
    {
        EventSystem.current.currentSelectedGameObject?.GetComponent<Button>()?.onClick.Invoke();
        EventSystem.current.SetSelectedGameObject(null);
    }
}