using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour {

    public void SetImage(Sprite sprite) {
        GetComponent<Image>().sprite = sprite;
    }
}
