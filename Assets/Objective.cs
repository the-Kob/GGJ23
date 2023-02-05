using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IDamageable
{
    public Transform GetTransform()
    {
        return transform;
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
