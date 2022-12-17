using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: сАПЮРЭ АeЦ Б ОПШФЙЕ, АКНЙХПНБЮРЭ ЯРПЕКЭАС ОПХ АЕЦЕ

    #region

    /// тсмйжхъ онксвемхъ дюммшу н онгхжхх хцпнйю, дкъ оепедювх б 
    /// яйпхор сопюбкемхъ бпюцнл
    public static Transform instance;

    private void Awake()
    {
        instance = this.transform;
    }

    #endregion

    // оепелеммше дбхфемхъ
    [Header("Move Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    // оепелеммшецпюбхрюжхх
    [Header("Gravity")]
    [SerializeField] private float gravity;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isCharacterGrounded = false;
    private Vector3 velocity = Vector3.zero;

    private Animator anim;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void Update()
    {
        /// лернд, йюфдши йюдп, онксвюер бундмше дюммше йнмпнккепю(йкюбхьх WASD)
        /// х бшгшбюер лерндш дбхфемхъ, опшфйю, цпюбхрюжхх, аецю, юмхлюжхх.
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        if (moveZ < 0)  // ОПХ УНДЭАЕ МЮГЮД АКНЙЮЕЛ АЕЦ
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
        /// лернд янгдю╗р бейрнп дбхфемхъ х бшонкмъер дбхфемхе он мелс
        
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleRunning(float moveX, float moveZ)
    {
        /// лернд опнбепъер мюфюрхе мю SHIFT, 
        /// х лемъер хяонкмъелсч яйнпнярэ дбхфемхъ  

        if(Input.GetKeyDown(KeyCode.LeftShift))   // ОЕПЕЦМЮРЭ НЯЭ ХЙЯ Б АСК        ХКХ  ББЕЯРХ ОЕПЕЛЕММСЧ ПЕБЕПЯ ЯОХД
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
        /// лернд бшгшбючыхи юмхлюжхч бгюбхяхлнярх нр пефхлю 
        /// оепедбхфемхъ(ярюмдюпр(ярнхр мю лере), ундэаю, аец)

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
        /// лернд опнбепъер йюяюмхе й гелке(опхгелк╗ммнярэ)
        
        isCharacterGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }

    private void HandleGravity()
    {
        /// лернд опхлемъчыхи опхръфемхе й гелке оняке опшфйю

        if(isCharacterGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJumping()
    {
        /// лернд бшгшбючыхи опшфнй

        if(Input.GetKeyDown(KeyCode.Space) && isCharacterGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void GetReferences()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void InitVariables()
    {
        moveSpeed = walkSpeed;
    }

}