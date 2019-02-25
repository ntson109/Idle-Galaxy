using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RDObj : MonoBehaviour {
    public Text txtTittle;
    public Text txtDescription;
    public Button thisButton;

    public void SetInfo(string _tittle, string _des, UnityEngine.Events.UnityAction _action)
    {
        txtTittle.text = _tittle;
        txtDescription.text = _des;
        thisButton.onClick.AddListener(_action);
    }
}
