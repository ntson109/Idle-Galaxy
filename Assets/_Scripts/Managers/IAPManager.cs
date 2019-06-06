using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
// #define RECEIPT_VALIDATION
#endif

#if INAPP
using UnityEngine.Purchasing;
using UnityEngine.UI;
#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;
#if UNITY_IOS || UNITY_ANDROID
    private List<string> productIds;

    public void OnPurchaseSuccess(int index)
    {
        var coin_pack = GameConfig.Instance.listCoinPacks[index];
        GameManager.Instance.AddCoin(coin_pack.value);
    }

    public void PurchaseProductIndex(int index)
    {
        if (m_PurchaseInProgress == true)
        {
            return;
        }

        Debug.Log(m_Controller);
        Debug.Log(m_Controller.products);
        Debug.Log(m_Controller.products.all);
        m_Controller.InitiatePurchase(m_Controller.products.all[index]); 
        m_PurchaseInProgress = true;
    }

    public void RestorePurchased()
    {
        m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
    }

    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;

    private int m_SelectedItemIndex = -1; // -1 == no product
    private bool m_PurchaseInProgress;

    private Selectable m_InteractableSelectable; // Optimization used for UI state management

#if RECEIPT_VALIDATION
	private CrossPlatformValidator validator;
#endif

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        Debug.Log("init " + m_Controller);
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

        Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
					{
						item.metadata.localizedTitle,
						item.metadata.localizedDescription,
						item.metadata.isoCurrencyCode,
						item.metadata.localizedPrice.ToString(),
						item.metadata.localizedPriceString
					}));
            }
        }

        // Prepare model for purchasing
        if (m_Controller.products.all.Length > 0)
        {
            m_SelectedItemIndex = 0;
        }

        // Now that I have real products, begin showing product purchase history
    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);

        m_PurchaseInProgress = false;

#if RECEIPT_VALIDATION
		if (Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer) {
			try {
				var result = validator.Validate(e.purchasedProduct.receipt);
				Debug.Log("Receipt is valid. Contents:");
				foreach (IPurchaseReceipt productReceipt in result) {
					Debug.Log(productReceipt.productID);
					Debug.Log(productReceipt.purchaseDate);
					Debug.Log(productReceipt.transactionID);

					GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
					if (null != google) {
						Debug.Log(google.purchaseState);
						Debug.Log(google.purchaseToken);
					}

					AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
					if (null != apple) {
						Debug.Log(apple.originalTransactionIdentifier);
						Debug.Log(apple.cancellationDate);
						Debug.Log(apple.quantity);
					}
				}
			} catch (IAPSecurityException) {
				Debug.Log("Invalid receipt, not unlocking content");
				return PurchaseProcessingResult.Complete;
			}
		}
#endif

        // You should unlock the content here.
        int index = productIds.IndexOf(e.purchasedProduct.definition.id);
        if (index != -1)
        {
            OnPurchaseSuccess(index);
        }
        // Indicate we have handled this purchase, we will not be informed of it again.x
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// This will be called is an attempted purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        Debug.Log("Purchase failed: " + item.definition.id);
        Debug.Log(r);
        m_PurchaseInProgress = false;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.productIds = new List<string>();
        for (int i = 0; i < GameConfig.Instance.listCoinPacks.Count; i++)
        {
            var coin_pack = GameConfig.Instance.listCoinPacks[i];
            this.productIds.Add(coin_pack.productID);
        }

        var module = StandardPurchasingModule.Instance();

        // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and 
        // developer ui (initialization, purchase, failure code setting). These correspond to 
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        var builder = ConfigurationBuilder.Instance(module);
        // This enables the Microsoft IAP simulator for local testing.
        // You would remove this before building your release package.
        builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
        builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApuIW24J3EANAAzuADCWJ9kJ5srMqTYfYqCehPhFIF+8SYCbcR8xsVqewB8NH3xGGL3RVKr7eFcnc7b1tY67C1qtzVTq5N7ylxMH5rPm8Jt0Cr0uRoY0vpVHhmtItd4jcsfU4D5RwekeUFE8YhIED7tdeseOhtfJP+O4o/+XQUr4PPRQwdwAICBbA7KL16urWwE1I5pQX6oEwMZzaCO6K9h/eHSEoPaGCWeu/I7L2kfjh5eEgFenaKjekuKu3d4ZvKAcw7trbB9L+7key4L84HRbRedwxZvV4wqA+tYO0UEm/EH+IsXPBWD/DvvCsY8Q+B/ZqAel1r6P0CpryVF2oIwIDAQAB");

        // Define our products.
        // In this case our products have the same identifier across all the App stores,
        // except on the Mac App store where product IDs cannot be reused across both Mac and
        // iOS stores.
        // So on the Mac App store our products have different identifiers,
        // and we tell Unity IAP this by using the IDs class.

        foreach (string id in this.productIds)
        {
            var type = id.Contains(".gem") ? ProductType.Consumable : ProductType.NonConsumable;
            builder.AddProduct(id, type, new IDs
		    {
			    {id, AppleAppStore.Name},
                {id, GooglePlay.Name},
		    });
        }
        
#if RECEIPT_VALIDATION
		validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
#endif

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);
    }

    private void OnTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored.");
    }

    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    public string GetLocalizedPrice(int index)
    {
        return this.m_Controller.products.all[index].metadata.localizedPriceString;
    }

#endif
}
#endif