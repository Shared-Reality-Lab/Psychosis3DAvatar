﻿using UnityEditor;
using UnityEngine;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class AvatarLoaderEditorWindowStarter : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string item in importedAssets)
            {
                if (item.Contains("RPM_EditorImage_"))
                {
                    AvatarLoaderEditorWindow.ShowWindow(false);
                    return;
                }
            }
        }
    }

    public class AvatarLoaderEditorWindow : EditorWindow
    {
        public const string Version        = "v1.4.3";

        private const string BannerPath     = "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Editor/RPM_EditorImage_Banner.png";
        private const string LovePath       = "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Editor/RPM_EditorImage_Love.png";
        private const string AnimTargetPath = "/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/AnimationTargets/MaleAnimationTarget.fbx";
        private const string AnimsPath = "/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/Animations/Male";

        private const string RpmUrl     = "https://readyplayer.me";
        private const string BlogUrl    = "https://readyplayer.me/blog";
        private const string DocsUrl    = "https://readyplayer.me/docs";
        private const string DiscordUrl = "https://discord.gg/UCvRaM2Hm9";
        private const string WolfUrl    = "https://wolf3d.io";

        private const string UrlSaveKey = "UrlSaveKey";
        private const string ModelCachingSaveKey = "ModelCachingSaveKey";
        private const string EyeAnimationSaveKey = "EyeAnimationSaveKey";
        private const string VoiceToAnimSaveKey = "VoiceToAnimSaveKey";

        private static Texture2D banner = null;
        private static Texture2D love = null;
        private static EditorAvatarLoader loader = null;

        private bool variablesLoaded = false;

        private string url = null;
        private bool useModelCaching = false;
        private bool useEyeAnimations = false;
        private bool useVoiceToAnim = false;

        private GUIStyle headerTextStyle = null;
        private GUIStyle footerTextStyle = null;
        private GUIStyle webButtonStyle = null;

        private readonly GUILayoutOption fieldHeight = GUILayout.Height(20);
        private readonly GUILayoutOption inputFieldWidth = GUILayout.Width(140);
        private readonly GUILayoutOption largeButtonHeight = GUILayout.Height(30);
        private readonly GUILayoutOption halfScreenButtonWidth = GUILayout.Width(248);

        [MenuItem("ReadyPlayerMe/Avatar Loader")]
        private static void ShowWindowMenu()
        {
            ShowWindow(true);
        }

        public static void ShowWindow(bool force)
        {
            if (force || !SessionState.GetBool("WindowInit", false))
            {
                AvatarLoaderEditorWindow window = GetWindow(typeof(AvatarLoaderEditorWindow)) as AvatarLoaderEditorWindow;
                window.titleContent = new GUIContent("RPM Unity SDK");
                window.minSize = window.maxSize = new Vector2(512, 455);
                window.ShowUtility();
                SessionState.SetBool("WindowInit", true);
            }
        }

        private void OnGUI()
        {
            LoadAssets();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(GUILayout.Width(512));

            DrawBanner();

            EditorGUILayout.BeginVertical("Box");
            DrawInputField();
            DrawModelCaching();
            DrawOptions();
            DrawLoadAvatarButton();
            EditorGUILayout.EndVertical();

            DrawAnimationButtons();

            DrawExternalLinks();

            DrawFooter();

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void LoadCachedVariables()
        {
            url = EditorPrefs.GetString(UrlSaveKey);
            useModelCaching = EditorPrefs.GetBool(ModelCachingSaveKey);
            useEyeAnimations = EditorPrefs.GetBool(EyeAnimationSaveKey);
            useVoiceToAnim = EditorPrefs.GetBool(VoiceToAnimSaveKey);

            variablesLoaded = true;
        }

        private void LoadAssets()
        {
            if(!variablesLoaded) LoadCachedVariables();

            if (banner == null) banner = AssetDatabase.LoadAssetAtPath<Texture2D>(BannerPath);
            if (love == null) love = AssetDatabase.LoadAssetAtPath<Texture2D>(LovePath);
            if (loader == null) loader = new EditorAvatarLoader();

            // Styles
            if (headerTextStyle == null)
            {
                headerTextStyle = new GUIStyle();
                headerTextStyle.fontSize = 18;
                headerTextStyle.richText = true;
                headerTextStyle.fontStyle = FontStyle.Bold;
                headerTextStyle.normal.textColor = Color.white;
            }

            if (footerTextStyle == null)
            {
                footerTextStyle = new GUIStyle();
                footerTextStyle.richText = true;
                footerTextStyle.fontStyle = FontStyle.Bold;
                footerTextStyle.normal.textColor = GUI.skin.label.normal.textColor;
            }

            if(webButtonStyle == null)
            {
                webButtonStyle = new GUIStyle(GUI.skin.button);
                webButtonStyle.fontSize = 10;
                webButtonStyle.fixedWidth = 97;
            }
        }

        private void DrawBanner()
        {
            if(banner != null)
            {
                GUI.DrawTexture(new Rect((position.size.x - banner.width) / 2, 0, banner.width, banner.height), banner);
            }

            EditorGUI.DropShadowLabel(new Rect((position.size.x - 280) / 2, 110, 280, 40), $"ReadyPlayerMe Unity SDK { Version }", headerTextStyle);

            GUILayout.Space(142);
        }

        private void DrawInputField()
        {
            GUI.skin.textField.fontSize = 12;

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(new GUIContent("Avatar URL", "Paste the avatar URL received from ReadyPlayerMe here."), inputFieldWidth);
            url = EditorGUILayout.TextField(url, fieldHeight);
            EditorPrefs.SetString(UrlSaveKey, url);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawModelCaching()
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Use Model Caching", "Use the model already downloaded instead of downloading it again."), inputFieldWidth);
            useModelCaching = EditorGUILayout.Toggle(useModelCaching, fieldHeight);
            EditorPrefs.SetBool(ModelCachingSaveKey, useModelCaching);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Changes you make on ReadyPlayerMe are reflected over the same URL. If caching is toggled on, avatar model with changes will not be downloaded.", MessageType.Info);

            EditorGUILayout.EndVertical();
        }

        private void DrawOptions()
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Use Eye Animations", "Optional helper component for random eye rotation and blinking, for a less static look."), inputFieldWidth);
            useEyeAnimations = EditorGUILayout.Toggle(useEyeAnimations, fieldHeight);
            EditorPrefs.SetBool(EyeAnimationSaveKey, useEyeAnimations);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Use Voice To Animation", "Optional helper component for voice amplitude to jaw bone movement."), inputFieldWidth);
            useVoiceToAnim = EditorGUILayout.Toggle(useVoiceToAnim, fieldHeight);
            EditorPrefs.SetBool(VoiceToAnimSaveKey, useVoiceToAnim);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawLoadAvatarButton()
        {
            GUI.enabled = !string.IsNullOrEmpty(url);

            if (GUILayout.Button("Load Avatar", largeButtonHeight))
            {
                loader.LoadAvatar(url, AvatarLoadCallback);
                loader.UseModelCaching = useModelCaching;
            }

            GUI.enabled = true;
        }

        private void AvatarLoadCallback(GameObject avatar, AvatarMetaData metaData)
        {
            Debug.Log("Avatar loaded.");

            if (useEyeAnimations) avatar.AddComponent<EyeAnimationHandler>();
            if (useVoiceToAnim) avatar.AddComponent<VoiceHandler>();

            Selection.activeObject = avatar;
        }

        private void DrawAnimationButtons()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.HelpBox("To use Mixamo animations on full-body avatars, please upload MaleAnimationTarget.FBX and FemaleAnimation.FBX files to Mixamo, and move your animation files to Animations folder.", MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reveal Animation Targets Folder", halfScreenButtonWidth))
            {
                EditorUtility.RevealInFinder(Application.dataPath + AnimTargetPath);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Reveal Animations Folder", halfScreenButtonWidth))
            {
                EditorUtility.RevealInFinder(Application.dataPath + AnimsPath);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawExternalLinks()
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("ReadyPlayerMe", webButtonStyle))
            {
                Application.OpenURL(RpmUrl);
            }
            if (GUILayout.Button("Blog", webButtonStyle))
            {
                Application.OpenURL(BlogUrl);
            }
            if (GUILayout.Button("Docs", webButtonStyle))
            {
                Application.OpenURL(DocsUrl);
            }
            if (GUILayout.Button("Discord", webButtonStyle))
            {
                Application.OpenURL(DiscordUrl);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFooter()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Built with  --  by Wolf3D", footerTextStyle, GUILayout.Width(160)))
            {
                Application.OpenURL(WolfUrl);
            }

            if (love != null)
            {
#if UNITY_2019_1_OR_NEWER
                GUI.DrawTexture(new Rect((position.size.x - 44f) / 2f, 430, 16, 16), love);
#else
                GUI.DrawTexture(new Rect((position.size.x - 28f) / 2f, 435, 16, 16), love);
#endif
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}
