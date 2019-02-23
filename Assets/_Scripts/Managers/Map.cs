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

    [Header("UI")]
    public Text txtAmountProduct;
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
    }

    void ON_START_GAME()
    {
        if (this.type == TypeMap.MOON)
        {
            this.transporter.capacity = GameConfig.Instance.lstCapTransporter[0];
        }
    }

    public void AddProduct(long _amountProduct, long _value)
    {
        totalAmount += _amountProduct;
        totalMoney += _value;
        CheckFullWareHouse();
        txtAmountProduct.text = totalAmount.ToString();
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
}

[System.Serializable]
public struct WareHouse
{
    public MineShaft.Product[] lstProduct;
}

[System.Serializable]
public enum TypeMap
{
    MOON,
    EARTH
}
