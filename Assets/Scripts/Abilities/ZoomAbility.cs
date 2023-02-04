using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class ZoomAbility : Ability
{

    private PlayerInput _playerInput;
    private InputAction _action;

    private Transform sourceTransform;
    private Transform cameraTransform;

    public override void Activate(GameObject parent, InputAction action) {
        _playerInput = parent.GetComponent<PlayerInput>();
        _action = action;

        GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 17;


        RootsController.Instance.StartCoroutine(Fire());
    }

    IEnumerator Fire() {
        do {
            yield return new WaitForEndOfFrame();

        } while(_action.IsPressed());

        GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 40;

        
    }
}
