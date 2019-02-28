using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class Map : MonoBehaviour
{
    public TypeMap type;
    public List<MineShaft> lstMineShaft;
    public GameObject spaceShip;
    public Transporter transporter;
    long totalAmount;
    long totalMoney;
    public long moneyPerTurn;
    public GameObject rdPrefabs;

    [Header("UI")]
    public Text txtAmountProduct;
    public Scrollbar scrollbarVertical;
    void Start()
    {
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
    }
    void Update()
    {
        if (GameManager.Instance.stateGame == StateGame.PLAYING)
        {
            if (!transporter.isTransporting)
                CheckFullWareHouse();
        }

        txtAmountProduct.text = totalAmount.ToString();
    }

    void ON_START_GAME()
    {
        if (this.type == TypeMap.MOON)
        {
            this.transporter.capacity = GameConfig.Instance.lstCapTransporter[0];
        }
        this.transporter.speed = GameConfig.Instance.SpeedTransporter;
        scrollbarVertical.value = 0;
    }

    public void AddProduct(long _amountProduct, long _value)
    {
        totalAmount += _amountProduct;
        totalMoney += _value;
        CheckFullWareHouse();
    }

    void CheckFullWareHouse()
    {
        if (totalAmount >= transporter.capacity)
        {
            transporter.Transport();
            moneyPerTurn = (totalMoney * transporter.capacity) / totalAmount;
            totalMoney -= moneyPerTurn;
            totalAmount -= transporter.capacity;
        }
    }

    public void CompleteTransport()
    {
        GameManager.Instance.AddGold(moneyPerTurn);
        moneyPerTurn = 0;
    }

    public void CheckUnlock(int _id)
    {
        if (_id + 1 >= lstMineShaft.Count)
            return;
        if (lstMineShaft[_id + 1].state == MineShaft.StateMineShaft.LOCK)
        {
            if (lstMineShaft[_id].numberMine >= lstMineShaft[_id + 1].properties.unlockCondition)
            {
                UIManager.Instance.SetActivePanel(lstMineShaft[_id + 1].panelUnlock);
                UIManager.Instance.SetDeActivePanel(lstMineShaft[_id + 1].panelUnlock_Condition);
            }
            else
            {
                UIManager.Instance.SetActivePanel(lstMineShaft[_id + 1].panelUnlock_Condition);
            }
        }
    }
}

[System.Serializable]
public enum TypeMap
{
    MOON,
    EARTH
}
