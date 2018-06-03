using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour {

    public GameObject startingWeaponPrefab;
    
	void Awake () {
        // load the player's weapon on game start
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<ItemSlot>().EquipItem(Instantiate(startingWeaponPrefab));
	}
	
	void Update () {
		
	}
}
