using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class AR_Ability : Ability
{
    public GameObject bullet;
    public GameObject muzzleVFX;
    public float fireRate;
    public int maxBullets = -1;
    public float bulletSpeed;
    public float bulletDamage;
    public string sourceTransformName = "mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand";

    // "Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm/Right_Hand"

    // "mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand"

    private PlayerInput _playerInput;
    private InputAction _action;

    private Transform sourceTransform;
    private Transform cameraTransform;

    public override void Activate(GameObject parent, InputAction action) {
        _playerInput = parent.GetComponent<PlayerInput>();
        _action = action;

        sourceTransform = parent.transform.Find(sourceTransformName);
        cameraTransform = Camera.main.transform;

        RootsController.Instance.StartCoroutine(Fire());
    }

    IEnumerator Fire() {
        GameObject bulletBuffer;
        GameObject vfxBuffer;
        Vector3 target;
        int bulletCount = 0;
        do {
            // aim from camera

            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {
                Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                target = hit.point;
            } else {
                target = sourceTransform.position + sourceTransform.TransformDirection(Vector3.forward)*1000;
            }
            
            // fire from source
            if(muzzleVFX != null) {
                vfxBuffer = Instantiate(muzzleVFX, sourceTransform.position, Quaternion.identity);
                vfxBuffer.transform.LookAt(hit.point);
                Destroy(vfxBuffer, 2f);
            }
            bulletBuffer = Instantiate(bullet, sourceTransform.position, Quaternion.identity);
            bulletBuffer.GetComponent<Bullet>().SetDamage(bulletDamage);
            bulletBuffer.GetComponent<Bullet>().SetSpeed(bulletSpeed);
            bulletBuffer.transform.LookAt(hit.point);

            bulletCount++;

            //Destroy(bulletBuffer, 5f); //max mifespan

            if(maxBullets != -1 && bulletCount >= maxBullets) {
                break;
            }


            yield return new WaitForSecondsRealtime(fireRate);

        } while(_action.IsPressed());
        
    }
}
