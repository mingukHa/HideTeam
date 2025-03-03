using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ImprovedUnusedAssetsFinder : EditorWindow
{
    [MenuItem("Tools/Find Unused Assets (Scene Included)")]
    static void FindUnusedAssets()
    {
        // ���� ������ �����ϴ� ���±��� �����ϱ� ���� �� ������ ������
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
        List<string> scenes = new List<string>();
        foreach (string sceneGUID in scenePaths)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            scenes.Add(scenePath);
        }

        // ������Ʈ �� ��� ���� ��������
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        List<string> unusedAssets = new List<string>();

        foreach (string asset in allAssets)
        {
            if (asset.StartsWith("Assets/")) // "Packages/" ���� �ý��� ���� ����
            {
                // ���� ������ ������ �˻�
                string[] dependencies = AssetDatabase.GetDependencies(scenes.ToArray(), true);
                if (!new List<string>(dependencies).Contains(asset))
                {
                    unusedAssets.Add(asset);
                }
            }
        }

        if (unusedAssets.Count == 0)
        {
            Debug.Log("��� ������ ������Ʈ���� ��� ���Դϴ�.");
        }
        else
        {
            Debug.Log("������ �ʴ� ���� ���:\n" + string.Join("\n", unusedAssets));
        }
    }
}
