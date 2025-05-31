using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : HuyMonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    //==========================================Variable==========================================
    [Header("Joystick Settings")]
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 100f;

    private Vector2 inputDirection = Vector2.zero;
    public Vector2 Direction => inputDirection;

    [SerializeField] private Canvas canvas;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.canvas, transform.parent, "LoadCanvas()");
        this.LoadComponent(ref this.background, transform, "LoadBackground");
        this.LoadComponent(ref this.handle, transform.Find("Handle"), "LoadHandle()");
    }


    //=========================================Interface==========================================
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, canvas.worldCamera, out pos);
        pos = Vector2.ClampMagnitude(pos, handleRange);
        handle.anchoredPosition = pos;
        inputDirection = pos / handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        inputDirection = Vector2.zero;
    }
}
