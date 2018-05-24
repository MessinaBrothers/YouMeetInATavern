using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationScripts : MonoBehaviour {

    public static event AttackEndedEventHandler attackEndedEventHandler;
    public delegate void AttackEndedEventHandler(int attackWeaponID);

    private int id;

    void Start() {
        id = GetComponentInParent<EntityID>().id;
    }

    void Update() {

    }

    public void EndAttack() {
        attackEndedEventHandler.Invoke(id);
    }
}
