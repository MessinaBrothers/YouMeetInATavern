using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnityUtility : MonoBehaviour {

    private static System.Random rng;

    void Awake() {
        rng = new System.Random();

        PreBuild();
    }
    
    //recursive
    public static void MoveToLayer(Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

    public static void Shuffle<T>(IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// replicates pre-build functionality of Assets/Misc/build_script.bat
    /// </summary>
    private static void PreBuild() {
        System.DateTime startPreBuildTime = System.DateTime.Now;
        #if UNITY_EDITOR
            CreateTxt("Assets/Resources/dialogue_xml");
            CreateTxt("Assets/Resources/results");
            
            UnityEditor.AssetDatabase.Refresh();
        #endif

        int preBuildTime = (System.DateTime.Now - startPreBuildTime).Milliseconds;
        Debug.LogFormat("Prebuild time: {0} ms", preBuildTime);
    }

    private static void CreateTxt(string filePath) {
        if (File.Exists(filePath + ".txt")) {
            File.Delete(filePath + ".txt");
        }

        // wait until file is deleted
        while (File.Exists(filePath + ".txt")) { }

        File.Copy(filePath + ".graphml", filePath + ".txt");

        // wait until file exists
        while (File.Exists(filePath + ".txt") == false) { }
    }
}
