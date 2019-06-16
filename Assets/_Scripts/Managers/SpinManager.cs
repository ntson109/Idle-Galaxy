using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using EventDispatcher;

public class SpinManager : MonoBehaviour
{

    public static SpinManager Instance;
    public long goldReward;
    public int coinReward;
    public int timeReward;

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }



    public void Spin()
    {
        if (GameManager.Instance.countSpin > 0 && !UIManager.Instance.isSpinning)
        {
            UIManager.Instance.isSpinning = true;
            GameManager.Instance.countSpin--;
            if (GameManager.Instance.countSpin <= 0)
            {
                //UIManager.Instance.imgCheckTime.fillAmount = 1;
            }
            UIManager.Instance.txtCountSpinMain.text = "x" + GameManager.Instance.countSpin;
            UIManager.Instance.txtCountSpin.text = "x" + GameManager.Instance.countSpin;
            StartCoroutine(RotationSpin());
        }
    }

    public IEnumerator RotationSpin()
    {
        float timeRandom = Random.Range(1f, 3f);
        transform.DORotate(new Vector3(0, 0, -360), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        yield return new WaitForSeconds(timeRandom);
        transform.DOPause();
        yield return new WaitForSeconds(0.05f);
        CheckComplete();


    }

    public void CheckComplete()
    {
        float Zspin = transform.localEulerAngles.z;
        if (Zspin < 0)
        {
            Zspin = 360 + Zspin;
        }
        if (Zspin > 4.5f && Zspin <= 28.5f)
        {

        }
        else if (Zspin > 28.5f && Zspin <= 52.5f)
        {

        }
        else if (Zspin > 52.5f && Zspin <= 75.5f)
        {

        }
        else if (Zspin > 75.5f && Zspin <= 99.5f)
        {

        }
        else if (Zspin > 99.5f && Zspin <= 122.5f)
        {

        }
        else if (Zspin > 122.5f && Zspin <= 146.5f)
        {

        }
        else if (Zspin > 146.5f && Zspin <= 170.5f)
        {

        }
        else if (Zspin > 170.5f && Zspin <= 193.5f)
        {

        }
        else if (Zspin > 193.5f && Zspin <= 217.5f)
        {

        }
        else if (Zspin > 217.5f && Zspin <= 242.5f)
        {

        }
        else if (Zspin > 242.5f && Zspin <= 266.5f)
        {

        }
        else if (Zspin > 266.5f && Zspin <= 290.5f)
        {

        }
        else if (Zspin > 290.5f && Zspin <= 315.5f)
        {

        }
        else if (Zspin > 315.5f && Zspin <= 340.5f)
        {

        }
        else if (Zspin > 340.5f && Zspin <= 364.5f)
        {

        }
    }

    public IEnumerator RewardSpin(int _type, float _value = 0)
    {
        UIManager.Instance.btnReceiveSpin.onClick.RemoveAllListeners();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetActivePanel(UIManager.Instance.panelReceiveSpin);
        switch (_type)
        {
            case 0:
                goldReward = (long)(_value * GameManager.Instance.GOLD);
                UIManager.Instance.receiveSpin_random.SetActive(false);
                UIManager.Instance.receiveSpin_normal.SetActive(true);
                UIManager.Instance.imgRewardSpin.sprite = UIManager.Instance.sprRewardSpin[_type];
                UIManager.Instance.txtRewardSpin.text = UIManager.Instance.ToLongString(goldReward);
                break;
            case 1:
                timeReward = (int)_value;
                UIManager.Instance.receiveSpin_random.SetActive(false);
                UIManager.Instance.receiveSpin_normal.SetActive(true);
                UIManager.Instance.imgRewardSpin.sprite = UIManager.Instance.sprRewardSpin[_type];
                UIManager.Instance.txtRewardSpin.text = "-" + UIManager.Instance.ConvertTime(timeReward * 3600);
                break;
            case 2:
                coinReward = (int)_value;
                UIManager.Instance.receiveSpin_random.SetActive(false);
                UIManager.Instance.receiveSpin_normal.SetActive(true);
                UIManager.Instance.imgRewardSpin.sprite = UIManager.Instance.sprRewardSpin[_type];
                UIManager.Instance.txtRewardSpin.text = coinReward.ToString();
                break;
            case 3:
                goldReward = (long)Random.Range(GameManager.Instance.GOLD * 0.05f, GameManager.Instance.GOLD * 0.15f);
                coinReward = Random.Range(5, 50);
                UIManager.Instance.receiveSpin_random.SetActive(true);
                UIManager.Instance.receiveSpin_normal.SetActive(false);
                UIManager.Instance.txtRewardSpin_randomCoin.text = coinReward.ToString();
                UIManager.Instance.txtRewardSpin_randomGold.text = UIManager.Instance.ToLongString(goldReward);
                break;
            default:
                break;
        }
        UIManager.Instance.btnReceiveSpin.onClick.AddListener(() => ReceiveSpin(_type));
        UIManager.Instance.isSpinning = false;
    }

    public void ReceiveSpin(int _type)
    {
        switch (_type)
        {
            case 0:
                GameManager.Instance.AddGold(goldReward);
                break;
            case 1:
                //this.PostEvent(EventID.SKIP_TIME, timeReward);
                GameManager.Instance.AddSkipTime(timeReward);
                break;
            case 2:
                GameManager.Instance.AddCoin(coinReward);
                break;
            case 3:
                GameManager.Instance.AddGold(goldReward);
                GameManager.Instance.AddCoin(coinReward);
                break;
            default:
                break;
        }
        UIManager.Instance.SetDeActivePanel(UIManager.Instance.panelReceiveSpin);
        goldReward = coinReward = timeReward = 0;
    }

    public void CloseSpin()
    {
        //UIManager.Instance.panelSpin.SetActive(false);
        //if (UIManager.Instance.isSpinning)
        //{
        //    UIManager.Instance.isSpinning = false;
        //    GameManager.Instance.countSpin++;
        //    UIManager.Instance.txtCountSpinMain.text = "x" + GameManager.Instance.countSpin;
        //    UIManager.Instance.txtCountSpin.text = "x" + GameManager.Instance.countSpin;
        //    if (UIManager.Instance.lsItem[6].isOnItem)
        //    {
        //        UIManager.Instance.imgCheckTime.fillAmount = 0;
        //        UIManager.Instance.lsItem[6].timeItem = 0;
        //        UIManager.Instance.lsItem[6].isOnItem = false;
        //    }
        //}
        if (!UIManager.Instance.isSpinning)
            transform.DOKill();
    }
}
