using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootedOverlay : MonoBehaviour
{
private Animator _animator;

    void Start()
    {
       _animator = gameObject.GetComponent<Animator>(); 
       
       RootsController.Instance.RootStart.AddListener(OnPlayerRootStart);
       RootsController.Instance.RootEnd.AddListener(OnPlayerRootEnd);
    }

    private void OnPlayerRootStart()
    {
        _animator.Play("Show");
    }

    private void OnPlayerRootEnd()
    {
        _animator.Play("Hide");
    }
}
