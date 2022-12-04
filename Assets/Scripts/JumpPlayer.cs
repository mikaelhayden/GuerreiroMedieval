using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayer : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] bool isGround; //boleana para saber se o jogador está no chão ou não
    [SerializeField] public bool isJump;
    [SerializeField] float jumpmultiplier;

    [SerializeField] AnimationCurve jumpFallOff;

    private Animator anim;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Jump();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isJump == false)
        {
            isJump = true;
            StartCoroutine("JumpEvent");

        }
        else if(isGround == true && !anim.GetBool("attacking"))
        {
            anim.SetInteger("transition", 0);
            anim.SetBool("walking", false);
            moveDirection = Vector3.zero;
        }
        isGround = controller.isGrounded;
    }

    private IEnumerator JumpEvent()
    {
        float timeInAir = 0.0f;
        do
        {

            float jumpForce = jumpFallOff.Evaluate(timeInAir) * jumpmultiplier * Time.deltaTime;

            controller.Move(Vector3.up * jumpForce);
            anim.SetInteger("transition", 5);

            timeInAir += Time.deltaTime;

            isGround = controller.isGrounded;
            isJump = !isGround;

            yield return null;
        } while (isJump == true && controller.collisionFlags != CollisionFlags.Above);

        isJump = false;
    }
}
