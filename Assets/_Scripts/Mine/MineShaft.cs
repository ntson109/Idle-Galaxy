using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class MineShaft : MonoBehaviour
{
    #region === STRUCT ===
    [Serializable]
    public struct Properties
    {
        public int ID;

        public int level;

        public int numberMine;

        public long buyMoreMinePrice;

        public int capacity;

        public int miningTime;

        public long unitPrice;

        public float unlockTime;

        public float speedMining;

        public void Reset()
        {
            level = 1;
            numberMine = 1;
        }
    }

    [Serializable]
    public struct UnlockCost
    {
        public TypeUnlock type;
        public long cost;
    }

    [Serializable]
    public enum TypeUnlock
    {
        GOLD,
        COIN,
        ADS
    }

    [Serializable]
    public enum TypeProduct
    {
        GOLD,
        SP1,
        SP2,
        SP3,
        SP4
    }

    [Serializable]
    public enum StateMineShaft
    {
        NONE,
        LOCK,
        WORKING
    }

    [Serializable]
    public struct Product
    {
        public TypeProduct type;
        public long amount;
    }
    #endregion

    public MineShaft.Properties properties; //thông số cơ bản
    public TypeMap typeMap;
    public Miner miner; //nhân viên
    public UnlockCost[] unlockCost;
    public TypeProduct typeProduct; //loại sản phẩm
    public StateMineShaft state; //trạng thái
    public bool isAutoWorking;
    public MineShaft nextMineShaft; //mỏ tiếp theo
    public int numberProduct_Completed; //số sản phẩm làm xong
    public int numberProduct_PushUp; //số sản phẩm đẩy lên mỏ trên
    public int numberProduct_Remain; //số sản phẩm thừa
    public int timer = 0; //thời gian đang chạy tiến trình Work
    public Transform posProduct_Remain;
    public Transform posProduct_PushUp;
    public Transform posProduct_Complete;
    public Map mapParent;

    [Header("UI")]
    public Text txtTimer;
    public Text txtTimeMining;
    public Text txtNumberMine;
    public Button btnWork;
    public Button btnUpgrade;
    public Button btnBuyMoreMine;
    public Button btnUnlock_byGold;
    public Text txtUnlock_byGold;
    public Button btnUnlock_byCoin;
    public Text txtUnlock_byCoin;
    public Button btnUnlock_byAD;

    public GameObject product_PushUp;
    public Text txtProduct_PushUp;
    public GameObject product_Remain;
    public Text txtProduct_Remain;

    #region === START VS UPDATE ===
    void Start()
    {
        typeMap = TypeMap.MOON;
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
    }

    void LateUpdate()
    {
        if (isAutoWorking && this.state == StateMineShaft.NONE)
        {
            StartCoroutine(Work());
            btnWork.gameObject.SetActive(false);
        }
    }
    #endregion

    void ON_START_GAME()
    {
        btnWork.onClick.AddListener(() => Btn_Work());
        btnUpgrade.onClick.AddListener(() => Btn_Upgrade());
        btnBuyMoreMine.onClick.AddListener(() => Btn_BuyMoreMine());
        btnUnlock_byGold.onClick.AddListener(() => Btn_Unlock(TypeUnlock.GOLD));
        btnUnlock_byCoin.onClick.AddListener(() => Btn_Unlock(TypeUnlock.COIN));
        btnUnlock_byAD.onClick.AddListener(() => Btn_Unlock(TypeUnlock.ADS));
        txtTimeMining.text = UIManager.Instance.ToDateTimeString(this.properties.miningTime);

        this.properties.level = 1;
        this.properties.buyMoreMinePrice = GameConfig.Instance.lstPropertiesMap[0].BuyMine_cost * this.properties.level / 10;
        this.state = StateMineShaft.NONE;
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());
    }

    void ON_CHANGE_GOLD_COIN()
    {
        if (state != StateMineShaft.LOCK)
        {
            if (GameManager.Instance.GOLD >= this.properties.buyMoreMinePrice)
            {
                btnBuyMoreMine.interactable = true;
            }
            else
            {
                btnBuyMoreMine.interactable = false;
            }
        }
        else
        {
            if (GameManager.Instance.GOLD >= this.unlockCost[0].cost)
            {
                btnUnlock_byGold.interactable = true;
            }
            else
            {
                btnUnlock_byGold.interactable = false;
            }

            if (GameManager.Instance.COIN >= this.unlockCost[1].cost)
            {
                btnUnlock_byCoin.interactable = true;
            }
            else
            {
                btnUnlock_byCoin.interactable = false;
            }
        }
    }

    public IEnumerator Work()
    {
        state = StateMineShaft.WORKING;
        //diễn anim working
        while (timer < this.properties.miningTime)
        {
            //Debug.Log("Working");
            txtTimer.text = UIManager.Instance.ToDateTimeString(timer);
            yield return new WaitForSeconds(1f);
            timer++;
        }

        //chạy xong
        numberProduct_Completed = this.properties.capacity;
        if (nextMineShaft != null)
        {
            if (numberProduct_Completed <= this.nextMineShaft.properties.capacity)
            {
                numberProduct_PushUp = numberProduct_Completed;
                Debug.Log("Chỉ chuyển lên next mine");
                product_PushUp.SetActive(true);
                StartCoroutine(Product_Move_PushUp());
            }
            else
            {
                numberProduct_PushUp = this.nextMineShaft.properties.capacity;
                numberProduct_Remain = numberProduct_Completed - numberProduct_PushUp;
                Debug.Log("Chia hàng chuyển 2 đg");
                product_PushUp.SetActive(true);
                product_Remain.SetActive(true);
                StartCoroutine(Product_Move_PushUp());
                StartCoroutine(Product_Move_Remain());
            }
        }
        else
        {
            Debug.Log("Chỉ chuyển lên top");
            numberProduct_Remain = numberProduct_Completed;
            product_Remain.SetActive(true);
            StartCoroutine(Product_Move_Remain());
        }

        //diễn anime chạy xong
        yield return new WaitForSeconds(3f);
        nextMineShaft.GiveInput(numberProduct_PushUp);
        numberProduct_Completed = numberProduct_PushUp = numberProduct_Remain = 0;

        yield return new WaitForEndOfFrame();
        product_PushUp.transform.position = posProduct_Complete.position;
        product_Remain.transform.position = posProduct_Complete.position;
        state = StateMineShaft.NONE;
        if (!isAutoWorking)
            btnWork.gameObject.SetActive(true);
        Debug.Log("Done");
    }

    IEnumerator Product_Move_PushUp()
    {
        if (product_PushUp.activeSelf)
        {
            while (product_PushUp.transform.position.y > this.posProduct_PushUp.position.y)
            {
                product_PushUp.transform.position = Vector3.MoveTowards(product_PushUp.transform.position, this.posProduct_PushUp.position, Time.deltaTime * this.properties.speedMining);
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            product_PushUp.SetActive(false);
        }
    }

    IEnumerator Product_Move_Remain()
    {
        if (product_Remain.activeSelf)
        {
            while (product_Remain.transform.position.y < this.posProduct_Remain.position.y)
            {
                product_Remain.transform.position = Vector3.MoveTowards(product_Remain.transform.position, this.posProduct_Remain.position, Time.deltaTime * this.properties.speedMining);
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            product_Remain.SetActive(false);
        }
    }

    public void GiveInput(long _input)
    {
        if (this.properties.ID != 1)
        {

        }
    }

    public void Btn_Work()
    {
        if (state == StateMineShaft.LOCK)
            return;

        StartCoroutine(Work());
        btnWork.gameObject.SetActive(false);
    }

    void Btn_Upgrade()
    {
        UpgradeComplete();
    }

    void UpgradeComplete()
    {

    }

    void Btn_BuyMoreMine()
    {
        GameManager.Instance.AddGold(-this.properties.buyMoreMinePrice);
        BuyMoreMineComplete();
    }

    void BuyMoreMineComplete()
    {
        this.properties.numberMine++;
    }

    public void Btn_Unlock(TypeUnlock _type)
    {
        //diễn anim unlock
        if (_type == TypeUnlock.GOLD)
        {
            GameManager.Instance.AddGold(-this.unlockCost[0].cost);
        }
        else if (_type == TypeUnlock.COIN)
        {
            GameManager.Instance.AddCoin(-this.unlockCost[1].cost);
        }
        else
        {
            //Xem ads
        }
    }

    void UnlockComplete()
    {
        state = StateMineShaft.NONE;
        isAutoWorking = false;
    }

    public Text txtTest1;
    public Text txtTest2;
    public void ABC()
    {
        txtTest1.gameObject.transform.position = txtTest2.gameObject.transform.position;
    }
}
