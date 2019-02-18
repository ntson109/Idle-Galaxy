using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    public static Purchaser Instance = new Purchaser();
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

    }

    public void Init()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(GameConfig.Instance.kProductID50, ProductType.Consumable);
        builder.AddProduct(GameConfig.Instance.kProductID300, ProductType.Consumable);
        builder.AddProduct(GameConfig.Instance.kProductID5000, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void Buy50()
    {
        BuyProductID(GameConfig.Instance.kProductID50);

        //if (UIManager.Instance.panelLoadingIAP != null)
        //    UIManager.Instance.panelLoadingIAP.SetActive(true);
    }
    public void Buy300()
    {
        BuyProductID(GameConfig.Instance.kProductID300);

        //if (UIManager.Instance.panelLoadingIAP != null)
        //    UIManager.Instance.panelLoadingIAP.SetActive(true);
    }
    public void Buy5000()
    {
        BuyProductID(GameConfig.Instance.kProductID5000);

        //if (UIManager.Instance.panelLoadingIAP != null)
        //    UIManager.Instance.panelLoadingIAP.SetActive(true);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {

            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            //Mng.mng.ui.loading.SetActive(false);

            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }




    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        
        Debug.Log("a");
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, GameConfig.Instance.kProductID50, StringComparison.Ordinal))
        {
            //GameManager.Instance.gold += 50;
            //if (PlayerPrefs.GetInt("Gold", 10) > 50 && Mathf.Abs(PlayerPrefs.GetInt("GoldPre", 0) - PlayerPrefs.GetInt("Gold", 10)) >= 50)
            //{
            //PlayerPrefs.SetInt("GoldPre", (int)GameManager.Instance.gold);
            //StorageService storageService = App42API.BuildStorageService();
            //storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, (int)GameManager.Instance.gold)), new UnityCallBack2());
            //}
            PlayerPrefs.SetInt("NoAds", 1);
            //UIManager.Instance.PushGiveGold("You have recived 50 gold ");
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, GameConfig.Instance.kProductID300, StringComparison.Ordinal))
        {
            //GameManager.Instance.gold += 300;

            //PlayerPrefs.SetInt("GoldPre", (int)GameManager.Instance.gold);
            //StorageService storageService = App42API.BuildStorageService();
            //storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, (int)GameManager.Instance.gold)), new UnityCallBack2());
            //UIManager.Instance.PushGiveGold("You have recived 300 gold ");
            PlayerPrefs.SetInt("NoAds", 1);
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, GameConfig.Instance.kProductID5000, StringComparison.Ordinal))
        {
            //GameManager.Instance.gold += 5000;

            //PlayerPrefs.SetInt("GoldPre", (int)GameManager.Instance.gold);
            //StorageService storageService = App42API.BuildStorageService();
            //storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, (int)GameManager.Instance.gold)), new UnityCallBack2());
            //UIManager.Instance.PushGiveGold("You have recived 5000 gold ");
            PlayerPrefs.SetInt("NoAds", 1);
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            //UIManager.Instance.PushGiveGold("ProcessPurchase: FAIL ");
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        //if (UIManager.Instance.panelLoadingIAP != null)
            //UIManager.Instance.panelLoadingIAP.SetActive(false);
        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        //if (UIManager.Instance.panelLoadingIAP != null)
            //UIManager.Instance.panelLoadingIAP.SetActive(false);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
