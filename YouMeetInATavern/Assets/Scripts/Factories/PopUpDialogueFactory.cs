using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDialogueFactory : MonoBehaviour {

    public GameObject popupDialoguePrefab;
    
    private static GameObject popupDialoguePrefabWrapper;

    void Start() {
        // set static variablesd
        popupDialoguePrefabWrapper = popupDialoguePrefab;
    }

    public static GameObject Create(string s, float lifetime, Transform transform) {
        GameObject go = Instantiate(popupDialoguePrefabWrapper, transform);
        go.transform.SetParent(null);

        PopUpDialogue popup = go.GetComponentInChildren<PopUpDialogue>();
        popup.SetText(s);
        popup.lifetime = lifetime;

        return go;
    }
}
