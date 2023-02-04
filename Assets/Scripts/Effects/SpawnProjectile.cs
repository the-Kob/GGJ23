using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject _effectToSpawn;
    
    
    public void SpawnVFX()
    {
        if (firePoint == null)
            return;

        GameObject effect = Instantiate(_effectToSpawn, firePoint.transform.position, Quaternion.identity);
    }
}
