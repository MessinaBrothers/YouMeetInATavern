using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    public GameObject weapon;

    public float attackSpeed;

    private RandomClip weaponSound;
    private WeaponHitDetect hitDetect;

    private Animator weaponAnimator;

    private int id;

    private bool canAttack;

    void Start() {
        id = weapon.GetComponent<EntityID>().id;

        weaponSound = weapon.GetComponentInChildren<RandomClip>();
        hitDetect = weapon.GetComponentInChildren<WeaponHitDetect>();
        weaponAnimator = weapon.GetComponentInChildren<Animator>();

        hitDetect.enabled = false;

        canAttack = true;
    }

    void Update() {
        if (canAttack == true && Input.GetButtonDown("Fire3")) {
            hitDetect.Restart();
            weaponSound.PlaySound();
            weaponAnimator.SetTrigger("Swing");

            canAttack = false;
        }
    }

    private void EndAttack(int attackWeaponID) {
        if (id == attackWeaponID) {
            hitDetect.enabled = false;
            canAttack = true;
        }
    }

    private void Hit(int attackWeaponID, GameObject hitObject) {
        if (id == attackWeaponID) {
            Health hitHealth = hitObject.GetComponentInChildren<Health>();
            if (hitHealth != null) {
                //print("This weapon hit " + hitObject.name);
                //hitHealth.Reduce(1);
            }
        }
    }

    void OnEnable() {
        WeaponAnimationScripts.attackEndedEventHandler += EndAttack;
        WeaponHitDetect.weaponHitEventHandler += Hit;
    }

    void OnDisable() {
        WeaponAnimationScripts.attackEndedEventHandler -= EndAttack;
        WeaponHitDetect.weaponHitEventHandler -= Hit;
    }
}
