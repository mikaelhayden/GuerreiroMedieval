using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private CharacterController controller;
    private bool isWalking;

    public float speed;
    public float gravity;
    public float smoothRotTime;
    public float colliderRadius;

    
    private float turnSmoothVelocity;

    private Animator anim;
    private Transform cam;

    public List<Transform> enemyList = new List<Transform>();

    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        GetMouseInput();
    }

    void move()
    {
        if(controller.isGrounded)
        {
            //pega o input horizontal (teclas direita/esquerda)
            float horizontal = Input.GetAxisRaw("Horizontal");
            //pega um input vertical (teclas cima/baixo)
            float vertical = Input.GetAxisRaw("Vertical");

            //variável local que armazena os eixos horizontal e vertical
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            //verifica se o personagem está movimentando (caso o input seja maior que zero)
            if (direction.magnitude > 0)
            {

                if(!anim.GetBool("attacking"))
                {
                    //variável local que armazena a rotação e o angulo de visualização da camera
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                    //variável local que armazena a rotação porém mais suave
                    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                    //passamos a rotação suave ao personagem
                    transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                    //armazena direção com base na direção do mouse
                    moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;

                    anim.SetInteger("transition", 1);

                    isWalking = true;
                }

                else
                {
                    anim.SetBool("walking", false);
                    moveDirection = Vector3.zero;
                }
      
            }
            //É executado quando o player está parado
            else if(isWalking)
            {
                    anim.SetInteger("transition", 0);
                    anim.SetBool("walking", false);
                    moveDirection = Vector3.zero;

                    isWalking = false;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        //move o personagem
        controller.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput()
    {
        if(controller.isGrounded)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if(!anim.GetBool("walking"))
                {
                    StartCoroutine("attack");
                }
            }
        }
    }

    IEnumerator attack()
    {
        anim.SetBool("attacking", true);
        anim.SetInteger("transition", 2);
        yield return new WaitForSeconds(0.4f);

        GetEnemiesList();

        foreach(Transform e in enemyList)
        {
            Debug.Log(e.name);
        }

        yield return new WaitForSeconds(1f);

        anim.SetInteger("transition", 0);

        anim.SetBool("attacking", false);

    }

    void GetEnemiesList()
    {
        enemyList.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if(c.gameObject.CompareTag("enemy"))
            {
                enemyList.Add(c.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}
