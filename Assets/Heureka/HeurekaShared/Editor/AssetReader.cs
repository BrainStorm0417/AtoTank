using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace HeurekaGames
{
    public static class AssetReader
    {
        public static List<string> GetPrefabsFromSceneFiles(string[] scenes, out Dictionary<string, List<string>> assetSceneDependencies)
        {
            assetSceneDependencies = new Dictionary<string, List<string>>();

            List<string> sceneAssetPaths = new List<string>();

            foreach (string str in scenes)
            {
                string[] fooScenes = new string[1] { str };
                string[] dependencies = AssetDatabase.GetDependencies(fooScenes);
                
                for (int i = 0; i < dependencies.Length; i++)
                {
                    EditorUtility.DisplayProgressBar(
                        "Getting Dependencies",
                        "Analyzing scenes to get used prefabs",
                        (float)i / (float)(dependencies.Length));


                    if (!sceneAssetPaths.Contains(dependencies[i]))
                    {
                        if (dependencies[i] != null)
                            sceneAssetPaths.Add(dependencies[i]);
                    }

                    //Add to Asset/Scene Dependency dictionary
                    if (!assetSceneDependencies.ContainsKey(dependencies[i]))
                        assetSceneDependencies.Add(dependencies[i], new List<string>());

                    assetSceneDependencies[dependencies[i]].Add(str);
                }
            }

            EditorUtility.ClearProgressBar();

            return sceneAssetPaths;
        }

        public static string[] GetEnabledScenesInBuild()
        {
            return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
        }

        public static string[] GetAllScenesInBuild()
        {
            return (from scene in EditorBuildSettings.scenes select scene.path).ToArray();
        }

        public static string[] GetAllScenes()
        {
            return (from scene in AssetDatabase.GetAllAssetPaths() where scene.EndsWith(".unity") select scene).ToArray();
        }
    }
}