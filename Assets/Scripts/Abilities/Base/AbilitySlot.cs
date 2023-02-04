using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

public class AbilitySlot : MonoBehaviour
{
    public KeyCode key;
    public Ability currentAbility;

    public enum AbilityState {
        READY,
        ACTIVE,
        ON_COOLDOWN

    };

    private AbilityState currentAbilityState;
    private bool playerRooted = false;
    private float lastTime;
    private float remainingCooldown;

    private InputAction abilityAction;

    void Start() {
        RootsController.Instance.RootStart.AddListener(OnPlayerRootStart);
        RootsController.Instance.RootEnd.AddListener(OnPlayerRootEnd);
        currentAbilityState = AbilityState.READY;

        // bind ability to key
        abilityAction = new InputAction(
            type: InputActionType.Button,
            binding: GetActionBinding()
        );
        abilityAction.Enable();
    }

    string GetActionBinding() {
        if(key.ToString().ToLower().Contains("mouse")) {
            string buttonName;
            switch(key.ToString().ToLower()) {
                case "mouse2":
                    buttonName = "middleButton";
                break;
                case "mouse1":
                    buttonName = "rightButton";
                break;
                case "mouse0":
                    buttonName = "leftButton";
                break;
                default:
                    buttonName = "forwardButton";
                break;
            }
            return "<Mouse>/" + buttonName;
        } else {
            return "<Keyboard>/" + key.ToString().ToLower();
        }
    }

    void Update() {
        switch (currentAbilityState)
        {
            case AbilityState.READY:
                if(((!currentAbility.rootedAbility && !playerRooted) || (currentAbility.rootedAbility && playerRooted)) && abilityAction.WasPressedThisFrame()) {
                    Debug.Log("Ability used");
                    currentAbility.Activate(RootsController.Instance.gameObject, abilityAction);
                    remainingCooldown = currentAbility.cooldownTime;
                    lastTime = Time.time;
                    currentAbilityState = AbilityState.ON_COOLDOWN;
                }
                break;

            case AbilityState.ON_COOLDOWN:
                if(playerRooted && Time.time >= lastTime + remainingCooldown) {
                    Debug.Log("Ability ready");
                    currentAbilityState = AbilityState.READY;
                }
                if(playerRooted)
                    Debug.Log(currentAbility.cooldownTime - (Time.time - lastTime));
                break;

            default:
                break;
        }
    }

    public float GetRemainingCooldown() {
        return remainingCooldown;
    }

    public AbilityState GetAbilityState() {
        return currentAbilityState;
    }

    public float GetTotalCooldown() {
        return currentAbility.cooldownTime;
    }

    public float GetCooldownPercentage() {
        if(playerRooted)
            return (Time.time-lastTime+currentAbility.cooldownTime-remainingCooldown)/currentAbility.cooldownTime;
        
        return 1-remainingCooldown/currentAbility.cooldownTime;
    }

    void OnPlayerRootStart() {
        playerRooted = true;
        lastTime = Time.time;
    }

    void OnPlayerRootEnd() {
        playerRooted = false;
        remainingCooldown = remainingCooldown - (Time.time - lastTime);
    }
}
