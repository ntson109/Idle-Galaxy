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
    //public float time;
    long capacityWillUp;
    public RectTransform posBeginTransport;
    public RectTransform posEndTransport;
    public Button thisButton;
    public Map mapParent;
    public bool isTransporting;

    void Start()
    {
        thisButton.onClick.AddListener(() => ShowUpgrade());
        isTransporting = false;
        this.speed = 3f;
    }

    void Update()
    {

    }

    public void SetInfo(int _level, long _cap, long _price)
    {
        this.level = _level;
        this.capacity = _cap;
        this.price = _price;
        this.isTransporting = false;
        this.speed = 3f;
        this.StopAllCoroutines();
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
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(-1, 1, 1);
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
        this.gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.35f);
        isTransporting = false;
    }

    public void ShowUpgrade()
    {
        GetCapacityUp();
        GetPriceUp();
        UIManager.Instance.SetActivePanel(UIManager.Instance.panelUpgradeTransporter);
        UIManager.Instance.txtLevelTrans.text = "Level " + this.level + " -> " + (this.level + 1);
        UIManager.Instance.txtCapTrans.text = UIManager.Instance.ToLongString(this.capacity) + " -> " + UIManager.Instance.ToLongString(this.capacityWillUp);
        UIManager.Instance.txtPriceTrans.text = "Upgrade\n" + UIManager.Instance.ToLongString(this.price);
        /*UIManager.Instance.btnUpTrans.thisButton.onClick.RemoveAllListeners();
        UIManager.Instance.btnUpTrans.thisPrice = this.price;
        UIManager.Instance.btnUpTrans.type = MyButton.Type.GOLD;
        UIManager.Instance.btnUpTrans.thisButton.onClick.AddListener(() => Upgrade());*/
        UIManager.Instance.btnUpTrans.Init(PriceType.GOLD, this.price, () => Upgrade());

        if (PlayerPrefs.GetInt(KeyPrefs.TUTORIAL_DONE) == 0)
        {
            UIManager.Instance.Step_9_Tutorial();
        }
    }

    void GetCapacityUp()
    {
        if (this.level <= 10)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[0];
        }
        else if (this.level <= 20)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[1];
        }
        else if (this.level <= 50)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[2];
        }
        else if (this.level <= 100)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[3];
        }
        else if (this.level <= 200)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[4];
        }
        else if (this.level <= 500)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[5];
        }
        else if (this.level > 500)
        {
            this.capacityWillUp = this.capacity + GameConfig.Instance.Trans_Capacity_2[6];
        }
    }

    void GetPriceUp()
    {
        if (this.level <= 10)
        {
            double t = 1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[0];
            if (t - (long)t > 0.5f)
            {
                t += 1;
            }
            this.price = (long)t;
        }
        else if (this.level <= 20)
        {
            this.price = (long)(1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[1]);

        }
        else if (this.level <= 50)
        {
            this.price = (long)(1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[2]);
        }
        else if (this.level <= 100)
        {
            this.price = (long)(1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[3]);
        }
        else if (this.level <= 200)
        {
            this.price = (long)(1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[4]);
        }
        else if (this.level <= 500)
        {
            this.price = (long)(1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[5]);
        }
        else if (this.level > 500)
        {
            this.price = (long)(1 + this.price + this.price * GameConfig.Instance.Trans_Cost_2[6]);
        }
    }

    public void Upgrade()
    {
        GameManager.Instance.AddGold(-this.price);
        this.level += 1;
        this.capacity = capacityWillUp;
        ShowUpgrade();

        if (PlayerPrefs.GetInt(KeyPrefs.TUTORIAL_DONE) == 0)
        {
            UIManager.Instance.Step_10_Tutorial();
        }
    }
}
