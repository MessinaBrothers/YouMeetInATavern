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
        System.DateTime time = System.DateTime.Now;
        #if UNITY_EDITOR
            string filePath = "Assets/Resources/dialogue_xml.txt";
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }

            // wait until file is deleted
            while (File.Exists(filePath)) { }

            File.Copy("Assets/Resources/dialogue_xml.graphml", filePath);

            // wait until file exists
            while (File.Exists(filePath) == false) { }

            UnityEditor.AssetDatabase.ImportAsset("dialogue_xml.txt");
            UnityEditor.AssetDatabase.Refresh();
        #endif

        System.DateTime now = System.DateTime.Now;
        print(now - time);
    }
}
