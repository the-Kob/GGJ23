using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour
{
    public AbilitySlot abilitySlot;
    private AbilitySlot.AbilityState currentAbilityState;
    private RectTransform cooldownForeground;
    private RectTransform cooldownBackground;
    private bool playerRooted;
    private TextMeshProUGUI cooldownText;
    private TextMeshProUGUI abilityKeyText;

    private string storedAbilityKeyText = "";
    private string cooldownAbilityKeyText = "R";

    void Start() {
        RootsController.Instance.RootStart.AddListener(OnPlayerRootStart);
        RootsController.Instance.RootEnd.AddListener(OnPlayerRootEnd);
        cooldownBackground = (RectTransform)transform.Find("Cooldown background");
        cooldownForeground = (RectTransform)transform.Find("Cooldown foreground");
        
        cooldownText = transform.Find("Cooldown Text").GetComponent<TextMeshProUGUI>();
        abilityKeyText = transform.Find("Ability Key").GetComponent<TextMeshProUGUI>();
        storedAbilityKeyText = abilityKeyText.text;
        
        transform.Find("Ability Key").GetComponent<TextMeshProUGUI>().text = abilitySlot.key.ToString().ToUpper();

        cooldownBackground.gameObject.GetComponent<Image>().sprite = abilitySlot.currentAbility.abilityUI;
    }

    void Update() {
        currentAbilityState = abilitySlot.GetAbilityState();

        if(currentAbilityState == AbilitySlot.AbilityState.ON_COOLDOWN) {
            float percentage = abilitySlot.GetCooldownPercentage();
            cooldownForeground.sizeDelta = new Vector2(cooldownBackground.sizeDelta.x, cooldownBackground.sizeDelta.y*(1-percentage));
            
            cooldownText.text = ((1-percentage)*abilitySlot.GetTotalCooldown()).ToString("F1");
            abilityKeyText.text = cooldownAbilityKeyText;

        } else if(currentAbilityState == AbilitySlot.AbilityState.READY) {
            cooldownForeground.sizeDelta = Vector2.zero;
            
            cooldownText.text = "";
            abilityKeyText.text = storedAbilityKeyText;
        }

        
    }

    void OnPlayerRootStart() {
        playerRooted = true;
    }

    void OnPlayerRootEnd() {
        playerRooted = false;
    }
}
