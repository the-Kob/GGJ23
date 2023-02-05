using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public GameObject hitVFX;
    public LayerMask hitMask;
    private float speed;
    private int damage;

    public void SetSpeed(float value) {
        speed = value;
    }

    public void SetDamage(int value) {
        damage = value;
    }

    void Update() {
        transform.position += transform.forward*speed*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if(hitMask != (hitMask | (1 << other.gameObject.layer)))
            return;

        
        if(other.gameObject.GetComponent<Enemy>() != null) {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        if(hitVFX != null) {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
