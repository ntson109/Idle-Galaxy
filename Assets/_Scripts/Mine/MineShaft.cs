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

        public long capacity;

        public float miningTime;

        public long unitPrice;

        public float unlockTime;

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

    public enum TypeUnlock
    {
        GOLD,
        COIN,
        AD
    }

    public enum TypeProduct
    {
        GOLD,
        SP1,
        SP2,
        SP3,
        SP4
    }

    public enum StateMineShaft
    {
        NONE,
        WORKING
    }
    #endregion

    public MineShaft.Properties properties; //thông số cơ bản
    public Miner miner; //nhân viên
    public UnlockCost unlockCost; 
    public TypeProduct typeProduct; //loại sản phẩm
    public StateMineShaft state; //trạng thái
    public bool isAutoWorking; 
    public MineShaft nextMineShaft; //mỏ tiếp theo
    public long numberProduct_Completed; //số sản phẩm làm xong
    public long numberProduct_PushUp; //số sản phẩm đẩy lên mỏ trên
    public long numberProduct_Remain; //số sản phẩm thừa
    public int timer = 0; //thời gian đang chạy tiến trình Work

    [Header("UI")]
    public Text txtTimer;
    public Text txtTimeMining;
    public Button btnUpgrade;

    #region === START VS UPDATE ===
    void Start()
    {
        this.RegisterListener(EventID.START_GAME,(param) => ON_START_GAME());       
    }

    void LateUpdate()
    {
        if (isAutoWorking && this.state == StateMineShaft.NONE)
        {
            StartCoroutine(Work());
        }
    }
    #endregion

    void ON_START_GAME()
    {
        btnUpgrade.onClick.AddListener(() => Btn_Upgrade());
    }

    public void Reset()
    {
        this.properties.Reset();
        this.state = StateMineShaft.NONE;
    }

    public IEnumerator Work()
    {
        state = StateMineShaft.WORKING;
        //diễn anim working
        while(timer < this.properties.miningTime)
        {
            Debug.Log("Working");
            txtTimer.text = UIManager.Instance.ToDateTimeString(timer);
            yield return new WaitForSeconds(1f);
            timer++;
        }
        //chạy xong
        numberProduct_Completed = this.properties.capacity;
        if (numberProduct_Completed <= this.nextMineShaft.properties.capacity)
        {
            numberProduct_PushUp = this.nextMineShaft.properties.capacity;
            Debug.Log("Chỉ chuyển lên");
        }
        else
        {
            numberProduct_PushUp = this.nextMineShaft.properties.capacity;
            numberProduct_Remain = numberProduct_Completed - this.nextMineShaft.properties.capacity;
            Debug.Log("Chia hàng chuyển 2 đg");
        }
        //diễn anime chạy xong
        yield return new WaitForSeconds(1f);
        nextMineShaft.GiveInput(numberProduct_PushUp);
        numberProduct_Completed = numberProduct_PushUp = numberProduct_Remain = 0;
        state = StateMineShaft.NONE;
    }

    public void GiveInput(long _input)
    {
        if (this.properties.ID != 1)
        {

        }
    }

    void Btn_Upgrade()
    {

    }
}
