using System;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedOffset : MonoBehaviour
{
    private Material mat;

    private Vector2 offset = Vector2.zero;
    private void Start()
    {
        this.mat = base.GetComponent<Image>().material;
    }

    private void Update()
    {
        this.mat.mainTextureOffset = new Vector2(Mathf.Repeat(Time.time / 6f, 1f), 0f);
    }
}
