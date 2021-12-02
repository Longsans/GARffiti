using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingCanvas : BtnBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        ARCursor.Instance.Drawing(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Clicked();
        ARCursor.Instance.StartDrawing(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ARCursor.Instance.EndDrawing();
    }
}
