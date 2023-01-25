using UnityEditor;
using UnityEngine;

public class AnimationPostprocessor : AssetPostprocessor
{
    private const string AnimationAssetPath = "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/Animations";
    private const string AnimationTargetPath = "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/AnimationTargets";

    private const string MaleAnimationTargetName = "AnimationTargets/MaleAnimationTargetV2";
    private const string FemaleAnimationTargetName = "AnimationTargets/FemaleAnimationTargetV2";

    private static bool AnimationsImported = false;
    private static readonly string[] AnimationFiles = new string[]
    {
        "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/Animations/Female/FemaleAnimationTargetV2@Breathing Idle.fbx",
        "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/Animations/Female/FemaleAnimationTargetV2@Walking.fbx",
        "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/Animations/Male/MaleAnimationTargetV2@Breathing Idle.fbx",
        "Assets/Plugins/Wolf3D ReadyPlayerMe SDK/Resources/Animations/Male/MaleAnimationTargetV2@Walking.fbx",
    };

    private void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;

        void SetModelImportData()
        {
            modelImporter.useFileScale = false;
            modelImporter.animationType = ModelImporterAnimationType.Human;
        }

        if (AnimationsImported && assetPath.Contains(AnimationAssetPath))
        {
            SetModelImportData();

            bool isFemaleFolder = assetPath.Contains("Female");
            GameObject animationTarget = Resources.Load<GameObject>(isFemaleFolder ? FemaleAnimationTargetName : MaleAnimationTargetName);
            modelImporter.sourceAvatar = animationTarget.GetComponent<Animator>().avatar;
        }
        else if (assetPath.Contains(AnimationTargetPath))
        {
            SetModelImportData();
        }
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string item in importedAssets)
        {
            if (item.Contains(MaleAnimationTargetName))
            {
                for (int i = 0; i < AnimationFiles.Length; i++)
                {
                    AssetDatabase.ImportAsset(AnimationFiles[i]);
                }

                AnimationsImported = true;
            }
        }
    }
}
