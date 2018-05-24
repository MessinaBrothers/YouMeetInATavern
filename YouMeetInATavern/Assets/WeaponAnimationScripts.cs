using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationScripts : MonoBehaviour {

    private WeaponAttack attack;

    void Start() {
        attack = GetComponentInParent<WeaponAttack>();
    }

    void Update() {

    }

    public void EndAttack() {
        attack.EndAttack();
    }
}
