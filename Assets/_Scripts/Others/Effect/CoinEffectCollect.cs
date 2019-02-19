using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffectCollect : MonoBehaviour
{
    private Vector3 point;

    private Vector2 firstVelocity;

    private float velocity;

    private float tmp;

    private double value;

    private bool isDone = true;

    private bool isInit;
    public void Init(Vector3 point, double value)
    {
        this.point = point;
        Vector2 vector;
        //vector.ctor(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //this.firstVelocity = vector.normalized;
        this.velocity = Random.Range(3f, 6f);
        this.tmp = Random.Range(0.5f, 1f);
        base.transform.localScale = Vector3.zero;
        this.value = value;
        this.isDone = false;
        this.isInit = true;
    }

    private void Update()
    {
        //if (this.tmp > 0f)
        //{
        //    this.tmp -= Time.deltaTime;
        //    Vector2 vector = this.firstVelocity * Time.deltaTime * this.velocity * this.tmp;
        //    base.transform.position += new Vector3(vector.x, vector.y, 0f);
        //    base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, Time.deltaTime * 6f);
        //}
        //else
        //{
        //    base.transform.position = Vector3.Lerp(base.transform.position, this.point, Time.deltaTime * 4f);
        //    if (Vector3.Distance(base.transform.position, this.point) < 0.2f)
        //    {
        //        this.OnDoneMove();
        //        GameManager.instance.AddTapEffect(this.point);
        //        this.Deactive();
        //    }
        //}
    }

    public virtual void OnDoneMove()
    {
        //VangManager.GetInstance().AddVang(this.value);
        //SoundManager.PlaySound("item");
        this.isDone = true;
        this.isInit = false;
    }

    private void OnDisable()
    {
        if (!this.isDone)
        {
            this.OnDoneMove();
            this.Deactive();
        }
    }

    private void Deactive()
    {
        //MyPool.DespawnObj("UI", base.transform);
    }
}
