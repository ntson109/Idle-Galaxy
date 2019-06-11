using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour
{
    public AdsType Type;

    void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(() => this.OnAdsButtonClick());
    }

    private void OnAdsButtonClick()
    {
        System.Action success_action = null;

        switch (this.Type)
        {
            case AdsType.HOME:
                success_action = () => UIManager.Instance.OnHomeAdsX2Success();
                break;

            default:
                break;
        }

        AdmobManager.Instance.RequestRewardBasedVideo(success_action);
    }
}

public enum AdsType
{
    HOME,
    UNLOCK
}