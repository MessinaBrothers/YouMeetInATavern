using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationScripts : MonoBehaviour {

    public static event AttackEndedEventHandler attackEndedEventHandler;
    public delegate void AttackEndedEventHandler();

    void Start() {
    }

    void Update() {

    }

    public void EndAttack() {
        attackEndedEventHandler.Invoke();
    }
}
