using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ImprovedUnusedAssetsFinder : EditorWindow
{
    [MenuItem("Tools/Find Unused Assets (Scene Included)")]
    static void FindUnusedAssets()
    {
        // 현재 씬에서 참조하는 에셋까지 포함하기 위해 씬 에셋을 가져옴
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
        List<string> scenes = new List<string>();
        foreach (string sceneGUID in scenePaths)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            scenes.Add(scenePath);
        }

        // 프로젝트 내 모든 에셋 가져오기
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        List<string> unusedAssets = new List<string>();

        foreach (string asset in allAssets)
        {
            if (asset.StartsWith("Assets/")) // "Packages/" 같은 시스템 파일 제외
            {
                // 씬을 포함한 의존성 검사
                string[] dependencies = AssetDatabase.GetDependencies(scenes.ToArray(), true);
                if (!new List<string>(dependencies).Contains(asset))
                {
                    unusedAssets.Add(asset);
                }
            }
        }

        if (unusedAssets.Count == 0)
        {
            Debug.Log("모든 에셋이 프로젝트에서 사용 중입니다.");
        }
        else
        {
            Debug.Log("사용되지 않는 에셋 목록:\n" + string.Join("\n", unusedAssets));
        }
    }
}
