using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = new UIManager();
    public bool isNewPlayer = true;
    public float timeVideo;
    public bool isCanClickVideo;

    [HideInInspector]
    public List<string> arrAlphabetNeed = new List<string>();
    private string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    public Text txtGold;
    public Text txtCoin;

    [Header("UI HOME")]
    public GameObject panelYesNoNewPlay;

    [Header("UI MAIN")]
    public Button btnContinue;
    public GameObject panelShowUpgrade;
    public GameObject panelCoinAds;
    public Text txtCoin_panelCoinAds;
    public MyButton btnCoin_panelCoinAds;
    public Text txtBoost;
    public Text txtTimeBoost;
    public Sprite[] sprMoreMine;
    public Image imgXMine;

    [Header("TRANSPORTER")]
    public GameObject panelUpgradeTransporter;
    public MyButton btnUpTrans;
    public Text txtNameTrans;
    public Text txtLevelTrans;
    public Text txtLevelTrans_Up;
    public Text txtCapTrans;
    public Text txtCapTrans_Up;
    public Text txtTimeTrans;
    public Text txtTimeTrans_Up;
    public Text txtPriceTrans;

    [Header("STORE")]
    public GameObject panelUpgradeStore;
    public MyButton btnUpStore;
    public Text txtNameStore;
    public Text txtLevelStore;
    public Text txtLevelStores_Up;
    public Text txtCapStore;
    public Text txtCapStore_Up;
    public Text txtPriceStore;

    [Header("UFO")]
    public GameObject panelUFO;
    public GameObject panelUFO_Gold;
    public GameObject panelUFO_CoinVideo;
    public Text txtGold_UFO;
    public Text txtCoin_UFO;
    public Image imgRewardUFO;
    public Text txtReward_UFO;
    public Sprite[] lstSprReward;

    [Header("OFFLINE")]
    public GameObject panelOffline;
    public Text txtTittleOffline;
    public Text txtGoldOffline;

    [Header("SPIN")]
    public GameObject panelSpin;
    public Text txtCountSpin;
    public Text txtCountSpinMain;
    public Button btnReceiveSpin;
    public GameObject panelReceiveSpin;
    public Sprite[] sprRewardSpin;
    public GameObject receiveSpin_normal;
    public Image imgRewardSpin;
    public Text txtRewardSpin;
    public GameObject receiveSpin_random;
    public Text txtRewardSpin_randomGold;
    public Text txtRewardSpin_randomCoin;

    [Header("VIDEO")]
    public Text txtTimeVideo;
    public GameObject panelVideo;
    public Text txtReward_Video;
    public Image imgRewardVideo;

    [Header("UNLOCK")]
    public GameObject panelUnlockReward;
    public Text txtTittleUnlock;
    public GameObject unlock_1;
    public GameObject unlock_2;
    public GameObject unlock_3;
    public Button btnOkUnlock;

    [Header("TUTORIAL")]
    public GameObject mainTutorial;
    public GameObject panelTutorial;
    public GameObject handTutorial;
    public GameObject[] lsButtonTutorial;

    //[Header("MOUSE CLICK")]
    //public GameObject mouseClick;
    //public Canvas parentCanvas;

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    #region === START VS UPDATE ===
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < arrAlphabet.Length; j++)
            {
                arrAlphabetNeed.Add(arrAlphabet[i] + arrAlphabet[j]);
            }
        }

        if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 1)
        {
            btnContinue.interactable = true;
        }
        else
        {
            btnContinue.interactable = false;
        }

        if (timeVideo > 0)
        {
            timeVideo -= Time.deltaTime;
            txtTimeVideo.text = transformToTime(timeVideo);
            if (isCanClickVideo)
                isCanClickVideo = false;
        }
        else
        {
            txtTimeVideo.text = "";
            if (!isCanClickVideo)
                isCanClickVideo = true;
        }
    }

    void Update()
    {
        if (GameManager.Instance.stateGame == StateGame.PLAYING)
        {
            HideIfClickedOutside(panelCoinAds);
            UIManager.Instance.txtCountSpinMain.text = "x" + GameManager.Instance.countSpin;
        }
    }
    #endregion

    #region === SUPPORT ===
    public string transformToTime(float time = 0)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public string ConvertNumber(long number)
    {
        string smoney = string.Format("{0:#,##0}", number);
        for (int i = 0; i < arrAlphabetNeed.Count; i++)
        {
            if (smoney.Length >= 5 + i * 4 && smoney.Length < 9 + i * 4)
            {
                if (smoney[smoney.Length - (3 + i * 4)] != '0')
                {
                    smoney = smoney.Substring(0, smoney.Length - (5 + i * 4 - 3));
                    smoney = smoney + arrAlphabetNeed[i];
                    if (i < 4)
                    {
                        smoney = smoney.Remove(smoney.Length - 3, 1);
                        smoney = smoney.Insert(smoney.Length - 2, ".");
                    }
                    else
                    {
                        smoney = smoney.Remove(smoney.Length - 4, 1);
                        smoney = smoney.Insert(smoney.Length - 3, ".");
                    }
                }
                else
                {
                    smoney = smoney.Substring(0, smoney.Length - (5 + i * 4 - 1));
                    smoney = smoney + arrAlphabetNeed[i];
                }
                return smoney;
            }
        }
        return smoney;
    }

    public void SetActivePanel(GameObject _g)
    {
        if (_g == null || _g.activeSelf)
            return;

        _g.SetActive(true);
        //if (_g.name == "InWall")
        //    _g.GetComponent<Animator>().Play("ActivePanel");
    }

    public void SetDeActivePanel(GameObject _g)
    {
        if (_g == null || !_g.activeSelf)
            return;

        _g.SetActive(false);
        //_g.GetComponent<Animator>().Play("DeActivePanel");
    }

    /// <summary>
    /// Hien chuot khi click man hinh
    /// </summary>
    void ShowMouseClick()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Vector2 click;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out click);
        //    Vector3 mousePos = parentCanvas.transform.TransformPoint(click) + new Vector3(0.2f, -0.3f, 0);
        //    mouseClick.transform.position = mousePos;
        //}
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            panel.SetActive(false);
        }
    }

    //================================================
    private string[] timeFormat = new string[]
	{
		"d",
		"h",
		"mins",
		"s"
	};

    private string[] cashFormat = new string[]
	{
		"K",
		"M",
		"B",
		"T"
	};
    public string ConvertCash(double cash)
    {
        if (cash < 1000.0)
        {
            return Math.Round(cash).ToString();
        }
        int num = 0;
        double num2 = 0.0;
        for (int i = 0; i < cashFormat.Length; i++)
        {
            num2 = cash / Math.Pow(1000.0, (double)(i + 1));
            if (num2 < 1000.0)
            {
                num2 = Math.Round(num2, (num2 >= 100.0) ? 0 : 1);
                num = i;
                break;
            }
        }
        return num2.ToString() + cashFormat[num];
    }

    //2h46m
    public string ConvertTime(int second)
    {
        int num = second / 86400;
        int num2 = second % 86400 / 3600;
        int num3 = second % 3600 / 60;
        int num4 = second % 60;
        if (num > 0)
        {
            return num.ToString() + timeFormat[0] + ((num2 <= 0) ? string.Empty : (num2.ToString() + timeFormat[1]));
        }
        if (num2 > 0)
        {
            return num2.ToString() + timeFormat[1] + ((num3 <= 0) ? string.Empty : (num3.ToString() + timeFormat[2]));
        }
        if (num3 > 0)
        {
            return num3.ToString() + timeFormat[2] + ((num4 <= 0) ? string.Empty : (num4.ToString() + timeFormat[3]));
        }
        return num4.ToString() + timeFormat[3];
    }

    //================================================
    //2:46:40s
    public string ToDateTimeString(int seconds)
    {
        int num = seconds / 3600;
        int num2 = seconds % 3600 / 60;
        int num3 = seconds % 60;
        if (num > 0)
        {
            return string.Format("{0}:{1:D2}:{2:D2}", num, num2, num3) + "s";
        }
        if (num2 > 0)
        {
            return num2.ToString() + ":" + num3.ToString("00") + "s";
            //eturn num2.ToString("00") + ":" + num3.ToString("00") + "s";
        }
        return num3.ToString() + " s";
        //return num3.ToString("00") + "s";
    }

    public string ToDoubleString(double pValue)
    {
        int num = -1;
        while (pValue >= 1000.0)
        {
            pValue /= 1000.0;
            num++;
        }
        string str = string.Empty;
        if (num >= 0)
        {
            MONEY_UNIT suffix = (MONEY_UNIT)num;
            str = suffix.ToString();
            return pValue.ToString("G4") + str;
        }
        return ((int)pValue).ToString();
    }

    public string ToLongString(double lValue)
    {
        int num = -1;
        while (lValue >= 1000)
        {
            lValue /= 1000;
            num++;
        }
        string str = string.Empty;
        if (num >= 0)
        {
            MONEY_UNIT suffix = (MONEY_UNIT)num;
            str = suffix.ToString();
            return lValue.ToString("G4") + str;
        }
        return ((int)lValue).ToString();
    }

    private enum MONEY_UNIT
    {
        K,
        M,
        B,
        T,
        Qa,
        Qu,
        Sxt,
        Spt,
        Oct,
        Non,
        Dec,
        aa,
        bb,
        cc,
        ee,
        ff,
        gg,
        hh,
        ii,
        kk,
        ll,
        mm,
        nn,
        oo,
        pp,
        qq,
        rr,
        ss,
        tt,
        uu,
        vv,
        ww,
        xx,
        yy,
        zz
    }

    //================================================
    public int GetOfflineTime(string dateTime)
    {
        if (dateTime == string.Empty)
        {
            return 0;
        }
        if ((int)Mathf.Round((float)DateTime.Now.Subtract(Convert.ToDateTime(dateTime)).TotalMinutes) <= int.MaxValue)
        {
            return (int)Mathf.Round((float)DateTime.Now.Subtract(Convert.ToDateTime(dateTime)).TotalMinutes);
        }
        else
        {
            return int.MaxValue;
        }
    }

    #endregion

    #region === UI HOME ===
    public void Btn_Play()
    {
        AudioManager.Instance.Play("Click");
        SetActivePanel(panelYesNoNewPlay);
        isNewPlayer = true;
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
            {
                this.PostEvent(EventID.START_GAME);
                GameManager.Instance.stateGame = StateGame.PLAYING;
                GameManager.Instance.AddGold(GameConfig.Instance.GoldStart);
                GameManager.Instance.AddCoin(GameConfig.Instance.CoinStart);
                AudioManager.Instance.Play("GamePlay", true);
            });
    }

    public void Btn_Yes_NewPlay()
    {
        AudioManager.Instance.Play("Click");
        isNewPlayer = true;
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
        {
            this.PostEvent(EventID.START_GAME);
            GameManager.Instance.stateGame = StateGame.PLAYING;
            GameManager.Instance.AddGold(GameConfig.Instance.GoldStart);
            GameManager.Instance.AddCoin(GameConfig.Instance.CoinStart);
            AudioManager.Instance.Play("GamePlay", true);
        });
    }

    public void Btn_No_NewPlay()
    {
        AudioManager.Instance.Play("Click");
        SetDeActivePanel(panelYesNoNewPlay);
    }

    public void Btn_Continue()
    {
        AudioManager.Instance.Play("Click");
        isNewPlayer = false;
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
        {
            GameManager.Instance.stateGame = StateGame.PLAYING;
            DataPlayer.Instance.LoadDataPlayer();
            AudioManager.Instance.Play("GamePlay", true);
        });
    }

    public void Btn_Share()
    {
        //DataPlayer.Instance.SaveDataPlayer();
    }

    public void Btn_Rate()
    {

    }
    #endregion

    #region === UI MAIN ===

    public void ShowPanelCoinAds(int _coin, UnityEngine.Events.UnityAction _action)
    {
        SetActivePanel(panelCoinAds);
        btnCoin_panelCoinAds.thisButton.onClick.RemoveAllListeners();
        btnCoin_panelCoinAds.thisPrice = _coin;
        txtCoin_panelCoinAds.text = btnCoin_panelCoinAds.thisPrice.ToString();
        btnCoin_panelCoinAds.type = MyButton.Type.COIN;
        btnCoin_panelCoinAds.thisButton.onClick.AddListener(_action);
    }

    #region === OFFLINE ===
    public int timeOffline;
    public long goldOffline;
    int xOffline = 1;

    public void ShowPanelOffline()
    {
        SetActivePanel(panelOffline);
        txtTittleOffline.text = "You are offline: \n" + ConvertTime(timeOffline * 60);
        txtGoldOffline.text = ToLongString(goldOffline);
    }
    public void Btn_ReceiveOffline()
    {
        this.PostEvent(EventID.SKIP_TIME, timeOffline * 60);
        GameManager.Instance.AddGold(goldOffline * xOffline);
        SetDeActivePanel(panelOffline);
    }

    public void x2_ReceiveOffline()
    {
        xOffline = 2;
        txtGoldOffline.text = ToLongString(goldOffline * xOffline);
    }
    #endregion

    #region === SPIN ===
    public bool isSpinning;

    public void Btn_ShowSpin()
    {
        if (GameManager.Instance.countSpin > 0)
        {
            SetActivePanel(panelSpin);
            txtCountSpin.text = "x" + GameManager.Instance.countSpin;
        }
    }

    public void Btn_CloseSpin()
    {
        if (!isSpinning)
        {
            SetDeActivePanel(panelSpin);
        }
    }
    #endregion

    #region === VIDEO ===
    long reward_Video;
    int typeReward_Video;
    public void On_Success_Ad_Video()
    {
        SetActivePanel(panelVideo);
        reward_Video = 0;
        typeReward_Video = 0;
        int r = UnityEngine.Random.Range(0, 10);
        if (r < 8) //gold
        {
            long priceLastMine = 0;
            for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
            {
                if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
                {
                    if (priceLastMine <= GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice)
                        priceLastMine = GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice;
                }
            }
            reward_Video = (long)(UnityEngine.Random.Range(GameConfig.Instance.UFO_rate_gold[0], GameConfig.Instance.UFO_rate_gold[1]) * priceLastMine);
            imgRewardVideo.sprite = UIManager.Instance.lstSprReward[1];
            typeReward_Video = 1;
        }
        else //coin
        {
            int r1 = UnityEngine.Random.Range(1, 11);
            int countMine = 0;
            for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
            {
                if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
                    countMine++;
            }
            if (r1 <= 4)
            {
                reward_Video = UnityEngine.Random.Range(1, 3) * countMine;
            }
            else if (r1 <= 8)
            {
                reward_Video = UnityEngine.Random.Range(3, 5) * countMine;
            }
            else
            {
                reward_Video = 5 * countMine;
            }
            imgRewardVideo.sprite = UIManager.Instance.lstSprReward[1];
            typeReward_Video = 2;
        }
        UIManager.Instance.txtReward_Video.text = ToLongString(reward_Video);
    }

    public void Btn_OK_Video()
    {
        if (typeReward_Video == 1)
        {
            GameManager.Instance.AddGold(reward_Video);
        }
        else if (typeReward_Video == 2)
        {
            GameManager.Instance.AddCoin(reward_Video);
        }
        SetDeActivePanel(panelVideo);
        timeVideo = 380;
        isCanClickVideo = false;
    }

    int countSpin_Unlock;
    int timeSkip_Unlock;
    long gold_Unlock;
    int coin_Unlock;
    public void UnlockReward(long _gold, int _spin, int _time, int _coin)
    {
        btnOkUnlock.onClick.RemoveAllListeners();

        gold_Unlock = _gold;
        unlock_2.SetActive(true);
        unlock_2.GetComponentInChildren<Text>().text = ToLongString(gold_Unlock);

        countSpin_Unlock = _spin;
        unlock_1.SetActive(true);
        unlock_1.GetComponentInChildren<Text>().text = countSpin_Unlock.ToString();

        timeSkip_Unlock = _time;
        coin_Unlock = _coin;

        btnOkUnlock.onClick.AddListener(() => Btn_OK_Unlock());
    }

    public void On_Success_Ad_Unlock()
    {
        unlock_3.SetActive(true);
        int r = UnityEngine.Random.Range(0, 10);
        if (r > 5)
        {
            timeSkip_Unlock = UnityEngine.Random.Range(5, 16);
            unlock_3.GetComponentInChildren<Text>().text = "-" + timeSkip_Unlock + "mins";
        }
        else
        {
            int r1 = UnityEngine.Random.Range(1, 11);
            int countMine = 0;
            for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
            {
                if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
                    countMine++;
            }
            if (r1 <= 4)
            {
                coin_Unlock = UnityEngine.Random.Range(1, 3) * countMine;
            }
            else if (r1 <= 8)
            {
                coin_Unlock = UnityEngine.Random.Range(3, 5) * countMine;
            }
            else
            {
                coin_Unlock = 5 * countMine;
            }
            unlock_3.GetComponentInChildren<Text>().text = coin_Unlock.ToString();
        }
    }

    public void Btn_OK_Unlock()
    {
        SetDeActivePanel(panelUnlockReward);
        unlock_1.SetActive(false);
        unlock_2.SetActive(false);
        unlock_3.SetActive(false);
        GameManager.Instance.AddGold(gold_Unlock);
        GameManager.Instance.countSpin += countSpin_Unlock;
        if (timeSkip_Unlock > 0)
        {
            this.PostEvent(EventID.SKIP_TIME, timeSkip_Unlock);
        }
        if (timeSkip_Unlock > 0)
        {
            GameManager.Instance.AddCoin(coin_Unlock);
        }

        countSpin_Unlock = timeSkip_Unlock = coin_Unlock = 0;
        gold_Unlock = 0;
    }
    #endregion

    public void Btn_X_MoreMine()
    {
        for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
        {
            //if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
            GameManager.Instance.lstMap[0].lstMineShaft[i].Btn_X();
        }
        if (GameManager.Instance.lstMap[0].lstMineShaft[0].xMoreMine == 1)
        {
            imgXMine.sprite = sprMoreMine[0];
        }
        else
        {
            imgXMine.sprite = sprMoreMine[1];
        }
    }
    public void Test_Boost()
    {
        GameManager.Instance.boost.SetBoost(TypeBoost.GOLD, 2, 30);
        txtBoost.text = 2.ToString();
        GameManager.Instance.timeBoost = 30;
    }

    public void TestMoney()
    {
        GameManager.Instance.AddGold(10000);
        GameManager.Instance.AddCoin(100);
    }
    #endregion

    #region === TUTORIAL ===
    //public void TutorialOnclick()
    //{
    //    AudioManager.Instance.Play("Click");
    //    AudioManager.Instance.Stop("Menu");
    //    AudioManager.Instance.Play("GamePlay");
    //    PlayerPrefs.SetFloat("X4TimeGame", 4f);
    //    GameManager.Instance.isTutorial = true;
    //    ResetGame();
    //    btnX4.image.sprite = spX1;
    //    menuGame.SetActive(false);
    //    Loading(false);
    //    isPlay = true;
    //    GameManager.Instance.LoadDate();
    //    GameManager.Instance.main.bitCoin = GameConfig.Instance.bitcoinStartGame;
    //    GameManager.Instance.main.dollars = GameConfig.Instance.dollarStartGame;
    //    OnclickWORD(false);
    //    if (PlayerPrefs.GetInt("isDoneTutorial") == 0 || GameManager.Instance.isTutorial)
    //    {
    //        Turorial(WorldManager.Instance.lsCountry[1].gameObject, WorldManager.Instance.lsCountry[1].transform.position, Vector3.zero);
    //    }
    //}

    public void Turorial(GameObject main, Vector3 posHand, Vector3 angleHand)
    {
        if (mainTutorial != null)
            Destroy(mainTutorial);
        panelTutorial.SetActive(true);
        Vector3 pos = main.transform.position;
        mainTutorial = Instantiate(main, panelTutorial.transform);
        mainTutorial.SetActive(true);
        mainTutorial.transform.SetAsFirstSibling();
        mainTutorial.transform.position = pos;
        handTutorial.transform.position = posHand;
        handTutorial.transform.localEulerAngles = angleHand;
    }

    public void Step_1_Tutorial()
    {

    }


    #endregion
}

