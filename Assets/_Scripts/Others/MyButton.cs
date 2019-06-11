using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

[RequireComponent(typeof(Button))]
public class MyButton : MonoBehaviour
{
    public enum Type
    {
        GOLD,
        COIN
    }

    public Button thisButton;
    public long thisPrice;
    public Type type;

    void Awake()
    {
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => CheckEnoughGemCoin());
        this.CheckEnoughGemCoin();
    }

    void CheckEnoughGemCoin()
    {
        if (type == Type.GOLD)
        {
            if (GameManager.Instance.GOLD >= thisPrice)
            {
                if (thisButton != null)
                {
                    thisButton.interactable = true;
                }
            }
            else
            {
                if (thisButton != null)
                {
                    thisButton.interactable = false;
                }
            }
        }
        else if (type == Type.COIN)
        {
            if (GameManager.Instance.COIN >= thisPrice)
            {
                if (thisButton != null)
                {
                    thisButton.interactable = true;
                }
            }
            else
            {
                if (thisButton != null)
                {
                    thisButton.interactable = false;
                }
            }
        }
    }
}
