using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLocationButton : MonoBehaviour {

    public GameObject highlight;

    public GameData.Location location;

    private InputController inputController;

    void Start() {
        inputController = FindObjectOfType<InputController>();

        highlight.SetActive(false);
    }

    public void Broadcast() {
        inputController.ChooseLocation(location);
    }

    void OnEnable() {
        InputController.chooseLocationEventHandler += HandleLocation;
    }

    void OnDisable() {
        InputController.chooseLocationEventHandler -= HandleLocation;
    }

    private void HandleLocation(GameData.Location location) {
        highlight.SetActive(this.location == location);
    }
}
