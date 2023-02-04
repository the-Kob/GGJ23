using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CooldownIndicator : MonoBehaviour
{
    public AbilitySlot abilitySlot;
    private AbilitySlot.AbilityState currentAbilityState;
    private RectTransform cooldownForeground;
    private RectTransform cooldownBackground;
    private bool playerRooted;
    private TextMeshProUGUI cooldownText;

    void Start() {
        RootsController.Instance.RootStart.AddListener(OnPlayerRootStart);
        RootsController.Instance.RootEnd.AddListener(OnPlayerRootEnd);
        cooldownBackground = (RectTransform)transform.Find("Cooldown background");
        cooldownForeground = (RectTransform)transform.Find("Cooldown foreground");
        cooldownText = transform.Find("Cooldown Text").GetComponent<TextMeshProUGUI>();
        transform.Find("Ability Key").GetComponent<TextMeshProUGUI>().text = abilitySlot.key.ToString().ToUpper();
    }

    void Update() {
        currentAbilityState = abilitySlot.GetAbilityState();

        if(currentAbilityState == AbilitySlot.AbilityState.ON_COOLDOWN) {
            float percentage = abilitySlot.GetCooldownPercentage();
            cooldownForeground.sizeDelta = new Vector2(cooldownBackground.sizeDelta.x, cooldownBackground.sizeDelta.y*(1-percentage));
            cooldownText.text = ((1-percentage)*abilitySlot.GetTotalCooldown()).ToString("F1");

        } else if(currentAbilityState == AbilitySlot.AbilityState.READY) {
            cooldownForeground.sizeDelta = Vector2.zero;
            cooldownText.text = "";
        }

        
    }

    void OnPlayerRootStart() {
        playerRooted = true;
    }

    void OnPlayerRootEnd() {
        playerRooted = false;
    }
}
