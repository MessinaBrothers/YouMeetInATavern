using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public static event DeathEventHandler deathEventHandler;
    public delegate void DeathEventHandler(int deadID);

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
    }

    void LateUpdate() {
        text.text = string.Format("{0:0.}", current);
    }

    public void Reduce(int amount) {
        current -= amount;
    }

    private void Hit(int attackWeaponID, GameObject weaponObject, GameObject hitObject) {
        Health hitHealth = hitObject.GetComponentInChildren<Health>();
        if (hitHealth == this) {

            Reduce(weaponObject.GetComponent<WeaponAttack>().damage);

            if (current <= 0) {
                deathEventHandler.Invoke(id);
                Destroy(gameObject);
            }
        }
    }

    void OnEnable() {
        WeaponHitDetect.weaponHitEventHandler += Hit;
    }

    void OnDisable() {
        WeaponHitDetect.weaponHitEventHandler -= Hit;
    }
}
