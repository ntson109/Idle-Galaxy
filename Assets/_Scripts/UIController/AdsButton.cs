using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AdsButton : MonoBehaviour
{
    [System.Serializable] public class mEvent : UnityEvent { }
    public mEvent onSuccess;
    public mEvent onFail;

    void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(() => this.OnAdsButtonClick());
    }

    private void OnAdsButtonClick()
    {
        AdmobManager.Instance.RequestRewardBasedVideo(() =>
        {
            this.onSuccess.Invoke();
        });
    }
}
