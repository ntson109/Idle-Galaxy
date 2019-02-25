﻿using System;
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
        public int level;

        public long buyMoreMinePrice;

        public int capacity;

        public int miningTime;

        public long unitPrice;

        public float unlockTime;

        public float speedMining;

        public int unlockCondition;
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
        IDLE,
        LOCK,
        UNLOCKING,
        UPGRADING,
        WORKING
    }

    [Serializable]
    public struct Product
    {
        public TypeProduct type;
        public long amount;
    }
    #endregion

    [Header("PROPERTIES")]
    public int ID;
    public MineShaft.Properties properties; //thông số cơ bản    
    public int numberMine;
    public int totalCapacity;
    public int input = 0;
    public TypeMap typeMap;
    public Miner miner; //nhân viên
    public UnlockCost[] unlockCost;
    public TypeProduct typeProduct; //loại sản phẩm
    public StateMineShaft state; //trạng thái
    private bool isAutoWorking = false;
    public MineShaft nextMineShaft; //mỏ tiếp theo
    public int numberProduct_Completed; //số sản phẩm làm xong
    public int numberProduct_PushUp; //số sản phẩm đẩy lên mỏ trên
    public int numberProduct_Remain; //số sản phẩm thừa
    public int timer = 0; //thời gian đang chạy tiến trình Work
    public RectTransform posProduct_Remain;
    public RectTransform posProduct_PushUp;
    public RectTransform posProduct_Complete;
    public Map mapParent;
    private bool isCanWork = false;
    public float timeUnlocking;

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
    public GameObject panelUnlock;
    public GameObject panelUnlock_Condition;
    public Text txtUnlock_Condition;
    public Text txtTimeUnlock;

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
        if (!isCanWork && btnWork.gameObject.activeSelf)
            btnWork.gameObject.SetActive(false);
        else if (isCanWork && !btnWork.gameObject.activeSelf && !isAutoWorking)
            btnWork.gameObject.SetActive(true);

        if (isAutoWorking && this.state == StateMineShaft.IDLE && this.input > 0)
        {
            StartCoroutine(Work());
            btnWork.gameObject.SetActive(false);
        }

        txtTimer.text = UIManager.Instance.ToDateTimeString(timer);
        txtNumberMine.text = this.numberMine.ToString();

        if (this.state == StateMineShaft.UNLOCKING)
        {
            if (timeUnlocking <= 0)
            {
                UnlockComplete();
                timeUnlocking = 0;
            }
            timeUnlocking -= Time.deltaTime;
            txtTimeUnlock.text = transformToTime(timeUnlocking);
        }
    }
    #endregion

    string transformToTime(float time = 0)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ON_START_GAME()
    {
        btnWork.onClick.AddListener(() => Btn_Work());
        btnUpgrade.onClick.AddListener(() => Btn_Upgrade());
        btnBuyMoreMine.onClick.AddListener(() => Btn_BuyMoreMine());
        btnUnlock_byGold.onClick.AddListener(() => Btn_Unlock(TypeUnlock.GOLD));
        btnUnlock_byCoin.onClick.AddListener(() => Btn_Unlock(TypeUnlock.COIN));
        btnUnlock_byAD.onClick.AddListener(() => Btn_Unlock(TypeUnlock.ADS));

        GetInfo();
        this.properties.level = 1;
        this.numberMine = 1;
        this.totalCapacity = this.properties.capacity * this.numberMine;
        this.properties.speedMining = 2;
        this.state = StateMineShaft.LOCK;
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());
        txtTimeMining.text = UIManager.Instance.ToDateTimeString(this.properties.miningTime);
        if (ID == 0)
        {
            this.state = StateMineShaft.IDLE;
            this.input = this.totalCapacity;
            isCanWork = true;
        }

        if (this.state == StateMineShaft.LOCK)
        {
            UIManager.Instance.SetActivePanel(panelUnlock_Condition);
            txtUnlock_Condition.text = "Need: " + this.properties.unlockCondition.ToString() + " pre house !";
        }
        else
        {
            UIManager.Instance.SetDeActivePanel(panelUnlock_Condition);
            UIManager.Instance.SetDeActivePanel(panelUnlock);
        }
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
            if (GameManager.Instance.GOLD >= this.unlockCost[1].cost)
            {
                btnUnlock_byGold.interactable = true;
            }
            else
            {
                btnUnlock_byGold.interactable = false;
            }

            if (GameManager.Instance.COIN >= this.unlockCost[0].cost)
            {
                btnUnlock_byCoin.interactable = true;
            }
            else
            {
                btnUnlock_byCoin.interactable = false;
            }
        }
    }

    void GetInfo()
    {
        this.properties.capacity = GameConfig.Instance.lstPropertiesMap[ID].Productivity[0];
        this.properties.buyMoreMinePrice = GameConfig.Instance.lstPropertiesMap[ID].BuyMine_cost * this.properties.level / 10;
        this.properties.unitPrice = GameConfig.Instance.lstPropertiesMap[ID].Unit_Price;
        this.properties.unlockTime = GameConfig.Instance.lstPropertiesMap[ID].Unlock_time;
        this.properties.unlockCondition = GameConfig.Instance.lstPropertiesMap[ID].Unlock_condition;
        this.properties.miningTime = GameConfig.Instance.lstPropertiesMap[ID].miningTime;

        this.unlockCost = new UnlockCost[2];
        this.unlockCost[0].type = TypeUnlock.COIN;
        this.unlockCost[0].cost = GameConfig.Instance.lstPropertiesMap[ID].Unlock_cost[0];
        this.unlockCost[1].type = TypeUnlock.GOLD;
        this.unlockCost[1].cost = GameConfig.Instance.lstPropertiesMap[ID].Unlock_cost[1];
        txtUnlock_byCoin.text = UIManager.Instance.ToLongString(this.unlockCost[0].cost);
        txtUnlock_byGold.text = UIManager.Instance.ToLongString(this.unlockCost[1].cost) + "$";
    }

    public IEnumerator Work()
    {
        state = StateMineShaft.WORKING;
        isCanWork = false;
        //diễn anim working
        while (timer < this.properties.miningTime)
        {       
            yield return new WaitForSeconds(1f);
            timer++;
        }

        //chạy xong
        numberProduct_Completed = this.input;// *this.numberMine;
        if (nextMineShaft != null && nextMineShaft.state != StateMineShaft.LOCK && nextMineShaft.state != StateMineShaft.UNLOCKING && nextMineShaft.isActiveAndEnabled && nextMineShaft.input == 0)
        {
            if (numberProduct_Completed <= this.nextMineShaft.totalCapacity)
            {
                numberProduct_PushUp = numberProduct_Completed;
                Debug.Log("Chỉ chuyển lên next mine");
                product_PushUp.SetActive(true);
                txtProduct_PushUp.text = numberProduct_PushUp.ToString();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(Product_Move_PushUp());
            }
            else
            {
                numberProduct_PushUp = this.nextMineShaft.totalCapacity;
                numberProduct_Remain = numberProduct_Completed - numberProduct_PushUp;
                Debug.Log("Chia hàng chuyển 2 đg");
                product_PushUp.SetActive(true);
                product_Remain.SetActive(true);
                txtProduct_PushUp.text = numberProduct_PushUp.ToString();
                txtProduct_Remain.text = numberProduct_Remain.ToString();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(Product_Move_PushUp());
                StartCoroutine(Product_Move_Remain());
            }
        }
        else
        {
            Debug.Log("Chỉ chuyển lên top");
            numberProduct_Remain = numberProduct_Completed;
            product_Remain.SetActive(true);
            txtProduct_Remain.text = numberProduct_Remain.ToString();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Product_Move_Remain());
        }
        //diễn anime chạy xong
        yield return new WaitForSeconds(1.75f);
        numberProduct_Completed = numberProduct_PushUp = numberProduct_Remain = 0;
        if (this.ID == 0)
        {
            this.input = this.totalCapacity;
            isCanWork = true;
        }
        else
        {
            this.input = 0;
        }
        timer = 0;
        yield return new WaitForEndOfFrame();
        state = StateMineShaft.IDLE;
        if (!isAutoWorking)
            btnWork.gameObject.SetActive(true);
        Debug.Log("Done");

    }

    IEnumerator Product_Move_PushUp()
    {
        //if (product_PushUp.activeSelf)
        //{
        while (product_PushUp.GetComponent<RectTransform>().transform.position.x > this.posProduct_PushUp.position.x)
        {
            //product_PushUp.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(product_PushUp.GetComponent<RectTransform>().transform.position, this.posProduct_PushUp.position, Time.deltaTime * this.properties.speedMining);
            product_PushUp.GetComponent<RectTransform>().transform.Translate(Vector3.left * Time.deltaTime * this.properties.speedMining);
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        product_PushUp.SetActive(false);
        product_PushUp.transform.position = posProduct_Complete.position;
        nextMineShaft.GiveInput(numberProduct_PushUp);
        //}
    }

    IEnumerator Product_Move_Remain()
    {
        //if (product_Remain.activeSelf)
        //{
        while (product_Remain.GetComponent<RectTransform>().transform.position.x < this.posProduct_Remain.position.x)
        {
            //product_Remain.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(product_Remain.GetComponent<RectTransform>().transform.position, this.posProduct_Remain.position, Time.deltaTime * this.properties.speedMining);
            product_Remain.GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * this.properties.speedMining);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        product_Remain.SetActive(false);
        product_Remain.transform.position = posProduct_Complete.position;
        mapParent.AddProduct(numberProduct_Remain, numberProduct_Remain * this.properties.unitPrice);
        //}
    }

    public void GiveInput(int _input)
    {
        if (this.ID != 0)
        {
            input = _input;
            isCanWork = true;
        }
    }

    public void Btn_Work()
    {
        if (state == StateMineShaft.LOCK)
            return;

        StartCoroutine(Work());
        //btnWork.gameObject.SetActive(false);
    }

    public void BuyAI()
    {
        GameManager.Instance.AddGold(-100);
        this.isAutoWorking = true;
    }

    void Btn_Upgrade()
    {
        UpgradeComplete();
    }

    void UpgradeComplete()
    {

    }

    public void Btn_BuyMoreMine()
    {
        GameManager.Instance.AddGold(-this.properties.buyMoreMinePrice);
        BuyMoreMineComplete();
    }

    void BuyMoreMineComplete()
    {
        this.numberMine++;
        mapParent.CheckUnlock(this.ID);
        this.totalCapacity = this.properties.capacity * numberMine;
        if (this.ID == 0)
            this.input = this.totalCapacity;
    }

    public void ShowUnlockPanel()
    {
        UIManager.Instance.SetActivePanel(panelUnlock);
        UIManager.Instance.SetDeActivePanel(panelUnlock_Condition);
    }

    public void Btn_Unlock(TypeUnlock _type)
    {
        //diễn anim unlock
        if (_type == TypeUnlock.GOLD)
        {
            GameManager.Instance.AddGold(-this.unlockCost[1].cost);
            this.timeUnlocking = this.properties.unlockTime;
            this.state = StateMineShaft.UNLOCKING;
        }
        else if (_type == TypeUnlock.COIN)
        {
            GameManager.Instance.AddCoin(-this.unlockCost[0].cost);
            UnlockComplete();
        }
        else
        {
            btnUnlock_byAD.GetComponent<PlayUnityAd>().showAd();
            this.timeUnlocking = this.properties.unlockTime;
            this.state = StateMineShaft.UNLOCKING;
        }
        
    }

    public void UnlockComplete()
    {
        state = StateMineShaft.IDLE;
        isAutoWorking = false;
        UIManager.Instance.SetDeActivePanel(panelUnlock);
    }
}
