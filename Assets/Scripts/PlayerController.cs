using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: ������ �e� � ������, ����������� �������� ��� ����

    #region

    /// ������� ��������� ������ � ������� ������, ��� �������� � 
    /// ������ ���������� ������
    public static Transform instance;
    public static GameObject player;

    private void Awake()
    {
        instance = this.transform;
        player = this.gameObject;
    }

    #endregion

    // ���������� ��������
    [Header("Move Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    // ��������������������
    [Header("Gravity")]
    [SerializeField] private float gravity;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isCharacterGrounded = false;
    private Vector3 velocity = Vector3.zero;

    private Animator anim;
    private PlayerStats stats;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void Update()
    {
        /// �����, ������ ����, �������� ������� ������ ����������(������� WASD)
        /// � �������� ������ ��������, ������, ����������, ����, ��������.
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        if (moveZ < 0)  // ��� ������ ����� ������� ���
            moveSpeed = walkSpeed;
        //print(controller.velocity.magnitude);

        HandleIsGrounded();
        HandleJumping();
        HandleGravity();

        HandleRunning(moveX, moveZ);

        HandleMovement(moveX, moveZ);
        HandleAnimations();
    }

    private void HandleMovement(float moveX, float moveZ)
    {
        /// ����� ������� ������ �������� � ��������� �������� �� ����
        
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        if(!stats.IsDead())
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleRunning(float moveX, float moveZ)
    {
        /// ����� ��������� ������� �� SHIFT, 
        /// � ������ ����������� �������� ��������  

        if(Input.GetKeyDown(KeyCode.LeftShift))   // ��������� ��� ��� � ���        ���  ������ ���������� ������ ����
        {
            moveSpeed = runSpeed;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
    }

    private void HandleAnimations()
    {
        /// ����� ���������� �������� ������������ �� ������ 
        /// ������������(��������(����� �� ����), ������, ���)

        if (moveDirection == Vector3.zero)
        {
            anim.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
        }
        else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        }
    }

    private void HandleIsGrounded()
    {
        /// ����� ��������� ������� � �����(������˨������)
        
        isCharacterGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }

    private void HandleGravity()
    {
        /// ����� ����������� ���������� � ����� ����� ������

        if(isCharacterGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJumping()
    {
        /// ����� ���������� ������

        if(Input.GetKeyDown(KeyCode.Space) && isCharacterGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void GetReferences()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    private void InitVariables()
    {
        moveSpeed = walkSpeed;
    }

}