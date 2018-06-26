using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public static event DeathEventHandler deathEventHandler;
    public delegate void DeathEventHandler(int deadID);

    public static event ReceiveDamageEventHandler receiveDamageEventHandler;
    public delegate void ReceiveDamageEventHandler(int id, int damageAmount, GameObject weaponObject);

    public float invincibleAfterHitTime;
    private float invincibleTimer;

    public int max;
    private int current;

    private int id;

    private Text text;

    void Start() {
        current = max;

        id = GetComponentInParent<EntityID>().id;

        text = GetComponentInChildren<Text>();
    }

    void Update() {
        // always orient up
        // http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.up, Camera.main.transform.rotation * Vector3.up);

        if (invincibleTimer > 0) {
            invincibleTimer -= Time.deltaTime;
        }
    }

    void LateUpdate() {
        text.text = string.Format("{0:0.}", current);
    }

    public void Reduce(int amount) {
        current -= amount;
    }

    private void GetHit(int attackWeaponID, GameObject weaponObject, GameObject hitObject) {
        if (invincibleTimer > 0) {
            return;
        }

        Health hitHealth = hitObject.GetComponentInChildren<Health>();
        if (hitHealth == this) {
            int oldHP = current;

            Reduce(weaponObject.GetComponent<WeaponAttack>().damage);

            receiveDamageEventHandler.Invoke(id, oldHP - current, weaponObject);

            if (current <= 0) {
                deathEventHandler.Invoke(id);
                Destroy(gameObject);
            } else {
                invincibleTimer = invincibleAfterHitTime;
            }
        }
    }

    void OnEnable() {
        WeaponHitDetect.weaponHitEventHandler += GetHit;
    }

    void OnDisable() {
        WeaponHitDetect.weaponHitEventHandler -= GetHit;
    }
}
