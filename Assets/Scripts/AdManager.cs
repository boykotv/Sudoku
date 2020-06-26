using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public string appID;

    public string bannerAdId;

    public string interstitialAdlId;

    public AdPosition bannerPos;

    public bool testDevice = false;

    public static AdManager Instance;

    private BannerView myBannerView;

    private InterstitialAd interstitial;

    public void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        MobileAds.Initialize(appID); //?
        
        this.CreateBanner(CreateRequest());
        this.CreateinterstitialAd(CreateRequest());

    }

    private AdRequest CreateRequest()
    {
        AdRequest request;

        if (testDevice)
        {
            request = new AdRequest.Builder().AddTestDevice(SystemInfo.deviceUniqueIdentifier).Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }
        return request;
    }

    #region interstitialAd

        public void CreateinterstitialAd(AdRequest request)
        {
            this.interstitial = new InterstitialAd(interstitialAdlId);
            this.interstitial.LoadAd(request);
        }

        public void ShowInterstitialAd()
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }

            this.interstitial.LoadAd(CreateRequest());
        }

    #endregion

    #region BannerAD

        public void CreateBanner(AdRequest request)
        {
            this.myBannerView = new BannerView(bannerAdId, AdSize.SmartBanner, bannerPos);
            this.myBannerView.LoadAd(request);
            HideBanner();
        }

        public void HideBanner()
        {
            myBannerView.Hide();
        }

        public void ShowBanner()
        {
            myBannerView.Show();
        }

    #endregion



}
