using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(StarterAssets.ThirdPersonController))]
public class RootsController : MonoBehaviour
{
    public GameObject rootsPrefab;

    private StarterAssets.ThirdPersonController _controller; 
    private bool _rooted;
    private GameObject _currentRoots;
    private PlayerInput _playerInput;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        _controller = GetComponent<StarterAssets.ThirdPersonController>();
    }

    void Update()
    {
        if(_playerInput.actions["root"].WasPressedThisFrame()) {
            if(!_rooted && _controller.Grounded) {
                _rooted = true;
                _controller.MoveSpeed = 0;
                _controller.SprintSpeed = 0;
                //GetComponent<Rigidbody>().isKinematic = true;
                _currentRoots = Instantiate(rootsPrefab, this.transform);

                RootStart.Invoke();
            } else if (_rooted) {
                _rooted = false;
                _controller.MoveSpeed = 2;
                _controller.SprintSpeed = 5.335f;
                //GetComponent<Rigidbody>().isKinematic = true;
                Destroy(_currentRoots);
                _currentRoots = null;

                RootEnd.Invoke();
            }

        }
    }

    // EVENTS //////////////////////////////////////

    public static RootsController Instance;

    public UnityEvent RootStart;
    public UnityEvent RootEnd;

}
