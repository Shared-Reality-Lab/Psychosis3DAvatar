using UnityEngine;
using Wolf3D.ReadyPlayerMe.AvatarSDK;

public class RuntimeTest : MonoBehaviour
{
    private readonly string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/29d42edb-2705-4ee4-ab6b-ff079322e48b.glb";

    private void Start()
    {
        Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarLoaded);
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        Debug.Log($"Avatar loeded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
    }
}
