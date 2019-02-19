using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;

    public float shakeDuration;

    public float shakeAmount = 0.7f;

    public float decreaseFactor = 1f;

    private bool canShake;

    private Vector3 originalPos;
    private void Awake()
    {
        if (this.camTransform == null)
        {
            this.camTransform = (base.GetComponent(typeof(Transform)) as Transform);
        }
    }

    public void Init(float shakeDuration)
    {
        this.shakeDuration = shakeDuration;
        this.originalPos = this.camTransform.localPosition;
        this.canShake = true;
    }

    private void Update()
    {
        if (this.canShake)
        {
            if (this.shakeDuration > 0f)
            {
                this.camTransform.localPosition = this.originalPos + Random.insideUnitSphere * this.shakeAmount;
                this.shakeDuration -= Time.deltaTime * this.decreaseFactor;
            }
            else
            {
                this.shakeDuration = 0f;
                this.camTransform.localPosition = this.originalPos;
                this.canShake = false;
            }
        }
    }
}

