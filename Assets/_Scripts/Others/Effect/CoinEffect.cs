using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffect : MonoBehaviour {
    private double value;

    private bool isCollect;

    private const float minY = -190f;

    private const float a = -700f;

    private float targetY;

    private float time;

    private string textDisplay;

    private Vector2 firstVelocity;

    private bool auto;

    private bool isAvaiable;
    public void Init(double pValue, string ptextDisplay, bool autoCollect = false)
    {
        this.value = pValue;
        this.textDisplay = ptextDisplay;
        this.isCollect = false;
        this.targetY = Random.Range(-190f, -150f);
        this.firstVelocity = new Vector2(Random.Range(-200f, 200f), (float)Random.Range(100, 200));
        this.auto = autoCollect;
        this.isAvaiable = true;
    }

    private void Update()
    {
        //if (!this.isCollect)
        //{
        //    if (base.transform.localPosition.y > this.targetY)
        //    {
        //        Vector3 localPosition = base.transform.localPosition;
        //        localPosition.x += this.firstVelocity.x * Time.deltaTime;
        //        this.firstVelocity.y = this.firstVelocity.y + -350f * Time.deltaTime;
        //        localPosition.y += this.firstVelocity.y * Time.deltaTime;
        //        base.transform.localPosition = localPosition;
        //    }
        //    else if (this.auto)
        //    {
        //        this.isCollect = true;
        //        GameManager.instance.ShowTextDamage(this.textDisplay, base.transform.position, new Color32(byte.MaxValue, 223, 88, byte.MaxValue));
        //    }
        //}
        //else
        //{
        //    if (base.transform.parent != GameManager.instance.textView)
        //    {
        //        base.transform.parent = GameManager.instance.textView;
        //    }
        //    if (Vector3.Distance(base.transform.position, GameManager.instance.coinIcon.transform.position) < 0.1f)
        //    {
        //        VangManager.GetInstance().AddVang(this.value);
        //        this.Deactive();
        //    }
        //    else
        //    {
        //        base.transform.position = Vector3.Lerp(base.transform.position, GameManager.instance.coinIcon.transform.position, Time.deltaTime * 4f);
        //    }
        //}
    }

    private void OnDisable()
    {
        if (this.isAvaiable)
        {
            if (this.isCollect)
            {
                //VangManager.GetInstance().AddVang(this.value);
                this.Deactive();
            }
            else
            {
                this.Deactive();
            }
        }
    }

    public void OnCollect()
    {
        this.isCollect = true;
        //GameManager.instance.ShowText(this.textDisplay, base.transform.position, new Color32(byte.MaxValue, 223, 88, byte.MaxValue));
    }

    private void Deactive()
    {
        this.isAvaiable = false;
        //MyPool.DespawnObj("UI", base.transform);
    }   
}
