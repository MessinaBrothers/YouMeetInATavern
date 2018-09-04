using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtility : MonoBehaviour {

    private static System.Random rng;

    void Awake() {
        rng = new System.Random();
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
}
