using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndConverseButton : MonoBehaviour {

    public static event EndConverseEventHandler endConverseEventHandler;
    public delegate void EndConverseEventHandler(GameObject card);

    private GameData data;
    
    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {

    }

    public void BroadcastKey() {
        endConverseEventHandler.Invoke(data.selectedCard);
    }
}
