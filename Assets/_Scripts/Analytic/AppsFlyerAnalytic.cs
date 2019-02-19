using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyerAnalytic : MonoBehaviour
{
    private bool tokenSent;
    public string DEV_KEY;
    public string APP_ID;

    public static AppsFlyerAnalytic Instance;
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
        Application.runInBackground = true;
        Screen.orientation = ScreenOrientation.Portrait;
        DontDestroyOnLoad(this);
        AppsFlyer.setIsDebug(true);

#if UNITY_IOS

        AppsFlyer.setAppsFlyerKey(DEV_KEY);
        AppsFlyer.setAppID(APP_ID);
        AppsFlyer.setIsDebug(true);
        AppsFlyer.getConversionData();
        AppsFlyer.trackAppLaunch();

        // register to push notifications for iOS uninstall
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
        Screen.orientation = ScreenOrientation.Portrait;

#elif UNITY_ANDROID

		AppsFlyer.init ("WdpTVAcYwmxsaQ4WeTspmh");

		//AppsFlyer.setAppID ("YOUR_APP_ID"); 

		// for getting the conversion data
		AppsFlyer.loadConversionData("StartUp");

		// for in app billing validation
        //AppsFlyer.createValidateInAppListener ("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure"); 

		//For Android Uninstall
		//AppsFlyer.setGCMProjectNumber ("YOUR_GCM_PROJECT_NUMBER");


#endif
    }

    void Update()
    {
#if UNITY_IOS
        if (!tokenSent)
        {
            byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
            if (token != null)
            {
                //For iOS uninstall
                AppsFlyer.registerUninstall(token);
                tokenSent = true;
            }
        }
#endif
    }

    #region === EVENTS ===
    public void Level_Achieved(string _level)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add(AFInAppEvents.LEVEL, _level);
        //eventValue.Add(AFInAppEvents.SCORE, "category_a");
        AppsFlyer.trackRichEvent(AFInAppEvents.LEVEL_ACHIEVED, eventValue);
    }

    public void Tutorial_Completion()
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add(AFInAppEvents.SUCCESS, "Complete");
        eventValue.Add(AFInAppEvents.CONTENT_ID, "Tutorial");
        AppsFlyer.trackRichEvent(AFInAppEvents.TUTORIAL_COMPLETION, eventValue);
    }

    public void Purchaser(string _id, string _content, string _price, string _currency, string _dateCheckIn)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add(AFInAppEvents.CONTENT_ID, _id);
        eventValue.Add(AFInAppEvents.CONTENT_TYPE, _content);
        eventValue.Add(AFInAppEvents.REVENUE, _price);
        eventValue.Add(AFInAppEvents.CURRENCY, _currency);
        eventValue.Add(AFInAppEvents.DATE_A, _dateCheckIn);
        AppsFlyer.trackRichEvent(AFInAppEvents.PURCHASE, eventValue);
    }

    public void Ad_View(string _type)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add("af_adrev_ad_type", _type);
        AppsFlyer.trackRichEvent("af_ad_view", eventValue);
    }

    public void Ad_Click(string _type)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add("af_adrev_ad_type", _type);
        AppsFlyer.trackRichEvent("af_ad_click", eventValue);
    }
    #endregion
}
