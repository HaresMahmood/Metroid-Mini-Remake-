using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Fields

    private float movementSpeed;
    private bool jump;

    #endregion

    #region Properties

    private float MovementSpeed
    {
        set { movementSpeed = value;  Move(); }
    }

    #endregion

    #region Variables

    private Controls controls;
    private CharacterController2D controller;

    #endregion

    #region Miscellaneous Methods

    private void Move()
    {
        
    }

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Awake()
    {
        controls = new Controls();

        controls.Player.Jump.performed += ctx => jump = true;
        controls.Player.Jump.canceled += ctx => jump = false;
        controls.Player.Move.performed += ctx => MovementSpeed = controls.Player.Move.ReadValue<float>();
        controls.Player.Move.canceled += ctx => MovementSpeed = 0;

        controller = GetComponent<CharacterController2D>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        controller.Move(movementSpeed, false, jump);
    }

    #endregion
}

