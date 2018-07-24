using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFactory : MonoBehaviour {

    public GameObject npcCardPrefab, itemCardPrefab;

    private static GameData data;
    private static GameObject npcCardPrefabWrapper, itemCardPrefabWrapper;

    void Start() {
        data = FindObjectOfType<GameData>();

        // set static variables
        npcCardPrefabWrapper = npcCardPrefab;
        itemCardPrefabWrapper = itemCardPrefab;
    }

    public static GameObject CreateNPCCard(uint npcID) {
        // get NPC data
        NPCData npcData = data.npcData[npcID];

        // create the card gameobject
        GameObject card = Instantiate(npcCardPrefabWrapper);
        card.name = npcData.name + " NPC";

        // set the NPC values
        NPC npc = card.GetComponent<NPC>();
        npc.npcID = npcID;

        // set the card name
        card.GetComponentInChildren<Text>().text = npcData.name;

        // set the card image
        card.GetComponentInChildren<CardImage>().SetImage(Resources.Load<Sprite>("Card Art/" + npcData.imageFile));

        // set the NPC sfx
        CardSFX sfx = card.GetComponent<CardSFX>();
        sfx.introductionClip = Resources.Load<AudioClip>("NPC SFX/" + npcData.sfxIntro);
        sfx.greetingClips = Convert("NPC SFX/", npcData.sfxOnClicks);
        sfx.startConversationClips = Convert("NPC SFX/", npcData.sfxGreetings);

        sfx.goodbyeClips = Convert("NPC SFX/", npcData.sfxGoodbyes);

        return card;
    }

    public static GameObject CreateItemCard(string key) {
        // get item data
        ItemData itemData = data.itemData[key];

        // create the card gameobject
        GameObject card = Instantiate(itemCardPrefabWrapper);
        card.name = "Item - " + itemData.name;

        //// set the NPC values
        //NPC npc = card.GetComponent<NPC>();
        //npc.npcID = npcID;

        // set the card name
        card.GetComponentInChildren<Text>().text = itemData.name;

        // set the card image
        card.GetComponentInChildren<CardImage>().SetImage(Resources.Load<Sprite>("Item Art/" + itemData.imageFile));

        // set the NPC sfx
        CardSFX sfx = card.GetComponent<CardSFX>();
        sfx.greetingClips = Convert("Item SFX/", itemData.sfxOnClicks);

        return card;
    }

    // convert array of file locations to an array of audio clips
    private static AudioClip[] Convert(string fileLocation, string[] clipFileNames) {
        AudioClip[] clips = new AudioClip[clipFileNames.Length];
        for (int i = 0; i < clipFileNames.Length; i++) {
            clips[i] = Resources.Load<AudioClip>(fileLocation + clipFileNames[i]);
        }
        return clips;
    }
}
