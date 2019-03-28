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
                this.PostEvent(EventID.SKIP_TIME, timeReward);
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

    int l;
    long CapacityOffline()
    {
        long _money = 0;
        int counter = 0;
        int time = 0;
        l = 0;
        int T_invalid = 5000;
        int[] c;
        int[] a;
        int[] b;
        int[] n_counter = new int[6] { 10, 100, 100, 200, 400, 800 };

        for (int i = 0; i < GameManager.Instance.lstMap[0].lstMineShaft.Count; i++)
        {
            if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
            {
                l++;
            }
            else
                break;
        }

        c = new int[l];
        a = new int[l];
        b = new int[l];
        for (int i = 0; i < l; i++)
        {
            if (GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.LOCK && GameManager.Instance.lstMap[0].lstMineShaft[i].state != MineShaft.StateMineShaft.UNLOCKING)
            {
                c[i] = GameManager.Instance.lstMap[0].lstMineShaft[i].properties.miningTime - GameManager.Instance.lstMap[0].lstMineShaft[i].timer;
                a[i] = GameManager.Instance.lstMap[0].lstMineShaft[i].input;
                if (i == 0)
                {
                    b[i] = a[i];
                }
                else
                {
                    b[i] = GameManager.Instance.lstMap[0].lstMineShaft[i - 1].store.value;
                }
            }
        }

        while (counter < n_counter[l - 1])
        {
            int j = 0;
            int temp = c[0];
            int sp = 0;

            for (int i = 1; i < l; i++)
            {
                if (c[i] < temp)
                {
                    temp = c[i];
                    j = i;
                }
            }

            sp = GameManager.Instance.lstMap[0].lstMineShaft[j].input;
            time = time + c[j];

            for (int i = 0; i < l; i++)
            {
                //neu nha may dang trong qua trinh san xuat thi update lai thoi gian
                if (c[i] != T_invalid)
                    c[i] = c[i] - temp;
            }

            if (b[j] > 0)
            {
                //Neu so san pham o nha chua j ma lon hon cong suat nha may j thi day so san pham len bang cong suat
                if (b[j] >= GameManager.Instance.lstMap[0].lstMineShaft[j].totalCapacity)
                {
                    //set so san pham dang xu ly cho nha chua j
                    a[j] = GameManager.Instance.lstMap[0].lstMineShaft[j].totalCapacity;
                    //set lai so luong trong nha chua j
                    if (j != 0)
                        b[j] = b[j] - a[j];// truong hop j = 0 thi nha chua dau luon co so sp bang cong suat nhu da noi o tren, k can update
                }
                else
                {
                    //set so san pham dang xu ly cho nha chua j
                    a[j] = b[j];
                    if (j != 0)
                    {
                        //set lai so luong trong nha chua j
                        b[j] = 0;
                    }
                }
                //set lai thoi gian cho nha j
                c[j] = GameManager.Instance.lstMap[0].lstMineShaft[j].properties.miningTime;
            }
            else
            {
                a[j] = 0;			//khong co gi de xu ly
                c[j] = T_invalid;	//khong co thoi gian xu ly
            }

            if (j == l - 1)
            {
                // do nothing
            }
            else
            {
                //cac san pham nay se duoc day len nha chua j+1, con thua thi se duoc ban
                //kiem tra nha chua j+1, neu con kha nang chua duoc nhieu hon A[j] thi tat ca se duoc don len nha chua j+1
                //k co gi ban ra
                if (GameManager.Instance.lstMap[0].lstMineShaft[j].store.capacity - b[j + 1] >= sp)
                {
                    b[j + 1] = b[j + 1] + sp;
                    sp = 0;
                }
                else
                {
                    sp = sp - (GameManager.Instance.lstMap[0].lstMineShaft[j].store.capacity - b[j + 1]);
                    b[j + 1] = GameManager.Instance.lstMap[0].lstMineShaft[j].store.capacity;
                }

                //nếu nhà j+1 dang k san xuat (C[j+1] = T_invalid) thi set de nha j+1 san xuat
                if (c[j + 1] == T_invalid)
                {
                    //set A va B
                    //Neu so sp trong nha chua j+1 nhieu hon cong suat nha j+1
                    if (b[j + 1] >= GameManager.Instance.lstMap[0].lstMineShaft[j + 1].totalCapacity)
                    {
                        a[j + 1] = GameManager.Instance.lstMap[0].lstMineShaft[j + 1].totalCapacity;
                        b[j + 1] = b[j + 1] - a[j + 1];
                    }
                    else
                    {
                        a[j + 1] = b[j + 1];
                        b[j + 1] = 0;
                    }
                    //Set C
                    c[j + 1] = GameManager.Instance.lstMap[0].lstMineShaft[j + 1].properties.miningTime;
                }
            }

            //tinh tien
            _money = _money + sp * (long)GameManager.Instance.lstMap[0].lstMineShaft[j].properties.unitPrice;

            counter++;
        }

        Debug.Log(_money);
        Debug.Log(_money / time);
        if (_money / time < 5)
        {
            return 5;
        }
        else
        {
            return (_money / time);
        }
    }
}
