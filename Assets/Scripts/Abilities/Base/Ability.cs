using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public bool rootedAbility;

    public virtual void Activate(GameObject parent, InputAction action) {}

}
