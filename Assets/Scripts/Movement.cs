using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    private XRNode _input;
    private CharacterController _controller;
    private float _playerSpeed = 2.0f;
    private Vector2 _playerVelocity;
    private Camera _camera;
    private float _mouseX;
    private float _mouseY = 0;

    //Rotation Sensitivity
    [SerializeField]
    [Range(0.1f,3)]
    private float _rotationSensitivity = 0.5f;
    private float _minAngle = -45.0f;
    private float _maxAngle = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        this._controller = gameObject.AddComponent<CharacterController>();
        this._camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBody();
        MoveHead();
    }

    /// <summary>
    /// Update the movement of the body based on the controller
    /// </summary>
    private void MoveBody()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveDirectionCamera = this._camera.transform.TransformDirection(moveDirection);
        moveDirectionCamera.y = 0;
        this._controller.Move(moveDirectionCamera * Time.deltaTime * _playerSpeed);

        gameObject.transform.position = moveDirection;
        this._controller.Move(_playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Update the movement of the head based on the controller
    /// </summary>
    private void MoveHead()
    {
        this._mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        this._mouseX *= this._rotationSensitivity;
        this._mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        this._mouseY *= this._rotationSensitivity;

        this._camera.transform.localRotation = Quaternion.Euler(
            new Vector4(Mathf.Clamp(-1f * (this._mouseY * 180f), this._minAngle, this._maxAngle), _mouseX * 360f, transform.localRotation.z));
    }

}

