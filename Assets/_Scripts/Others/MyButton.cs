using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

[RequireComponent(typeof(Button))]
public class MyButton : MonoBehaviour
{
    public Button thisButton;
    public long thisPrice = 0;

    void Start()
    {
        thisButton = GetComponent<Button>();
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
    }

    void ON_START_GAME()
    {
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());
    }

    void ON_CHANGE_GOLD_COIN()
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
}
