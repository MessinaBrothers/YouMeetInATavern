using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtility : MonoBehaviour {
    
    //recursive
    public static void MoveToLayer(Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }
}
