using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour 
{
	private Controls controls;
    
    private float horizontal;
    private Animator anim;

    [SerializeField] private float moveTime = 10f;
    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private bool readyToClear;

    private void Awake()
	{
		controls = new Controls();
	}

    private void Update()
    {
        ClearInput();
        Move();
    }

    void FixedUpdate()
    {
        // Set a flag that lets inputs to be cleared out during the next Update().
        // This ensures that all code gets to use the current inputs.
        readyToClear = true;
    }

    private void Move()
    {
        //=================================================
        // Horizontal
        //=================================================

        // Accumulate axis input
        // These values can be e.g. -2, -1, 0, 1, 2

        float horizontal = 0;

        if (this.horizontal != 0)
        {
           horizontal = this.horizontal;
        }

        this.horizontal += controls.Player.Move.ReadValue<float>();

        if (this.horizontal != 0 && horizontal == 0)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(this.horizontal - horizontal > 0 ? Vector3.right : Vector3.left), 0.1f);
        }

        //transform.Rotate(Vector3.right, -(int)( * 10f * Time.deltaTime, Space.Self);

        //Debug.Log(this.horizontal * moveTime * Time.deltaTime);
        //anim.SetFloat("InputX", this.horizontal, HorizontalAnimSmoothTime, Time.deltaTime * 2f);


        GetComponent<CharacterController>().Move(new Vector3(this.horizontal * moveTime * Time.deltaTime, 0, 0));
    }

    // Clear input, if we are ready
    private void ClearInput()
    {
        // If we are not ready to clear input, exit
        if (!readyToClear) return;

        // Reset all axis
        horizontal = 0f;
        readyToClear = false;    }


    void OnEnable()
	{
		controls.Player.Move.Enable();
	}

	void OnDisable()
	{
		controls.Player.Move.Enable();
	}


	/*
	public float Velocity;
    [Space]

	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    public float verticalVel;
    private Vector3 moveVector;



    // Use this for initialization
    void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		InputMagnitude ();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);


    }

    void PlayerMoveAndRotation() {
		

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
		}
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

	void InputMagnitude() {
		//Calculate Input Vectors
		

		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player

		if (Speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}
	*/
}
