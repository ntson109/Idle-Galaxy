using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Canvas))]
public class CanvasVisibility : MonoBehaviour {
    public float lowerOffset;

    private Canvas canvas;

    public ScrollRect scrollRect;

    private RectTransform scrollRectViewPort;

    private bool visible;

    private float distance;

    private void Start()
    {
        this.canvas = base.GetComponent<Canvas>();
        //this.scrollRect = base.GetComponentInParent<ScrollRect>();
        this.scrollRectViewPort = this.scrollRect.viewport;
        this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.UpdateCanvasVisibility));
    }

    private void Disable()
    {
        this.scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.UpdateCanvasVisibility));
    }

    private void UpdateCanvasVisibility(Vector2 position)
    {
        this.visible = false;
        this.distance = Mathf.Abs(this.scrollRectViewPort.InverseTransformPoint(base.transform.position).y);
        if (this.distance - this.lowerOffset < this.scrollRectViewPort.rect.height)
        {
            this.visible = true;
        }
        if (this.visible != this.canvas.enabled)
        {
            this.canvas.enabled = this.visible;
        }
    }
}
