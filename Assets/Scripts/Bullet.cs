using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;

    public void SetSpeed(float value) {
        speed = value;
    }

    public void SetDamage(float value) {
        damage = value;
    }

    void Update() {
        transform.position += transform.forward*speed*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<HealthHandler>() != null) {
            other.gameObject.GetComponent<HealthHandler>().TakeDamage(damage);
        }

        //TODO: spawn hit vfx

        Destroy(gameObject);
    }
}
