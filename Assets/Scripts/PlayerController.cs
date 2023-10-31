using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalMovement;
    float verticalMovement;
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rbDrag;
    public bool canMove;
    Vector3 moveDirection;

    [Header ("Ground Layer Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask groundLayerMask;
    bool isGrounded;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float airMultiplierSpeed;
    bool canJump;

    [Header("Camera")]
    [SerializeField] float camMultiplier;
    public bool canRotate;
    Camera cam;
    float MouseX;
    float MouseY;
    float xRotation;
    float yRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (canRotate)
        {
            CameraInput();
            CameraRotate();
        }
        if (!canMove) return;
        GroundCheck();
        MoveInput();
        ControlDrag();
        TurnJump();
        
        
    }
    private void FixedUpdate()
    {
        if (!canMove) return;
        MovePlayer();
        Jump(jumpForce);
    }

    void MoveInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    void MovePlayer()
    {

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed  * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed  * airMultiplierSpeed * 10f, ForceMode.Force);
        }
        Vector3 _moveVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if(_moveVelocity.magnitude>moveSpeed)
        {
            Vector3 _maxMoveVelocity = _moveVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(_maxMoveVelocity.x, rb.velocity.y, _maxMoveVelocity.z);
        }
    }

    public void StopPlayer()
    {
        rb.velocity = Vector3.zero;
    }

    void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag = rbDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    void TurnJump()
    {
        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            canJump = true;
        }
    }
    void Jump(float _jumpForce)
    {
        if(canJump)
        {
            canJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);
    }

    void CameraInput()
    {
        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += MouseX * camMultiplier;
        xRotation -= MouseY * camMultiplier;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    void CameraRotate()
    {
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    
}
