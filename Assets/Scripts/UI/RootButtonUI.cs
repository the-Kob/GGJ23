using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootButtonUI : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        
        RootsController.Instance.RootStart.AddListener(OnPlayerRootStart);
        RootsController.Instance.RootEnd.AddListener(OnPlayerRootEnd);
    }


    private void OnPlayerRootStart()
    {
        _animator.Play("Pressed");
    }

    private void OnPlayerRootEnd()
    {
        _animator.Play("Default");
    }

}
