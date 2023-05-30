
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class BundleLoader : MonoBehaviour
{
    string bundleUri = "https://storage.yandexcloud.net/showcasestore/goodsbundle";


    string[] prevNames;
    async UniTask Start()
    {
        if (Application.platform == RuntimePlatform.Android)
            bundleUri = "https://storage.yandexcloud.net/showcasestore/goods/goodsbundle";
        else
            bundleUri = "https://storage.yandexcloud.net/showcasestore/goodsbundle";

        commodity = new Dictionary<string, GameObject>();
        while(true)
        {
            await UniTask.Delay(3000);

            AssetBundle assetBundle = null;
                using (var assetBundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri: new System.Uri(bundleUri)))
                {
                    await assetBundleRequest.SendWebRequest();
                    print("downloading bundle...");
                    while (false == assetBundleRequest.downloadHandler.isDone)
                        await UniTask.Yield();

                    print("done");
                
                    assetBundle = DownloadHandlerAssetBundle.GetContent(assetBundleRequest);
                }

            if (assetBundle == null)
                continue;

            var newCommodityNames = assetBundle.GetAllAssetNames();

            bool resetCommodityShowcaseFlag = true;
            if (prevNames != null &&
                prevNames.Length == newCommodityNames.Length)
            {
                Array.Sort(prevNames);
                Array.Sort(newCommodityNames);

                if (prevNames.SequenceEqual(newCommodityNames))
                    resetCommodityShowcaseFlag = false;
            }

            if (resetCommodityShowcaseFlag)
            {
                if(prevNames != null)
                    foreach (string name in prevNames.Except(newCommodityNames))
                        RemoveCommodity(name);

                foreach (string name in newCommodityNames)
                    await AddCommodity(assetBundle, name);

                prevNames = newCommodityNames;
                updateDone?.Invoke(commodity.Values.ToList());
                assetBundle.Unload(false);
            }

            
        }
    }

    async Task AddCommodity(AssetBundle assetBundle, string assetName)
    {
        if (!commodity.ContainsKey(assetName))
        {
            commodity[assetName] = null;
            commodity[assetName] = (await assetBundle.LoadAssetAsync(assetName) as GameObject);
            commodityAdded?.Invoke(assetName);
        }  
    }

    void RemoveCommodity(string name)
    {
        if (commodity.ContainsKey(name))
        {
            commodity.Remove(name);
            commodityRemoved?.Invoke(name);
        }
    }

    public Action<string> commodityAdded, commodityRemoved;

    public Action<List<GameObject>> updateDone;
    public Dictionary<string, GameObject> commodity { get; private set; }
}
