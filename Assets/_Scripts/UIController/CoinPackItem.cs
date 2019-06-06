using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPackItem : MonoBehaviour
{
    // UIs.
    public Text txtValue, txtPrice;
    private CoinPack PackData;

    public void Init(int pack_index)
    {
        this.PackData = GameConfig.Instance.listCoinPacks[pack_index];
        this.txtValue.text = this.PackData.value.ToString();
        this.txtPrice.text = this.PackData.price;
    }

    public void OnBuyClick()
    {

    }
}
