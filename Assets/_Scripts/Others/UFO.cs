using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using EventDispatcher;

public class UFO : MonoBehaviour
{
    public float speed;
    public Transform[] posMove;
    public Button btnOk;
    int timeSkip;
    long goldReward;
    int x;
    public bool isOpening;

    public void Move()
    {
        this.transform.DOPath(new Vector3[] { posMove[1].position }, speed).OnComplete(() =>
        {
            Move_Middle();
        });
    }

    void Move_Middle()
    {
        this.transform.DOPath(new Vector3[] { transform.position }, 1f).OnComplete(() =>
        {
            Move_fromMidtoEnd(speed);
        });
    }

    void Move_fromMidtoEnd(float _speed)
    {
        this.transform.DOPath(new Vector3[] { posMove[2].position }, _speed).OnComplete(() =>
        {
            this.transform.position = posMove[0].position;
        });
    }

    public void OnClick()
    {
        isOpening = true;
        transform.DOKill();
        UIManager.Instance.SetActivePanel(UIManager.Instance.panelUFO);
        Move_fromMidtoEnd(speed / 3);
        btnOk.onClick.RemoveAllListeners();
        timeSkip = 0;
        goldReward = 0;
        x = 1;
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelUFO_CoinVideo);
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelUFO_Gold);
        int r = Random.Range(0, 3);
        if (r <= 1)
        {
            UIManager.Instance.SetActivePanel(UIManager.Instance.panelUFO_Gold);
            UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelUFO_CoinVideo);
            long priceLastMine = 0;
            for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
            {
                if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
                    priceLastMine = GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice;
            }
            long a = (long)(Random.Range(GameConfig.Instance.UFO_rate_gold[0], GameConfig.Instance.UFO_rate_gold[1]) * priceLastMine);
            if (a < 10)
                a = 10;
            UIManager.Instance.txtGold_UFO.text = UIManager.Instance.ToLongString(a);
            btnOk.onClick.AddListener(() => Btn_OK(a, 0));
        }
        else
        {
            UIManager.Instance.SetActivePanel(UIManager.Instance.panelUFO_CoinVideo);
            UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelUFO_Gold);
            int r1 = Random.Range(1, 11);
            int countMine = 0;
            int a;
            for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
            {
                if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
                    countMine++;
            }
            if (r1 <= 4)
            {
                a = Random.Range(1, 3) * countMine;
            }
            else if (r1 <= 8)
            {
                a = Random.Range(3, 5) * countMine;
            }
            else
            {
                a = 5 * countMine;
            }
            UIManager.Instance.txtCoin_UFO.text = a.ToString();
            UIManager.Instance.imgRewardUFO.sprite = UIManager.Instance.lstSprReward[0];
            UIManager.Instance.txtReward_UFO.text = "Ad";
            btnOk.onClick.AddListener(() => Btn_OK(a, 1));
        }
    }

    public void Btn_OK(long _value, int _type)
    {
        if (_type == 0)
        {
            GameManager.Instance.AddGold(_value);
        }
        else
        {
            GameManager.Instance.AddCoin(_value * x);
            GameManager.Instance.countSpin++;
            GameManager.Instance.AddGold(goldReward);
            this.PostEvent(EventID.SKIP_TIME, timeSkip);
        }
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelUFO);
        isOpening = false;
    }

    public void On_Success_Ad()
    {
        int r = Random.Range(0, 3);
        x = 2;
        if (r == 0) //luot quay Spin
        {
            UIManager.Instance.imgRewardUFO.sprite = UIManager.Instance.lstSprReward[2];
            UIManager.Instance.txtReward_UFO.text = "1";
        }
        else if (r == 1) //skip thoi gian
        {
            timeSkip = Random.Range(5, 16);
            UIManager.Instance.imgRewardUFO.sprite = UIManager.Instance.lstSprReward[3];
            UIManager.Instance.txtReward_UFO.text = "-" + timeSkip + "mins";
        }
        else //nhan gold
        {
            long priceLastMine = 0;
            for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
            {
                if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
                {
                    if(priceLastMine <= GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice)
                        priceLastMine = GameManager.Instance.lstMap[0].lstMineShaft[i].properties.buyMoreMinePrice;
                }
            }
            goldReward = (long)(Random.Range(GameConfig.Instance.UFO_rate_gold[0], GameConfig.Instance.UFO_rate_gold[1]) * priceLastMine);
            UIManager.Instance.imgRewardUFO.sprite = UIManager.Instance.lstSprReward[1];
            UIManager.Instance.txtReward_UFO.text = UIManager.Instance.ToLongString(goldReward);
        }
    }
}
