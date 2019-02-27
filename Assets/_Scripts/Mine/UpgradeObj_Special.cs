using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class UpgradeObj_Special : MonoBehaviour
{
    public int ID;
    public bool isUpgrading = false;
    public float timeUpgrading;
    public Text txtNameAI;
    public Text txtDescriptionAI;
    public Text txtName;
    public Text txtLevel;
    public Text txtCapacity;
    public Text txtMiningTime;
    public Text txtUnitPrice;
    public Text txtPriceUpgrade;
    public Text txtPriceAI;
    public MyButton btnBuyAI;
    public MyButton btnUpgrade;
    public Text txtTimeUpgrading;

    [Header("UP FAST")]
    public Text txtPriceFast;
    public MyButton btnCoinfast;
    public MyButton btnAdsFast;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    string transformToTime(float time = 0)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetInfoUpgrade(string _nameAI, string _DescriptionAI, string _name, string _level, string _capacity, string _miningTime, string _unitPrice, long _priceAI, long _priceUp, UnityEngine.Events.UnityAction _actionBtnAI, UnityEngine.Events.UnityAction _actionBtnUp)
    {
        if (!UIManager.Instance.panelShowUpgrade.activeSelf)
        {
            UIManager.Instance.SetActivePanel(UIManager.Instance.panelShowUpgrade);
        }
        else
        {
            UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelShowUpgrade);
        }

        txtNameAI.text = _nameAI;
        txtDescriptionAI.text = _DescriptionAI;
        txtName.text = _name;
        txtLevel.text = _level;
        txtCapacity.text = _capacity;
        txtMiningTime.text = _miningTime;
        txtUnitPrice.text = _unitPrice;

        btnBuyAI.thisPrice = _priceAI;
        btnUpgrade.thisPrice = _priceUp;
        btnBuyAI.thisButton.onClick.AddListener(_actionBtnAI);
        btnUpgrade.thisButton.onClick.AddListener(_actionBtnUp);
    }

    public void SetUpgrading(int _time,UnityEngine.Events.UnityAction _actionBtnUp)
    {
        btnUpgrade.thisButton.onClick.RemoveAllListeners();
        timeUpgrading = _time;
        isUpgrading = true;
        btnUpgrade.thisButton.onClick.AddListener(_actionBtnUp);
    }

    public void SetUpgradeFast(long _price, UnityEngine.Events.UnityAction _actionAds, UnityEngine.Events.UnityAction _actionCoin)
    {
        btnUpgrade.thisButton.onClick.RemoveAllListeners();
        txtPriceFast.text = _price.ToString();
        btnAdsFast.thisButton.onClick.AddListener(_actionAds);
        btnCoinfast.thisButton.onClick.AddListener(_actionCoin);
    }
}
