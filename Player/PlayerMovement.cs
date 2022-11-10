using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform camTransform;
    public CharacterController controller;
    public float speed = 5f;

    public Animator anim;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public HumanSurvivor combatInfo;

    [Header("MobileControls")]
    public bool usingMobile;
    public Joystick moveJoystick;
    public Joystick lookJoystick;

    [SerializeField] private LayerMask groundMask;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        combatInfo = GetComponent<HumanSurvivor>();
    }
    private void Update()
    {
        if (combatInfo.alive && !usingMobile)
        {
            //input from player, movement
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            anim.SetFloat("movement", input.magnitude);
            if (input.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection * speed * Time.deltaTime);
            }

            //look at mouse
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, groundMask))
            {
                transform.LookAt(hit.point);
            }
        }
        else if(combatInfo.alive && usingMobile)
        {
            //MovePlayer
            Vector3 input = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical).normalized;
            anim.SetFloat("movement", input.magnitude);
            if (input.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection * speed * Time.deltaTime);
            }
            //Rotate Player & call attack action
            if(lookJoystick.Horizontal!=0 || lookJoystick.Vertical!=0)
            {
                float heading = Mathf.Atan2(lookJoystick.Horizontal, lookJoystick.Vertical);
                transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);

                GetComponent<EquippedWeapon>().AttackAction();
            }

        }
    }
}

