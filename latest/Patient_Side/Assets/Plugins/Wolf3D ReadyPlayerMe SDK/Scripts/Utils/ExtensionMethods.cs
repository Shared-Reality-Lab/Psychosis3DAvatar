using UnityEngine;
using System.Collections;
using System.Linq;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public static class ExtensionMethods
    {
        public class CoroutineRunner : MonoBehaviour {
            ~CoroutineRunner() {
                Destroy(gameObject);
            }
        }

        private static CoroutineRunner coroutineRunner;

        public static Coroutine Run(this IEnumerator ienum)
        {
            if (coroutineRunner == null)
            {
                coroutineRunner = new GameObject("[Wolf3D.CoroutineRunner]").AddComponent<CoroutineRunner>();
                coroutineRunner.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInBuild;
                coroutineRunner.gameObject.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInBuild;
            }

            return coroutineRunner.StartCoroutine(ienum);
        }

        private static readonly string[] headNameFilter =
            {"Wolf3D.Avatar_Renderer_Head", "Wolf3D.Avatar_Renderer_Avatar"};
        public static SkinnedMeshRenderer GetHeadMeshRenderer(this GameObject gameObject)
        {
            var allChildren = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            var headMesh = allChildren.First(s => headNameFilter.Contains(s.gameObject.name));
            
            if (headMesh == null)
                Debug.LogWarning("No Head mesh found");
            
            return headMesh;
        }
    }
}
