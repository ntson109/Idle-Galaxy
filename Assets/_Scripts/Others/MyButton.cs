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
    //private long m_thisPrice;
    public long thisPrice;
    //{
    //        get {
    //            return m_thisPrice;
    //        }
    //        set{
    //            m_thisPrice = value;
    //            ON_CHANGE_GOLD_COIN();
    //        }
    //    }

    public Type type;

    void Start()
    {
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());
    }

    void ON_CHANGE_GOLD_COIN()
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
