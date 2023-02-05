using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour, IDamageable
{
    public Transform GetTransform()
    {
        return transform;
    }

    public void TakeDamage(int damage)
    {
        ObjectiveTakeDamage.Invoke(damage);
    }

    public UnityEvent<int> ObjectiveTakeDamage = new UnityEvent<int>();


}
