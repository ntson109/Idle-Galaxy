using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

/*public class GoogleMobileAdsDemoHandler : IDefaultInAppPurchaseProcessor
{
    private readonly string[] validSkus = { "android.test.purchased" };

    //Will only be sent on a success.
    public void ProcessCompletedInAppPurchase(IInAppPurchaseResult result)
    {
        result.FinishPurchase();
        GoogleMobileAdsDemoScript.OutputMessage = "Purchase Succeeded! Credit user here.";
    }

    //Check SKU against valid SKUs.
    public bool IsValidPurchase(string sku)
    {
        foreach (string validSku in validSkus)
        {
            if (sku == validSku)
            {
                return true;
            }
        }
        return false;
    }

    //Return the app's public key.
    public string AndroidPublicKey
    {
        //In a real app, return public key instead of null.
        get { return null; }
    }
}*/

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance { get; set; }
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;

    private VideoRewardType VideoRewardType { get; set; }
    private bool IsRewardedVideo { get; set; }

    void Awake()
    {
        Instance = this;
    }

    public void RequestBanner()
    {
        //if (GameManager.Instance.IsRemoveAds) return;
        
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-5689749980636351/8885879823";
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            string adUnitId = GameConfig.Instance.AdmobID_Banner_ios;
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        // Register for ad events.
        bannerView.OnAdLoaded += HandleAdLoaded;
        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
        bannerView.OnAdLoaded += HandleAdOpened;
        bannerView.OnAdClosed += HandleAdClosed;
        bannerView.OnAdLeavingApplication += HandleAdLeftApplication;
        // Load a banner ad.
        bannerView.LoadAd(createAdRequest());
    }

    public void RequestInterstitial()
    {
        //if (GameManager.Instance.IsRemoveAds) return;
        
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-5689749980636351/2839346226";
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            string adUnitId = GameConfig.Instance.AdmobID_Interstitial_ios;
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        interstitial.OnAdLoaded += HandleInterstitialLoaded;
        interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.OnAdOpening += HandleInterstitialOpened;
        interstitial.OnAdClosed += HandleInterstitialClosed;
        interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitial.LoadAd(createAdRequest());
    }

    public AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
             //.AddTestDevice(AdRequest.TestDeviceSimulator)
                 //.AddTestDevice("9ACD01B470951161A30C0AA4B6DA3A7D")
                 .AddKeyword("game")
                 .SetGender(Gender.Male)
                 .SetBirthday(new DateTime(1985, 1, 1))
                 .TagForChildDirectedTreatment(false)
                 .AddExtra("color_bg", "9B30FF")
                 .Build();
    }

    public void RequestRewardBasedVideo(VideoRewardType type)
    {
        //LoadingPopup.Show();
        
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-7945781459560557/8311323627";
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            string adUnitId = GameConfig.Instance.AdmobID_VideoReward_ios;
#else
            string adUnitId = "unexpected_platform";
#endif
        rewardBasedVideo = RewardBasedVideoAd.Instance;
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        rewardBasedVideo.LoadAd(createAdRequest(), adUnitId);

        this.VideoRewardType = type;
        this.IsRewardedVideo = false;
    }

    public void ShowBanner(bool show = true)
    {
        if (show)
        {
            //if (GameManager.Instance.IsRemoveAds) return;
            this.bannerView.Show();
        }
        else
        {
            this.bannerView.Hide();
        }
    }

    public void ShowInterstitial()
    {
        //if (GameManager.Instance.IsRemoveAds) return;

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            //LoadingPopup.Hide();
        }
        else
        {
            print("Interstitial is not ready yet.");
            //LoadingPopup.Show();
        }
    }

    public void ShowRewardBasedVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else
        {
            print("Reward based video ad is not ready yet.");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
        this.ShowBanner();
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
        this.ShowInterstitial();
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoLoaded event received.");
        this.ShowRewardBasedVideo();
        //LoadingPopup.Hide();
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoClosed event received");

        if (this.IsRewardedVideo)
        {
            if (this.VideoRewardType == VideoRewardType.HOME)
            {
                
            }
            else if (this.VideoRewardType == VideoRewardType.UPGRADE)
            {
                
            }
        }
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " +
                type);

        this.IsRewardedVideo = true;
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion

}

public enum VideoRewardType
{
    HOME,
    UPGRADE
}