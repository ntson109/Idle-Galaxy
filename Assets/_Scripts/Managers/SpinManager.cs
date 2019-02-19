using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SpinManager : MonoBehaviour
{

    public static SpinManager Instance;

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
        //if (GameManager.Instance.countSpin > 0 && !UIManager.Instance.lsItem[6].isOnItem && !UIManager.Instance.isSpinning)
        //{
        //    UIManager.Instance.isSpinning = true;
        //    GameManager.Instance.countSpin--;
        //    if (GameManager.Instance.countSpin <= 0)
        //    {
        //        UIManager.Instance.imgCheckTime.fillAmount = 1;
        //        UIManager.Instance.lsItem[6].timeItem = 10 * 60;
        //        UIManager.Instance.lsItem[6].timeItemTatol = 10 * 60;
        //        UIManager.Instance.lsItem[6].isOnItem = true;
        //    }
        //    UIManager.Instance.txtCountSpinMain.text = "x" + GameManager.Instance.countSpin;
        //    UIManager.Instance.txtCountSpin.text = "x" + GameManager.Instance.countSpin;
        //    StartCoroutine(RotationSpin());
        //}
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
            AddGive(1);
        }
        else if (Zspin > 28.5f && Zspin <= 52.5f)
        {
            AddGive(2);
        }
        else if (Zspin > 52.5f && Zspin <= 75.5f)
        {
            AddGive(3);
        }
        else if (Zspin > 75.5f && Zspin <= 99.5f)
        {
            AddGive(4);
        }
        else if (Zspin > 99.5f && Zspin <= 122.5f)
        {
            AddGive(5);
        }
        else if (Zspin > 122.5f && Zspin <= 146.5f)
        {
            AddGive(1);
        }
        else if (Zspin > 146.5f && Zspin <= 170.5f)
        {
            AddGive(2);
        }
        else if (Zspin > 170.5f && Zspin <= 193.5f)
        {
            AddGive(3);
        }
        else if (Zspin > 193.5f && Zspin <= 217.5f)
        {
            AddGive(4);
        }
        else if (Zspin > 217.5f && Zspin <= 242.5f)
        {
            AddGive(1);
        }
        else if (Zspin > 242.5f && Zspin <= 266.5f)
        {
            AddGive(2);
        }
        else if (Zspin > 266.5f && Zspin <= 290.5f)
        {
            AddGive(3);
        }
        else if (Zspin > 290.5f && Zspin <= 315.5f)
        {
            AddGive(4);
        }
        else if (Zspin > 315.5f && Zspin <= 340.5f)
        {
            AddGive(1);
        }
        else if (Zspin > 340.5f && Zspin <= 364.5f)
        {
            AddGive(2);
        }
    }

    public void AddGive(double dollar)
    {
       
        //int locationEnd = GameManager.Instance.lsLocation.Count - 1;
        //int jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        //double dollarRecive = 0;
        //if (GameManager.Instance.lsLocation.Count > 1)
        //{
        //    if (jobEnd == -1)
        //    {
        //        locationEnd--;
        //        jobEnd = GameManager.Instance.lsLocation[locationEnd].countType;
        //    }
        //    dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
        //}
        //else
        //{
        //    if (jobEnd == -1)
        //    {
        //        dollarRecive = GameManager.Instance.lsLocation[0].lsWorking[0].price;
        //    }
        //    else
        //    {
        //        dollarRecive = GameManager.Instance.lsLocation[locationEnd].lsWorking[jobEnd].price;
        //    }
        //}
        //StartCoroutine(IEOpenGive(dollar * dollarRecive / 2.5f));
    }

    public IEnumerator IEOpenGive(double dollar)
    {
        yield return new WaitForSeconds(1f);
        //GameManager.Instance.dollar += dollar;
        //UIManager.Instance.PushGiveGold("You have recived " + UIManager.Instance.ConvertNumber(dollar) + " dollar");
        //if (GameManager.Instance.countSpin <= 0)
        //{
        //    UIManager.Instance.adsSpin.SetActive(true);
        //    UIManager.Instance.bgSpin.color = new Color32(255, 255, 255, 128);
        //}
        //UIManager.Instance.isSpinning = false;

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
        transform.DOKill();
        StopAllCoroutines();
    }
}
