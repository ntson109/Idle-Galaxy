using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transporter : MonoBehaviour
{
    public int level;
    public long capacity;
    public float speed;
    public long price;
    public float time;
    long capacityWillUp;
    float speedWillUp;
    public RectTransform posBeginTransport;
    public RectTransform posEndTransport;
    public Button thisButton;
    public Map mapParent;
    public bool isTransporting;

    void Start()
    {
        thisButton.onClick.AddListener(() => ShowUpgrade());
        isTransporting = false;
    }

    void Update()
    {

    }

    public void SetInfo(int _level, long _cap, float _speed)
    {
        this.level = _level;
        this.capacity = _cap;
        this.speed = _speed;
    }

    public void Transport()
    {
        if (!isTransporting)
        {
            StartCoroutine(TurnGo());
            isTransporting = true;
        }
    }

    IEnumerator TurnGo()
    {
        while (this.gameObject.GetComponent<RectTransform>().transform.position.x > this.posEndTransport.position.x)
        {
            //this.gameObject.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().transform.position, this.posEndTransport.position, Time.deltaTime * this.speed);
            this.gameObject.GetComponent<RectTransform>().transform.Translate(Vector3.left * Time.deltaTime * this.speed);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        mapParent.CompleteTransport();
        yield return new WaitForSeconds(0.25f);
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(TurnBack());
    }

    IEnumerator TurnBack()
    {
        while (this.gameObject.GetComponent<RectTransform>().transform.position.x < this.posBeginTransport.position.x)
        {
            //this.gameObject.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(this.gameObject.GetComponent<RectTransform>().transform.position, this.posBeginTransport.position, Time.deltaTime * this.speed);
            this.gameObject.GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * this.speed);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(0.35f);
        isTransporting = false;
    }

    public void ShowUpgrade()
    {
        GetCapacityUp();
        GetPriceUp();
        UIManager.Instance.SetActivePanel(UIManager.Instance.panelUpgradeTransporter);
        UIManager.Instance.btnUpTrans.thisButton.onClick.RemoveAllListeners();
        UIManager.Instance.txtNameTrans.text = GameConfig.Instance.NameTransport;
        UIManager.Instance.txtLevelTrans.text = this.level.ToString();
        UIManager.Instance.txtLevelTrans_Up.text = (this.level + 1).ToString();
        UIManager.Instance.txtCapTrans.text = UIManager.Instance.ToLongString(this.capacity);
        UIManager.Instance.txtCapTrans_Up.text = UIManager.Instance.ToLongString(this.capacityWillUp);
        UIManager.Instance.txtTimeTrans.text = this.time.ToString();
        UIManager.Instance.txtTimeTrans_Up.text = this.time.ToString();
        UIManager.Instance.btnUpTrans.thisPrice = this.price;
        UIManager.Instance.txtPriceTrans.text = "Upgrade\n" + UIManager.Instance.ToLongString(this.price);
        UIManager.Instance.btnUpTrans.type = MyButton.Type.GOLD;
        UIManager.Instance.btnUpTrans.thisButton.onClick.AddListener(() => Upgrade());
    }

    void GetCapacityUp()
    {
        if (this.level <= 100)
        {
            double t = this.capacity + this.capacity * GameConfig.Instance.Capacity_2;
            if ((this.capacity * GameConfig.Instance.Capacity_2) < 2)
            {
                t = this.capacity + 2;
            }
            if (t - (long)t > 0.5f)
            {
                t += 1;
            }
            this.capacityWillUp = (long)t;
        }
        else
        {
            this.capacityWillUp = (long)(this.capacity + this.capacity * GameConfig.Instance.Capacity_3);
        }
    }

    void GetPriceUp()
    {
        if (this.level <= 10)
        {
            this.price = GameConfig.Instance.lstCostTransporter[0];
        }
        else if (this.level <= 20)
        {
            this.price = GameConfig.Instance.lstCostTransporter[1];
        }
        else if (this.level <= 50)
        {
            this.price = GameConfig.Instance.lstCostTransporter[2];
        }
        else if (this.level <= 100)
        {
            this.price = GameConfig.Instance.lstCostTransporter[3];
        }
        else if (this.level <= 200)
        {
            this.price = GameConfig.Instance.lstCostTransporter[4];
        }
        else if (this.level <= 500)
        {
            this.price = GameConfig.Instance.lstCostTransporter[4];
        }
        else if (this.level > 500)
        {
            this.price = GameConfig.Instance.lstCostTransporter[4];
        }
    }

    void Upgrade()
    {
        GameManager.Instance.AddGold(-this.price);
        this.level += 1;
        this.capacity = capacityWillUp;
        ShowUpgrade();
    }
}
