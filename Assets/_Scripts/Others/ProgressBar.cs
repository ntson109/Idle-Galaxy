using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image fillerImage;

    [SerializeField]
    [HideInInspector]
    private float _value;

    [SerializeField]
    [HideInInspector]
    private float _minValue;

    [SerializeField]
    [HideInInspector]
    private float _maxValue = 1f;

    public float Value
    {
        get
        {
            return this._value;
        }
        set
        {
            this._value = value;
            this._value = Mathf.Clamp(this._value, this._minValue, this._maxValue);
            this.OnValueChanged();
        }
    }

    public float MinValue
    {
        get
        {
            return this._minValue;
        }
    }

    public float MaxValue
    {
        get
        {
            return this._maxValue;
        }
        set
        {
            this._maxValue = value;
            this.UpdateLayout();
        }
    }

    private void OnValueChanged()
    {
        this.UpdateLayout();
    }

    private void UpdateLayout()
    {
        if (this.fillerImage == null)
        {
            return;
        }
        float fillAmount = this._value / this._maxValue;
        this.fillerImage.fillAmount = fillAmount;
    }
}

