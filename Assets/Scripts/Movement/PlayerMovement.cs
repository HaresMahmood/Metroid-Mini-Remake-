using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour 
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;

    private Controls controls;
    
    private new Rigidbody rigidbody;

    private float move;

    private int oldMove;

    private float Move
    {
        get { return move; }
        set { move = value; MoveCharacter(); }
    }

    private void Awake()
	{
		controls = new Controls();

        controls.Player.Jump.performed += ctx => StartCoroutine(Jump());
        controls.Player.Move.performed += ctx => Move = controls.Player.Move.ReadValue<float>();
        controls.Player.Move.canceled += ctx => Move = 0f;

        rigidbody = GetComponent<Rigidbody>();
	}

    private void MoveCharacter()
    {
        rigidbody.velocity = new Vector3(Move * speed, rigidbody.velocity.y, rigidbody.velocity.z);

        if ((int)Mathf.Clamp(move, -1, 1) != 0 && (int)Mathf.Clamp(move, -1, 1) != oldMove)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3((int)Mathf.Clamp(move, -1, 1), 0, 0));

            StartCoroutine(Turn());

            oldMove = (int)Mathf.Clamp(move, -1, 1);
        }

        if (rigidbody.velocity.x > 0.01 || rigidbody.velocity.x < -0.01)
        {
            GetComponent<Animator>().SetBool("isRunning", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isRunning", false);
        }
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

    private IEnumerator Jump()
    {
        if (rigidbody.velocity.y < 0.01)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);

            GetComponent<Animator>().SetTrigger("Jumping");

            yield return new WaitForSeconds(GetAnimationTime(GetComponent<Animator>()));

            GetComponent<Animator>().ResetTrigger("Jumping");
        }
    }

    private IEnumerator Turn()
    {
        GetComponent<Animator>().SetTrigger("Turning");

        yield return new WaitForSeconds(GetAnimationTime(GetComponent<Animator>()));

        GetComponent<Animator>().ResetTrigger("Turning");
    }

    public float GetAnimationTime(Animator animator)
    {
        AnimatorClipInfo[] currentClip = null;
        float waitTime = 0;

        currentClip = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip.Length > 0)
            waitTime = currentClip[0].clip.length;

        return waitTime;
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
