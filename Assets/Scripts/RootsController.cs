using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;

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
        /*
        if(_playerInput.actions["root"].WasPressedThisFrame()) {
            if(!_rooted && _controller.Grounded) {
                _rooted = true;
                _controller.MoveSpeed = 0;
                _controller.SprintSpeed = 0;
                //GetComponent<Rigidbody>().isKinematic = true;
                _currentRoots = Instantiate(rootsPrefab, this.transform);

                GameObject.Find("ArmAim").GetComponent<MultiAimConstraint>().weight = 1;
                GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 35;


                RootStart.Invoke();
            } else if (_rooted) {
                _rooted = false;
                _controller.MoveSpeed = 5;
                _controller.SprintSpeed = 5.335f;
                //GetComponent<Rigidbody>().isKinematic = true;
                Destroy(_currentRoots);
                _currentRoots = null;

                GameObject.Find("ArmAim").GetComponent<MultiAimConstraint>().weight = 0.5f;
                GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 40;


                RootEnd.Invoke();
            }

        }
        */
           
        if(_playerInput.actions["root"].WasPressedThisFrame() && _controller.Grounded) {
            _rooted = true;
            _controller.MoveSpeed = 0;
            _controller.SprintSpeed = 0;
            //GetComponent<Rigidbody>().isKinematic = true;
            GameObject.Find("RootLeft").GetComponent<Animator>().Play("Root");
            GameObject.Find("RootRight").GetComponent<Animator>().Play("Root");

            GameObject.Find("ArmAim").GetComponent<MultiAimConstraint>().weight = 1;
            GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 35;


            RootStart.Invoke();
        }

        if (_playerInput.actions["root"].WasReleasedThisFrame()) {
            _rooted = false;
            _controller.MoveSpeed = 5;
            _controller.SprintSpeed = 5.335f;
            //GetComponent<Rigidbody>().isKinematic = true;

            GameObject.Find("RootLeft").GetComponent<Animator>().Play("Unroot");
            GameObject.Find("RootRight").GetComponent<Animator>().Play("Unroot");

            GameObject.Find("ArmAim").GetComponent<MultiAimConstraint>().weight = 0.5f;
            GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = 40;


            RootEnd.Invoke();
        }

    }

    // EVENTS //////////////////////////////////////

    public static RootsController Instance;

    public UnityEvent RootStart;
    public UnityEvent RootEnd;

}
