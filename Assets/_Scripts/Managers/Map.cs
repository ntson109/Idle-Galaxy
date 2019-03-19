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
    public long totalAmount;
    public long totalMoney;
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
        if (UIManager.Instance.isNewPlayer)
        {
            this.transporter.SetInfo(1, GameConfig.Instance.Capacity_1, 5);           
        }
        else
        {
            for (int i = 0; i < lstMineShaft.Count; i++)
            {
                if (lstMineShaft[i].state == MineShaft.StateMineShaft.WORKING)
                {
                    lstMineShaft[i].LetWork();
                    lstMineShaft[i].imgWorkBar.fillAmount = (float)lstMineShaft[i].timer / (float)lstMineShaft[i].properties.miningTime;
                }
            }
        }

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
            moneyPerTurn = (totalMoney * transporter.capacity) / totalAmount;
            totalMoney -= moneyPerTurn;
            totalAmount -= transporter.capacity;
            transporter.Transport();
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
