using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;

public class UIManager : MonoBehaviour {
    public static UIManager Instance = new UIManager();

    [HideInInspector]
    public List<string> arrAlphabetNeed = new List<string>();
    private string[] arrAlphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    public Text txtGold;
    public Text txtDiamond;

    //[Header("PANEL")]
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
    void Start () {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < arrAlphabet.Length; j++)
            {
                arrAlphabetNeed.Add(arrAlphabet[i] + arrAlphabet[j]);
            }
        }
	}
	
	void Update () {
	}
    #endregion

    #region === SUPPORT ===
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
        if (_g == null)
            return;

        _g.SetActive(true);
        if (_g.name == "InWall")
            _g.GetComponent<Animator>().Play("ActivePanel");
    }

    public void SetDeActivePanel(GameObject _g)
    {
        if (_g == null)
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

    private string[] timeFormat = new string[]
	{
		"d",
		"h",
		"m",
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
            return num2.ToString("00") + ":" + num3.ToString("00") + "s";
        }
        return num3.ToString("00") + "s";
    }

    public int GetOfflineTime(string dateTime)
    {
        if (dateTime == string.Empty)
        {
            return 0;
        }
        return (int)Mathf.Round((float)DateTime.Now.Subtract(Convert.ToDateTime(dateTime)).TotalSeconds);
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
    
    #endregion

    #region === UI HOME ===
    public void Btn_Play()
    {
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
            {
                this.PostEvent(EventID.START_GAME);
                GameManager.Instance.stateGame = StateGame.PLAYING;
                GameManager.Instance.GOLD = GameConfig.Instance.GoldStart;
                GameManager.Instance.COIN = GameConfig.Instance.CoinStart;
            });
    }

    public void Btn_Yes_NewPlay()
    {

    }

    public void Btn_No_NewPlay()
    {

    }

    public void Btn_Continue()
    {

    }

    public void Btn_Share()
    {

    }

    public void Btn_Rate()
    {

    }
    #endregion
}

