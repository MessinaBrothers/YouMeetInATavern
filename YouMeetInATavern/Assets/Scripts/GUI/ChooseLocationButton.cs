using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLocationButton : MonoBehaviour {

    public GameObject highlight;

    public GameData.Location location;

    public bool isClickedOnStart;

    private InputController inputController;

    private GameData data;

    void Start() {
        inputController = FindObjectOfType<InputController>();
        data = FindObjectOfType<GameData>();

        highlight.SetActive(false);

        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.mute = true;
        if (isClickedOnStart == true) {
            //gameObject.GetComponent<Button>().onClick.Invoke();
            highlight.SetActive(true);
            data.chosenLocation = location;
        }
        //audioSource.mute = false;
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
        if (this.location == location) {
            data.chosenLocation = location;
            highlight.SetActive(true);
        } else {
            highlight.SetActive(false);
        }
    }
}
