using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public TypeMap type;
    public List<MineShaft> lstMineShaft;
    public WareHouse wareHouse = new WareHouse();
    public GameObject spaceShip;
    public Transform posBeginTransport;
    public Transform posEndTransport;
    bool isFull;

    [Header("UI")]
    public Text txtAmountProduct;
    void Start()
    {

    }
    void Update()
    {

    }

    public void AddProduct(MineShaft.TypeProduct _type, int _value)
    {
        for (int i = 0; i < wareHouse.lstProduct.Length; i++)
        {
            if (wareHouse.lstProduct[i].type == _type)
            {
                wareHouse.lstProduct[i].amount += _value;
                wareHouse.totalValue += _value;
                CheckFullWareHouse();
            }
        }
    }

    void CheckFullWareHouse()
    {
        //if(wareHouse.totalValue)
    }
}

[System.Serializable]
public struct WareHouse
{
    public MineShaft.Product[] lstProduct;
    public int totalValue;
}

[System.Serializable]
public enum TypeMap
{
    MOON,
    EARTH
}
