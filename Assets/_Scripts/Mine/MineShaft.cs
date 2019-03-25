using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;
using DG.Tweening;

public class MineShaft : MonoBehaviour
{
    #region === STRUCT ===
    [Serializable]
    public struct Properties
    {
        public string name;

        public int level;

        public long buyMoreMinePrice;

        public int capacity;

        public int miningTime;

        public double unitPrice;

        public float unlockTime;

        public float speedMining;

        public int unlockCondition;

        //public long buyAI;

        public long upgradePrice;

        public int upgradeTime;
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
    public enum StateMineShaft
    {
        IDLE,
        LOCK,
        UNLOCKING,
        WORKING
    }

    [Serializable]
    public struct UpgradeSpecial
    {
        public string name;
        public string description;
        public bool state;
        public long price;
        public float time;
        public int coinMax;
    }

    [Serializable]
    public struct Store
    {
        public MineShaft mineShaft;
        public int level;
        public int value;
        public int capacity;
        public long cost;
    }

    #endregion

    [Header("PROPERTIES")]
    public int ID;
    public MineShaft.Properties properties; //thông số cơ bản    
    public int numberMine;
    public int xMoreMine = 1;
    public double pricePreMine;
    public int totalCapacity;
    public int input = 0;
    //public TypeMap typeMap;
    public UnlockCost[] unlockCost;
    public StateMineShaft state; //trạng thái
    public List<UpgradeObj_Special> lstUpgradeSpecial;
    public List<UpgradeObj_Special.Type> typeUpgradeSpecial = new List<UpgradeObj_Special.Type>();
    public List<float> timeUpgradeSpecial = new List<float>();
    public int[] timeUpgradeSpecial_Max;
    public bool isAutoWorking = false;
    public MineShaft nextMineShaft; //mỏ tiếp theo
    public MineShaft preMineShaft;
    public int numberProduct_Completed; //số sản phẩm làm xong
    public int numberProduct_PushUp; //số sản phẩm đẩy lên mỏ trên
    public int numberProduct_Remain; //số sản phẩm thừa
    public int timer = 0; //thời gian đang chạy tiến trình Work
    public RectTransform posProduct_Remain;
    public RectTransform posProduct_PushUp;
    public RectTransform posProduct_Complete;
    public RectTransform posProduct_Complete_1;
    public Map mapParent;
    private bool isCanWork = false;
    public float timeUnlocking;

    public UpgradeObj_Level.Type typeUpgradeLevel;
    public float timeUpgradeLevel;
    private float timeUpgradeLevel_Max;

    public Store store;
    public Button btnStore;

    [Header("UI")]
    public Text txtName;
    public Text txtLevel;
    public Text txtTimer;
    public Text txtNumberMine;
    public Text txtMoreMinePrice;
    public Text txtX;
    public Button btnWork;
    public Button btnUpgrade;
    public MyButton btnBuyMoreMine;
    public Button btnX;
    public Button btnUnlock_byGold;
    public Text txtUnlock_byGold;
    public Button btnUnlock_byCoin;
    public Text txtUnlock_byCoin;
    public Button btnUnlock_byAD;
    public GameObject panelUnlock;
    public GameObject panelUnlock_Condition;
    public Text txtUnlock_Condition;
    public Text txtTimeUnlock;

    public GameObject product_Complete;
    public Text txtProduct_Complete;
    public GameObject product_PushUp;
    public Text txtProduct_PushUp;
    public GameObject product_Remain;
    public Text txtProduct_Remain;

    [Header("ANIM")]
    public Animator workAnim;
    public Animator pushAnim;
    public Image imgWorkBar;
    public Sprite sprLight0;
    public GameObject imgAI;

    #region === START VS UPDATE ===
    void Start()
    {
        //typeMap = TypeMap.MOON;
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
    }

