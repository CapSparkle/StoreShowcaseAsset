using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
    public string bundleUrl; // The URL of the AssetBundle to download

    private AssetBundle bundle; // Reference to the downloaded AssetBundle

    void Start()
    {
        StartCoroutine(LoadBundle());
    }

    private IEnumerator LoadBundle()
    {
        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
        {
            // Send the request and wait for it to complete
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                // Handle errors if necessary
                Debug.Log("Error loading AssetBundle: " + request.error);
            }
            else
            {
                // Get a reference to the downloaded AssetBundle
                bundle = DownloadHandlerAssetBundle.GetContent(request);

                // Instantiate all GameObjects in the AssetBundle
                foreach (GameObject prefab in bundle.LoadAllAssets<GameObject>())
                {
                    Instantiate(prefab);
                }

                // Unload the AssetBundle to free up memory
                bundle.Unload(false);
            }
        }
    }
}
