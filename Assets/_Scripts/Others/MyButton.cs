using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using EventDispatcher;

[RequireComponent(typeof(Button))]
public class MyButton : MonoBehaviour
{
    public Button PriceButton;
    public Text txtPrice;
    public long PriceValue, MaxPriceValue;
    public PriceType Type;
    public float Time, MaxTime;

    void Awake()
    {
        /*this.PriceButton = this.GetComponent<Button>();
        this.txtPrice = this.GetComponentInChildren<Text>();*/
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => CheckEnoughGemCoin());
        this.CheckEnoughGemCoin();
    }

    public void Init(PriceType price_type, long price_value, UnityAction on_click)
    {
        this.Type = price_type;
        this.SetPrice(price_value);
        this.PriceButton.onClick.RemoveAllListeners();
        this.PriceButton.onClick.AddListener(() => this.ProcessBuy());
        this.PriceButton.onClick.AddListener(on_click);
    }

    public void Init(PriceType price_type, long price_value, float maxTime, float currentTime, UnityAction on_click)
    {
        this.Type = price_type;
        this.SetPrice(price_value);
        this.MaxPriceValue = price_value;
        this.Time = currentTime;
        this.MaxTime = maxTime;
        this.PriceButton.onClick.RemoveAllListeners();
        this.PriceButton.onClick.AddListener(() => this.ProcessBuy());
        this.PriceButton.onClick.AddListener(on_click);
    }

    private void Update()
    {
        if (this.Time > 0)
        {
            this.Time -= UnityEngine.Time.deltaTime;
            var price_by_time = (long)(this.MaxPriceValue / this.MaxTime * this.Time);
            this.SetPrice(price_by_time);
        }
    }

    private void SetPrice(long price)
    {
        this.PriceValue = price;
        if (this.txtPrice) this.txtPrice.text = UIManager.Instance.ToLongString(price);
    }

    private void ProcessBuy()
    {
        if (this.Type == PriceType.GOLD)
        {
            GameManager.Instance.AddGold(-this.PriceValue);
        }
        else if (this.Type == PriceType.COIN)
        {
            GameManager.Instance.AddCoin(-this.PriceValue);
        }
        this.CheckEnoughGemCoin();
    }

    void CheckEnoughGemCoin()
    {
        if (this == null) return;
        long current_value = 0;
        if (this.Type == PriceType.GOLD)
        {
            current_value = GameManager.Instance.GOLD;
        }
        else if (this.Type == PriceType.COIN)
        {
            current_value = GameManager.Instance.COIN;
        }
        this.PriceButton.interactable = current_value >= this.PriceValue;
    }
}

public enum PriceType
{
    GOLD,
    COIN
}