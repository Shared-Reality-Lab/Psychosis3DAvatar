using UnityEngine;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class WebviewTest : MonoBehaviour
    {
        private void Start()
        {
            Webview webview = new Webview();
            webview.OnAvatarCreated = OnAvatarCreated;
            webview.SetScreenPadding(0, 0, 0, 0);
            webview.CreateWebview(this);
        }

        private void OnAvatarCreated(string url)
        {
            AvatarLoader avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(url, OnAvatarLoaded);
        }

        private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
        {
            Debug.Log("Loaded");
        }
    }
}
