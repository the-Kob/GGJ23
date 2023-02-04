using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class AR_Ability : Ability
{
    public GameObject bullet;
    public float fireRate;
    public float bulletSpeed;
    public float bulletDamage;
    public string sourceTransformName = "Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm/Right_Hand";

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
        Vector3 target;
        do {
            // aim from camera

            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {
                Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                target = hit.point;
            } else {
                target = cameraTransform.position + cameraTransform.TransformDirection(Vector3.forward);
            }
            
            // fire from source
            //TODO: missing vfx 
            bulletBuffer = Instantiate(bullet, sourceTransform.position, Quaternion.identity);
            bulletBuffer.GetComponent<Bullet>().SetDamage(bulletDamage);
            bulletBuffer.GetComponent<Bullet>().SetSpeed(bulletSpeed);
            bulletBuffer.transform.LookAt(hit.point);

            //Destroy(bulletBuffer, 5f); //max mifespan

            Debug.Log(Time.time);


            yield return new WaitForSecondsRealtime(fireRate);

        } while(_action.IsPressed());
        
    }
}
