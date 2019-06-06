using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPackItem : MonoBehaviour
{
    // UIs.
    public Text txtValue, txtPrice;
    private GoldPack PackData;

    public void Init(int pack_index)
    {
        this.PackData = GameConfig.Instance.listGoldPacks[pack_index];
        this.txtValue.text = this.PackData.value.ToString();
        this.txtPrice.text = this.PackData.price.ToString();
    }

    public void UpdateState()
    {
        this.GetComponentInChildren<Button>().interactable = GameManager.Instance.COIN >= this.PackData.price;
    }

    public void OnBuyClick()
    {
        if (this.PackData.price > GameManager.Instance.COIN)
        {
            return;
        }
            
        GameManager.Instance.AddCoin(-this.PackData.price);
        GameManager.Instance.AddGold(this.PackData.value);
        this.UpdateState();
    }
}
