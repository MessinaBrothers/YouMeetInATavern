using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotate : MonoBehaviour {

    public GameObject targetImage, verticalText, horizontalText;

    public bool isHorizontal;
    private bool wasHorizontal;
    
    void Start() {
        isHorizontal = false;
        wasHorizontal = false;

        ToggleText(isHorizontal);
    }

    void Update() {
        if (isHorizontal != wasHorizontal) {
            if (wasHorizontal == true) {
                targetImage.transform.Rotate(Vector3.forward, -90);
                transform.Rotate(Vector3.forward, 90);
                ToggleText(false);
            } else {
                targetImage.transform.Rotate(Vector3.forward, 90);
                transform.Rotate(Vector3.forward, -90);
                ToggleText(true);
            }

            wasHorizontal = isHorizontal;
        }
    }

    private void ToggleText(bool isHorizontal) {
        horizontalText.SetActive(isHorizontal);
        verticalText.SetActive(!isHorizontal);
    }
}
