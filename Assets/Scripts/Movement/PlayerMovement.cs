using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour 
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityScale = 3;
    [SerializeField] private float rotationSpeed = 90;

    private Controls controls;
    private CharacterController controller;
    private Animator animator;

    private float move;
    private bool jump;

    private float MoveValue
    {
        set { move = value; SetMoveAnimation(); //ChangeOrientation();
        }
    }

    private Vector3 moveDirection;

    private void Awake()
	{
		controls = new Controls();

        controls.Player.Jump.performed += ctx => jump = true;
        controls.Player.Jump.canceled += ctx => jump = false;
        controls.Player.Move.performed += ctx => MoveValue = controls.Player.Move.ReadValue<float>();
        controls.Player.Move.canceled += ctx => MoveValue = 0f;

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
	}

    private void FixedUpdate()
    {
        moveDirection = new Vector3(move * speed, moveDirection.y, moveDirection.z);

        if (controller.isGrounded && jump)
        {
            moveDirection.y = jumpForce;
            jump = false;
        }

        transform.right = Vector3.Slerp(transform.right, Vector3.back * (int)Mathf.Clamp(move, -1, 1), 0.15f);

        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void ChangeOrientation()
    {
        //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 90 * (int)move, 0, 0), Time.deltaTime * 0.5f / 100);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.up * (int)move), speed * Time.deltaTime);

    }

    private void SetMoveAnimation()
    {
        animator.SetFloat("Move Speed", move);
    }

    /*
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


        //GetComponent<CharacterController>().Move(new Vector3(this.horizontal * moveTime * Time.deltaTime, 0, 0));
    }
    */

    private void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = jumpForce;

            moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }

        /*
        if (rigidbody.velocity.y < 0.01)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);

            GetComponent<Animator>().SetTrigger("Jumping");

            yield return new WaitForSeconds(GetAnimationTime(GetComponent<Animator>()));

            GetComponent<Animator>().ResetTrigger("Jumping");
        }
        */
    }

    private void Move()
    {

    }


    void OnEnable()
	{
		controls.Player.Enable();
	}

	void OnDisable()
	{
		controls.Player.Enable();
	}


    /*
	         //rigidbody.velocity = new Vector3(move * speed, rigidbody.velocity.y, rigidbody.velocity.z);

        //GetComponent<Animator>().SetFloat("Move Speed", move);

        //if ((int)Mathf.Clamp(move, -1, 1) != 0 && (int)Mathf.Clamp(move, -1, 1) != oldMove)
        //{
        //if ((int)Mathf.Clamp(move, -1, 1) != 0)
        //    transform.rotation = Quaternion.LookRotation(new Vector3((int)Mathf.Clamp(move, -1, 1), 0, 0));

        //    oldMove = (int)Mathf.Clamp(move, -1, 1);
        //}

        if (rigidbody.velocity.x > 0.01 || rigidbody.velocity.x < -0.01)
        {
            GetComponent<Animator>().SetBool("isRunning", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isRunning", false);
        }

    // Calculate how fast we should be moving
    Vector3 targetVelocity = new Vector3(move, 0, 0);
    targetVelocity = transform.TransformDirection(targetVelocity);
    targetVelocity *= speed;

    // Apply a force that attempts to reach our target velocity
    Vector3 velocity = rigidbody.velocity;
    Vector3 velocityChange = (targetVelocity - velocity);
    velocityChange.x = Mathf.Clamp(velocityChange.x, -speed, speed);
    velocityChange.y = 0;
    velocityChange.z = 0;
    rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

    rigidbody.AddForce(new Vector3(move* 100, 0, 0));


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
