using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragnDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 positionBeforeMovement; 

    private void Awake() //executes when the script instance is loaded
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; //disable the listener on this instance
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        // make the object move alongside the mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // in case the canvas scales it still follows the mouse


    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = positionBeforeMovement;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        positionBeforeMovement = rectTransform.anchoredPosition;
    }




    
}
