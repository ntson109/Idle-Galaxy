using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class RDObj : MonoBehaviour
{
    public int ID;
    public Text txtTittle;
    public Text txtDescription;
    public Text txtPrice;
    public Button thisButton;
    public long price;
    public bool isOver = false;

    private void Start()
    {
        this.RegisterListener(EventID.CHANGE_GOLD_COIN, (param) => ON_CHANGE_GOLD_COIN());
    }

    void ON_CHANGE_GOLD_COIN()
    {
        if (isOver)
            return;
        if (GameManager.Instance.GOLD >= price)
        {
            thisButton.interactable = true;
        }
        else
        {
            thisButton.interactable = false;
        }
    }

    public void SetInfo(string _tittle, string _des, long _price, UnityEngine.Events.UnityAction _action)
    {
        txtTittle.text = _tittle;
        txtDescription.text = _des;
        price = _price;
        txtPrice.text = UIManager.Instance.ToLongString(_price);
        thisButton.onClick.AddListener(_action);
    }

    public void SetOver(string _tittle, string _des)
    {
        txtTittle.text = _tittle;
        txtDescription.text = _des;
        thisButton.interactable = false;
        txtPrice.text = "";
        isOver = true;
    }
}
