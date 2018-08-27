using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDeckController : MonoBehaviour {

    public void Load(GameData data) {
        // create an ordered list of unlock keys
        List<string> unlockKeys = new List<string>(data.cardData.Keys);
        unlockKeys.Sort();

        // load deck list
        GetComponentInChildren<ViewDeckListGUI>().Load(data, unlockKeys);
        // load card list
        GetComponentInChildren<ViewCardsGUI>().Load(data, unlockKeys);
    }

    public void Display() {
        GetComponentInChildren<ViewDeckListGUI>().Display();
        GetComponentInChildren<ViewCardsGUI>().Display();
    }

    public void Close() {
        GetComponentInChildren<ViewDeckListGUI>().Close();
        GetComponentInChildren<ViewCardsGUI>().Close();
    }
}
