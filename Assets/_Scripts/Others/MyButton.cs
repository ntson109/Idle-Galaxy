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
    public long thisPrice = 0;
    public Type type;

    void Start()
    {
        thisButton = GetComponent<Button>();
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());

    }

    void ON_CHANGE_GOLD_COIN()
    {
        if (type == Type.GOLD)
        {
            if (GameManager.Instance.GOLD >= thisPrice)
            {
                thisButton.interactable = true;
            }
            else
            {
                thisButton.interactable = false;
            }
        }
        else if (type == Type.COIN)
        {
            if (GameManager.Instance.COIN >= thisPrice)
            {
                thisButton.interactable = true;
            }
            else
            {
                thisButton.interactable = false;
            }
        }
    }
}
