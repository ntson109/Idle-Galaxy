using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
//using UnityEngine.Advertisements;
using System;

public class Ads : MonoBehaviour
{
    [Header("Admob")]
    InterstitialAd interstitalAd;
    BannerView bannerView;

    bool isLoadAds = false;
    //bool isShowAds = false;

    public GameObject panelPlane;
    public Text txtPlaneVideoAds;
    public Text txtPlaneReciveDollar;

    [Header("Time")]
    public float timeAds = 1;

    public static Ads Instance = new Ads();
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize("ca-app-pub-4738062221647171~1836833926");
        //RequestAd();
        MobileAds.SetiOSAppPauseOnBackground(true);
    }

    void Update()
    {
        timeAds += Time.deltaTime;
        ShowInterstitialAd();
    }

    #region ===ADMOB===


    public void RequestAd()
    {

#if UNITY_ANDROID
        GameConfig.Instance.ID_Inter_android = "ca-app-pub-6285794272989840/5632501293"; //test
        if (!isLoadAds && GameConfig.Instance.ID_Inter_android != null)
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.ID_Inter_android);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#elif UNITY_IOS
        if (!isLoadAds && GameConfig.Instance.ID_Inter_ios != null && GameConfig.Instance.ID_Inter_ios != "")
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.ID_Inter_ios);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#else
        GameConfig.Instance.ID_Inter_android = "ca-app-pub-6285794272989840/5632501293"; //test
        if (!isLoadAds && GameConfig.Instance.ID_Inter_android != null)
        {
            interstitalAd = new InterstitialAd(GameConfig.Instance.ID_Inter_android);
            AdRequest requestInterAd = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(requestInterAd);
            isLoadAds = true;
            Debug.Log("Load Ads - " + interstitalAd.IsLoaded().ToString());
        }
#endif

    }

    public void ShowInterstitialAd()
    {
        if (timeAds < GameConfig.Instance.TimeAd)
        {
            RequestAd();
            return;
        }

        if (interstitalAd != null)
        {
            if (PlayerPrefs.GetInt("NoAds") == 0)
            {
                if (interstitalAd.IsLoaded())
                {
                    interstitalAd.Show();
                    isLoadAds = false;
                    timeAds = 0;
                    Debug.Log("Show Ads");
                }
            }
        }
        else
        {
            Debug.Log("Null");
            RequestAd();
        }
    }

    public void RequestBanner()
    {
        if (GameConfig.Instance.ID_Banner_ios != null)
        {
            bannerView = new BannerView(GameConfig.Instance.ID_Banner_ios, AdSize.Banner, AdPosition.Bottom);
            AdRequest requestBanner = new AdRequest.Builder().Build();
            bannerView.LoadAd(requestBanner);
        }
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            if (PlayerPrefs.GetInt("NoAds") == 0)
            {
                bannerView.Show();
                Debug.Log("Show Banner");
            }
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            Debug.Log("Hide Banner");
        }
    }
    #endregion

    #region ===UNITY ADS===

    public void SuccessAdsUnity()
    {
        //Debug.Log("Cong tien : " + GameConfig.Instance.dollarVideoAd);
    }
    #endregion
}