    void LateUpdate()
    {
        if (GameManager.Instance.stateGame == StateGame.PLAYING)
        {
            if (!isCanWork && btnWork.gameObject.activeSelf)
                btnWork.gameObject.SetActive(false);
            else if (isCanWork && !btnWork.gameObject.activeSelf && !isAutoWorking)
                btnWork.gameObject.SetActive(true);

            if (this.ID == 0)
            {
                if (isAutoWorking && this.state == StateMineShaft.IDLE && this.input > 0)
                {
                    LetWork();
                    btnWork.gameObject.SetActive(false);
                }
            }
            else
            {
                if (isAutoWorking && this.state == StateMineShaft.IDLE && this.preMineShaft.store.value > 0)
                {
                    LetWork();
                    btnWork.gameObject.SetActive(false);
                }
            }

            txtTimer.text = UIManager.Instance.ToDateTimeString(timer);
            txtNumberMine.text = this.numberMine.ToString();
            //if (nextMineShaft != null && nextMineShaft.state != StateMineShaft.LOCK && nextMineShaft.state != StateMineShaft.UNLOCKING && nextMineShaft.isActiveAndEnabled)
            //{
            //    //txtProduct_PushUp.text = numberProduct_PushUp + "/" + this.nextMineShaft.totalCapacity;
            //    if (this.store.value < this.nextMineShaft.totalCapacity)
            //    {
            //        this.nextMineShaft.GiveInput(this.store.value);
            //        this.store.value = 0;
            //    }
            //    else
            //    {
            //        this.nextMineShaft.GiveInput(this.nextMineShaft.totalCapacity);
            //        this.store.value -= this.nextMineShaft.totalCapacity;
            //    }
            //}
            txtProduct_PushUp.text = this.store.value + "/" + this.store.capacity;
            if (this.state == StateMineShaft.WORKING)
            {
                imgWorkBar.fillAmount += (1.000f / this.properties.miningTime) * Time.deltaTime;
            }

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

            for (int i = 0; i < this.typeUpgradeSpecial.Count; i++)
            {
                if (this.typeUpgradeSpecial[i] == UpgradeObj_Special.Type.UPGRADING)
                {
                    if (this.timeUpgradeSpecial[i] <= 0)
                    {
                        this.timeUpgradeSpecial[i] = 0;
                        SpecialUpgrade_Complete_1(i);
                    }
                    this.timeUpgradeSpecial[i] -= Time.deltaTime;
                }
            }

            if (this.typeUpgradeLevel == UpgradeObj_Level.Type.UPGRADING)
            {
                if (this.timeUpgradeLevel <= 0)
                {
                    this.timeUpgradeLevel = 0;
                    UpgradeLevel_Complete();
                }
                this.timeUpgradeLevel -= Time.deltaTime;
            }
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
        btnUpgrade.onClick.AddListener(() => Btn_ShowUpgrade());
        btnBuyMoreMine.thisButton.onClick.AddListener(() => Btn_BuyMoreMine());
        btnX.onClick.AddListener(() => Btn_X());
        if (this.ID < 5)
        {
            btnStore.onClick.AddListener(() => Btn_ShowUpgrade_Store());
        }        
        btnUnlock_byGold.onClick.AddListener(() => Btn_Unlock(TypeUnlock.GOLD));
        btnUnlock_byCoin.onClick.AddListener(() => Btn_Unlock(TypeUnlock.COIN));
        btnUnlock_byAD.onClick.AddListener(() => Btn_Unlock(TypeUnlock.ADS));

        GetInfo();

        this.RegisterListener(EventID.SKIP_TIME, (param) => ON_SKIP_TIME(param));
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());
        if (ID == 0)
        {
            this.state = StateMineShaft.IDLE;
            this.input = this.totalCapacity;
            isCanWork = true;
        }

        if (this.state == StateMineShaft.LOCK)
        {
            UIManager.Instance.SetActivePanel(panelUnlock_Condition);
            txtUnlock_Condition.text = "Need: unlock pre house !";

            if (ID >= 1 && mapParent.lstMineShaft[ID - 1].state != StateMineShaft.LOCK && mapParent.lstMineShaft[ID - 1].state != StateMineShaft.UNLOCKING)
            {
                UIManager.Instance.SetDeActivePanel(panelUnlock_Condition);
            }

            if (ID == 1)
            {
                UIManager.Instance.SetDeActivePanel(panelUnlock_Condition);
                UIManager.Instance.SetActivePanel(panelUnlock);
            }
        }

        if (this.state != StateMineShaft.LOCK)
        {
            UIManager.Instance.SetDeActivePanel(panelUnlock_Condition);
            UIManager.Instance.SetDeActivePanel(panelUnlock);
        }

        if (isAutoWorking)
        {
            imgAI.SetActive(true);
        }
    }

