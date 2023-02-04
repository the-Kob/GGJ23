using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class DashAbility : Ability {
    public float duration;
    public float movSpeedMult = 1;
    public float jumpBoostMult = 1;
    public float gravityMult = 1;

    private StarterAssets.ThirdPersonController _controller;

    public override void Activate(GameObject parent, InputAction action) {
        _controller = parent.GetComponent<StarterAssets.ThirdPersonController>();

        RootsController.Instance.StartCoroutine(Dash());
    }

    IEnumerator Dash() {
        _controller.MoveSpeed *= movSpeedMult;
        _controller.JumpHeight *= jumpBoostMult;
        _controller.Gravity *= gravityMult;

        yield return new WaitForSeconds(duration);

        _controller.MoveSpeed = 5;
        _controller.JumpHeight = 1.2f;
        _controller.Gravity = -15;

    }

}