    void ON_CHANGE_GOLD_COIN()
    {
        if (state != StateMineShaft.LOCK)
        {
            //if (GameManager.Instance.GOLD >= this.properties.buyMoreMinePrice)
            //{
            //    btnBuyMoreMine.interactable = true;
            //}
            //else
            //{
            //    btnBuyMoreMine.interactable = false;
            //}
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

    void ON_SKIP_TIME(object param)
    {
        int a = (int)param;

        if (state == StateMineShaft.UNLOCKING)
        {
            if (timeUnlocking > a)
            {
                timeUnlocking -= a;
            }
            else
            {
                timeUnlocking = 0;
            }
        }

        if (this.typeUpgradeLevel == UpgradeObj_Level.Type.UPGRADING)
        {
            if (timeUpgradeLevel > a)
            {
                timeUpgradeLevel -= a;
            }
            else
            {
                timeUpgradeLevel = 0;
            }
        }

        for (int i = 0; i < this.typeUpgradeSpecial.Count; i++)
        {
            if (this.typeUpgradeSpecial[i] == UpgradeObj_Special.Type.UPGRADING)
            {
                if (this.timeUpgradeSpecial[i] > a)
                {
                    this.timeUpgradeSpecial[i] -= a;
                }
                else
                {
                    this.timeUpgradeSpecial[i] = 0;
                }
            }
        }
    }

    void GetInfo()
    {
        if (UIManager.Instance.isNewPlayer)
        {
            this.properties.level = 1;
            this.numberMine = 1;
            this.properties.capacity = GameConfig.Instance.lstPropertiesMap[ID].Productivity[this.properties.level - 1];
            this.properties.miningTime = GameConfig.Instance.lstPropertiesMap[ID].miningTime[this.properties.level - 1];
            for (int i = 0; i < GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special.Count; i++)
            {
                this.typeUpgradeSpecial.Add(UpgradeObj_Special.Type.NONE);
            }

            for (int i = 0; i < GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special.Count; i++)
            {
                this.timeUpgradeSpecial.Add(GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[i].time);
            }
            this.properties.buyMoreMinePrice = GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_1;
            this.state = StateMineShaft.LOCK;
            this.store.level = 1;
            this.store.value = 0;
            this.store.capacity = GameConfig.Instance.lstPropertiesMap[ID].Store_Capacity;
        }

        this.store.mineShaft = this;
        this.txtLevel.text = "Level " + this.properties.level.ToString();
        this.properties.speedMining = 1;
        xMoreMine = 1;
        txtX.text = "x" + xMoreMine;

        this.properties.name = GameConfig.Instance.lstPropertiesMap[ID].Name;
        this.txtName.text = this.properties.name;

        this.totalCapacity = this.properties.capacity * this.numberMine;
        this.properties.upgradeTime = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_time[this.properties.level - 1];
        GetPriceMoreMine();
        GetPriceUpgradeCost();
        this.properties.unitPrice = GameConfig.Instance.lstPropertiesMap[ID].Unit_Price[this.properties.level - 1];
        this.properties.unlockTime = GameConfig.Instance.lstPropertiesMap[ID].Unlock_time;
        this.properties.unlockCondition = GameConfig.Instance.lstPropertiesMap[ID].Unlock_condition;

        for (int i = 0; i < GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special.Count; i++)
        {
            this.lstUpgradeSpecial.Add(GameManager.Instance.lstUpgradeSpecial[i]);
        }

        this.timeUpgradeSpecial_Max = new int[GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special.Count];
        for (int i = 0; i < this.timeUpgradeSpecial_Max.Length; i++)
        {
            this.timeUpgradeSpecial_Max[i] = (int)GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[i].time;
        }

        this.unlockCost = new UnlockCost[2];
        this.unlockCost[0].type = TypeUnlock.COIN;
        this.unlockCost[0].cost = GameConfig.Instance.lstPropertiesMap[ID].Unlock_cost[0];
        this.unlockCost[1].type = TypeUnlock.GOLD;
        this.unlockCost[1].cost = GameConfig.Instance.lstPropertiesMap[ID].Unlock_cost[1];
        txtUnlock_byCoin.text = UIManager.Instance.ToLongString(this.unlockCost[0].cost);
        txtUnlock_byGold.text = UIManager.Instance.ToLongString(this.unlockCost[1].cost) + "$";
    }

    double _temp;
    double _temp_2;
    void GetPriceMoreMine()
    {
        _temp = _temp_2 = 0;
        pricePreMine = this.properties.buyMoreMinePrice;
        for (int i = 0; i < xMoreMine; i++)
        {
            double t = pricePreMine * GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_3;
            if (t - (long)t > 0.5f)
            {
                t += 1;
            }
            _temp_2 = (long)(GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + t);
            _temp += _temp_2;
            pricePreMine = (long)_temp_2;
        }

        btnBuyMoreMine.thisPrice = (long)_temp;
        btnBuyMoreMine.type = MyButton.Type.GOLD;
        txtMoreMinePrice.text = UIManager.Instance.ToLongString(btnBuyMoreMine.thisPrice);
        GameManager.Instance.AddGold(0);
    }

    void GetPriceUpgradeCost()
    {
        if (this.properties.level == 1)
        {
            this.properties.upgradePrice = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_cost[this.properties.level - 1] * (GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + (this.ID + 1) * 1 * 1);
        }
        else if (this.numberMine == 2)
        {
            this.properties.upgradePrice = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_cost[this.properties.level - 1] * (GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + (this.ID + 1) * 2 * 11);
        }
        else if (this.numberMine == 3)
        {
            this.properties.upgradePrice = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_cost[this.properties.level - 1] * (GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + (this.ID + 1) * 3 * 21);
        }
        else if (this.numberMine == 4)
        {
            this.properties.upgradePrice = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_cost[this.properties.level - 1] * (GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + (this.ID + 1) * 4 * 51);
        }
        else if (this.numberMine == 5)
        {
            this.properties.upgradePrice = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_cost[this.properties.level - 1] * (GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + (this.ID + 1) * 5 * 101);
        }
        else if (this.numberMine == 6)
        {
            this.properties.upgradePrice = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_cost[this.properties.level - 1] * (GameConfig.Instance.lstPropertiesMap[ID].MoreMine_cost_2 + (this.ID + 1) * 6 * 201);
        }
    }

    #region === WORK ===

    public void LetWork()
    {
        StartCoroutine(Work());
    }
    public IEnumerator Work()
    {
        state = StateMineShaft.WORKING;
        isCanWork = false;
        if (this.ID != 0)
        {
            if (this.totalCapacity <= this.preMineShaft.store.value)
            {
                this.input = this.totalCapacity;
            }
            else
            {
                this.input = this.preMineShaft.store.value;
            }
            this.preMineShaft.store.value -= this.input;
        }
        if (timer == 0)
        {
            timer = this.properties.miningTime;
        }
        if (workAnim != null)
            workAnim.enabled = true;
        if (timer >= 1)
        {
            //diễn anim working
            while (timer > 0)
            {
                yield return new WaitForSeconds(1f);
                timer--;
            }
        }
        numberProduct_Completed = this.input;
        product_Complete.SetActive(true);
        yield return new WaitForEndOfFrame();
        txtProduct_Complete.text = "";// numberProduct_Completed.ToString();
        while (product_Complete.GetComponent<RectTransform>().transform.position.y < this.posProduct_Complete.position.y)
        {
            product_Complete.GetComponent<RectTransform>().transform.Translate(Vector3.up * Time.deltaTime * this.properties.speedMining * 3f);
            yield return null;
        }

        product_Complete.SetActive(false);
        product_Complete.GetComponent<RectTransform>().transform.position = posProduct_Complete_1.position;
        //chạy xong
        if (nextMineShaft != null && nextMineShaft.state != StateMineShaft.LOCK && nextMineShaft.state != StateMineShaft.UNLOCKING && nextMineShaft.isActiveAndEnabled && this.store.capacity - this.store.value > 0)
        {
            if (numberProduct_Completed <= this.store.capacity - this.store.value)// this.nextMineShaft.totalCapacity)
            {
                numberProduct_PushUp = numberProduct_Completed;
                Debug.Log("Chỉ chuyển lên next mine");
                product_PushUp.SetActive(true);
                txtProduct_PushUp.text = numberProduct_PushUp.ToString();
                txtProduct_Remain.text = "";
                yield return new WaitForSeconds(0.15f);
                if (pushAnim != null)
                {
                    pushAnim.enabled = true;
                    pushAnim.Play("LightPushUp");
                }
                product_PushUp.transform.DOLocalPath(new Vector3[] { posProduct_PushUp.localPosition }, 0.5f * this.properties.speedMining).OnComplete(() =>
                {
                    product_PushUp.SetActive(false);
                    product_PushUp.transform.position = posProduct_Complete.position;
                    this.store.value += numberProduct_PushUp;
                    //nextMineShaft.GiveInput(numberProduct_PushUp);
                });
                //StartCoroutine(Product_Move_PushUp());
            }
            else
            {
                //numberProduct_PushUp = this.nextMineShaft.totalCapacity;
                numberProduct_PushUp = this.store.capacity - this.store.value;
                numberProduct_Remain = numberProduct_Completed - numberProduct_PushUp;
                Debug.Log("Chia hàng chuyển 2 đg");
                product_PushUp.SetActive(true);
                product_Remain.SetActive(true);
                txtProduct_PushUp.text = numberProduct_PushUp.ToString();
                txtProduct_Remain.text = numberProduct_Remain.ToString();
                yield return new WaitForSeconds(0.15f);
                if (pushAnim != null)
                {
                    pushAnim.enabled = true;
                    pushAnim.Play("LightPushUp");
                }

                product_PushUp.transform.DOLocalPath(new Vector3[] { posProduct_PushUp.localPosition }, 0.5f * this.properties.speedMining).OnComplete(() =>
                {
                    if (pushAnim != null)
                        pushAnim.enabled = true;
                    product_PushUp.SetActive(false);
                    product_PushUp.transform.position = posProduct_Complete.position;
                    this.store.value += numberProduct_PushUp;
                    //nextMineShaft.GiveInput(numberProduct_PushUp);
                });
                product_Remain.transform.DOLocalPath(new Vector3[] { posProduct_Remain.localPosition }, 0.5f * this.properties.speedMining).OnComplete(() =>
                {
                    product_Remain.SetActive(false);
                    product_Remain.transform.position = posProduct_Complete.position;
                    mapParent.AddProduct(numberProduct_Remain, (long)(numberProduct_Remain * this.properties.unitPrice));
                });
                //StartCoroutine(Product_Move_PushUp());
                //StartCoroutine(Product_Move_Remain());
            }
        }
        else
        {
            Debug.Log("Chỉ chuyển lên top");
            numberProduct_Remain = numberProduct_Completed;
            product_Remain.SetActive(true);
            txtProduct_Remain.text = numberProduct_Remain.ToString();
            yield return new WaitForSeconds(0.15f);
            product_Remain.transform.DOLocalPath(new Vector3[] { posProduct_Remain.localPosition }, 0.5f * this.properties.speedMining).OnComplete(() =>
            {
                product_Remain.SetActive(false);
                product_Remain.transform.position = posProduct_Complete.position;
                mapParent.AddProduct(numberProduct_Remain, (long)(numberProduct_Remain * this.properties.unitPrice));
            });
            //StartCoroutine(Product_Move_Remain());
        }
        //diễn anime chạy xong
        yield return new WaitForSeconds(0.75f * this.properties.speedMining);
        numberProduct_Completed = numberProduct_PushUp = numberProduct_Remain = 0;
        if (this.ID == 0)
        {
            this.input = this.totalCapacity;
            isCanWork = true;
        }
        else
        {
            if (this.totalCapacity <= this.preMineShaft.store.value)
            {
                this.input = this.totalCapacity;
            }
            else
            {
                this.input = this.preMineShaft.store.value;
            }
        }

        timer = 0;
        yield return new WaitForEndOfFrame();
        if (workAnim != null)
            workAnim.enabled = false;
        if (pushAnim != null)
        {
            pushAnim.enabled = false;
            pushAnim.gameObject.GetComponent<Image>().sprite = sprLight0;
        }
        txtProduct_Remain.text = "";
        state = StateMineShaft.IDLE;
        imgWorkBar.fillAmount = 0f;
        if (!isAutoWorking)
            btnWork.gameObject.SetActive(true);
        Debug.Log("Done");

    }

    //IEnumerator Product_Move_PushUp()
    //{
    //    //if (product_PushUp.activeSelf)
    //    //{
    //    while (product_PushUp.GetComponent<RectTransform>().transform.position.x > this.posProduct_PushUp.position.x)
    //    {
    //        //product_PushUp.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(product_PushUp.GetComponent<RectTransform>().transform.position, this.posProduct_PushUp.position, Time.deltaTime * this.properties.speedMining);
    //        product_PushUp.GetComponent<RectTransform>().transform.Translate(Vector3.left * Time.deltaTime * this.properties.speedMining);
    //        yield return null;
    //    }

    //    yield return new WaitForEndOfFrame();
    //    product_PushUp.SetActive(false);
    //    product_PushUp.transform.position = posProduct_Complete.position;
    //    nextMineShaft.GiveInput(numberProduct_PushUp);
    //    //}
    //}

    //IEnumerator Product_Move_Remain()
    //{
    //    //if (product_Remain.activeSelf)
    //    //{
    //    while (product_Remain.GetComponent<RectTransform>().transform.position.x < this.posProduct_Remain.position.x)
    //    {
    //        //product_Remain.GetComponent<RectTransform>().transform.position = Vector3.MoveTowards(product_Remain.GetComponent<RectTransform>().transform.position, this.posProduct_Remain.position, Time.deltaTime * this.properties.speedMining);
    //        product_Remain.GetComponent<RectTransform>().transform.Translate(Vector3.right * Time.deltaTime * this.properties.speedMining);
    //        yield return null;
    //    }
    //    yield return new WaitForEndOfFrame();
    //    product_Remain.SetActive(false);
    //    product_Remain.transform.position = posProduct_Complete.position;
    //    mapParent.AddProduct(numberProduct_Remain, (long)(numberProduct_Remain * this.properties.unitPrice));
    //    //}
    //}

    //public void GiveInput(int _input)
    //{
    //    if (this.ID != 0)
    //    {
    //        input = _input;
    //        isCanWork = true;
    //    }
    //}

    public void Btn_Work()
    {
        if (state == StateMineShaft.LOCK)
            return;

        LetWork();
    }

    #endregion

    #region === UPGRADE ===

    public void BuySpecialUpgrade(int _id)
    {

        if (this.lstUpgradeSpecial[_id].type == UpgradeObj_Special.Type.NONE)
        {
            GameManager.Instance.AddGold(-GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[_id].price);
            this.lstUpgradeSpecial[_id].type = UpgradeObj_Special.Type.UPGRADING;
            this.typeUpgradeSpecial[_id] = UpgradeObj_Special.Type.UPGRADING;
        }
    }

    void SpecialUpgrade_Complete_1(int _id)
    {
        if (this.lstUpgradeSpecial[_id].type == UpgradeObj_Special.Type.UPGRADING)
        {
            SpecialUpgrade_Complete_2(_id);
            this.lstUpgradeSpecial[_id].type = UpgradeObj_Special.Type.UPGRADED;
            this.typeUpgradeSpecial[_id] = UpgradeObj_Special.Type.UPGRADED;
            this.lstUpgradeSpecial[_id].SetBought();
        }

    }

    void SpecialUpgrade_Complete_2(int _id)
    {
        switch (_id)
        {
            case 0:
                this.isAutoWorking = true;
                imgAI.SetActive(true);
                break;
            case 1:
                this.properties.unitPrice *= 2;
                break;
            case 2:
                this.properties.unitPrice *= 2;
                break;
            case 3:
                this.properties.capacity *= 2;
                break;
            default:
                break;
        }
    }

    void Btn_ShowUpgrade()
    {
        UIManager.Instance.SetActivePanel(UIManager.Instance.panelShowUpgrade);
        AudioManager.Instance.Play("Click");
        GameManager.Instance.upgradeLevel.SetInfo(this, typeUpgradeLevel);
        for (int i = 0; i < this.lstUpgradeSpecial.Count; i++)
        {
            int _coin = (int)((timeUpgradeSpecial[i] * GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[i].coinMax) / timeUpgradeSpecial_Max[i]);
            GameManager.Instance.lstUpgradeSpecial[i].SetInfo(
                i,
                this,
                GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[i].name,
                GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[i].description,
                GameConfig.Instance.lstPropertiesMap[ID].Upgrade_Special[i].price,
                this.typeUpgradeSpecial[i],
                _coin
                );
        }
    }



    public void Btn_UpgradeLevel()
    {
        if (typeUpgradeLevel == UpgradeObj_Level.Type.NONE)
        {
            GameManager.Instance.AddGold(-this.properties.upgradePrice);
            timeUpgradeLevel = GameConfig.Instance.lstPropertiesMap[ID].Upgrade_time[this.properties.level - 1];
            typeUpgradeLevel = UpgradeObj_Level.Type.UPGRADING;
            GameManager.Instance.upgradeLevel.Upgrading();
        }
    }

    public void UpgradeLevel_Coin()
    {
        GameManager.Instance.AddCoin(-10);
        UpgradeLevel_Complete();
    }

    private void UpgradeLevel_Complete()
    {
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelCoinAds);
        this.properties.level += 1;
        txtLevel.text = "Level " + this.properties.level;
        GetPriceUpgradeCost();
        this.properties.capacity = GameConfig.Instance.lstPropertiesMap[ID].Productivity[this.properties.level - 1];
        this.properties.miningTime = GameConfig.Instance.lstPropertiesMap[ID].miningTime[this.properties.level - 1];
        this.properties.unitPrice = GameConfig.Instance.lstPropertiesMap[ID].Unit_Price[this.properties.level - 1];
        typeUpgradeLevel = UpgradeObj_Level.Type.NONE;

        GameManager.Instance.upgradeLevel.SetInfo(this, typeUpgradeLevel);
    }

    public void UpgradeAds(int _id)
    {
        this.timeUpgradeSpecial[_id] /= 3;
    }

    public void UpgradeCoin(int _id, int _coin)
    {
        this.timeUpgradeSpecial[_id] = 0;
        GameManager.Instance.AddCoin(-_coin);
    }

    #endregion

    #region === MORE MINE  ===
    public void Btn_X()
    {
        if (xMoreMine == 1)
        {
            xMoreMine = 10;
        }
        else if (xMoreMine == 10)
        {
            xMoreMine = 1;
        }
        txtX.text = "x" + xMoreMine;
        GetPriceMoreMine();
    }
    public void Btn_BuyMoreMine()
    {
        AudioManager.Instance.Play("Click");
        GameManager.Instance.AddGold(-this.btnBuyMoreMine.thisPrice);
        BuyMoreMineComplete();
    }


    void BuyMoreMineComplete()
    {
        this.numberMine += xMoreMine;
        this.properties.buyMoreMinePrice = (long)pricePreMine;
        GetPriceMoreMine();
        mapParent.CheckUnlock(this.ID);
        this.totalCapacity = this.properties.capacity * numberMine;
        if (this.ID == 0)
            this.input = this.totalCapacity;
    }
    #endregion

    #region === UNLOCK ===
    public void Btn_Unlock(TypeUnlock _type)
    {
        //diễn anim unlock
        if (_type == TypeUnlock.GOLD)
        {
            GameManager.Instance.AddGold(-this.unlockCost[1].cost);
            this.btnUnlock_byGold.interactable = false;
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
        mapParent.CheckUnlock(this.ID);
        UIManager.Instance.SetDeActivePanel(panelUnlock);
    }
    #endregion

    #region === STORE ===
    int capStoreWillUp;
    void Btn_ShowUpgrade_Store()
    {
        if (nextMineShaft != null && nextMineShaft.state != StateMineShaft.LOCK && nextMineShaft.state != StateMineShaft.UNLOCKING)
        {
            GetStoreCost();
            GetSoreCapacity();
            UIManager.Instance.SetActivePanel(UIManager.Instance.panelUpgradeStore);
            UIManager.Instance.btnUpStore.thisButton.onClick.RemoveAllListeners();
            UIManager.Instance.txtNameStore.text = "Store Machine " + (this.ID + 1);
            UIManager.Instance.txtLevelStore.text = this.store.level.ToString();
            UIManager.Instance.txtLevelStores_Up.text = (this.store.level + 1).ToString();
            UIManager.Instance.txtCapStore.text = UIManager.Instance.ToLongString(this.store.capacity);
            UIManager.Instance.txtCapStore_Up.text = UIManager.Instance.ToLongString(this.capStoreWillUp);
            UIManager.Instance.btnUpStore.thisPrice = this.store.cost;
            UIManager.Instance.txtPriceStore.text = "Upgrade\n" + UIManager.Instance.ToLongString(this.store.cost);
            UIManager.Instance.btnUpStore.type = MyButton.Type.GOLD;
            UIManager.Instance.btnUpStore.thisButton.onClick.AddListener(() => Upgrade_Store());
        }
    }

    void Upgrade_Store()
    {
        GameManager.Instance.AddGold(-this.store.cost);
        this.store.level += 1;
        this.store.capacity = capStoreWillUp;;
        Btn_ShowUpgrade_Store();
    }

    
    void GetStoreCost()
    {
        this.store.cost = 100;
    }

    void GetSoreCapacity()
    {
        capStoreWillUp = this.store.capacity + 10;
    }
    #endregion
}
