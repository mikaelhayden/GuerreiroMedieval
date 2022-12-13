using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public CharacterController controller;

    //private bool isWalking;
    public bool hiting;
    public bool waitFor;
    public bool isDead;

    //public float speed;
    //public float gravity; //gravidade do player
    //public float smoothRotTime;
    public float colliderRadius;
    public float damage = 20;
    public float health;

    //public JumpPlayer jump;
   //public bool isJumping;

    //private float turnSmoothVelocity;

    private Animator anim;
    //private Transform cam;

    public List<Transform> enemyList = new List<Transform>();

    //Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        //cam = Camera.main.transform;
        //jump = GetComponent<JumpPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDead)
        {
            //move();
            GetMouseInput();
        }
    }

    /*void move()
    {
        if(controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * 0.04f, Input.GetAxis("Jump") * 0.2f, Input.GetAxis("Vertical") * 0.05f);
            moveDirection = transform.TransformDirection(moveDirection);
        }
        moveDirection.y -= 0.6f * Time.deltaTime;
        controller.Move(moveDirection);

        if(controller.isGrounded && jump.isJump == false)
        {
            //pega o input horizontal (teclas direita/esquerda)
            float horizontal = Input.GetAxisRaw("Horizontal");
            //pega um input vertical (teclas cima/baixo)
            float vertical = Input.GetAxisRaw("Vertical");

            //variável local que armazena os eixos horizontal e vertical
            Vector3 direction = new Vector3(horizontal, 0f , vertical);

            //verifica se o personagem está movimentando (caso o input seja maior que zero)
            if (direction.magnitude > 0)
            {

                if(!anim.GetBool("attacking"))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) 
                    {
                        anim.SetInteger("transition", 4);
                        speed = 6;
                    }
                    else
                    {
                        speed = 4;
                        anim.SetInteger("transition", 1);
                    }
                    anim.SetBool("walking", true);
                    //variável local que armazena a rotação e o angulo de visualização da camera
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                    //variável local que armazena a rotação porém mais suave
                    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                    //passamos a rotação suave ao personagem
                    transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                    //armazena direção com base na direção do mouse
                    moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;

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
    }*/

    //método para andar e atacar

    void GetMouseInput()
    {
        if(controller.isGrounded)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine("attack");

                /*if(anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if(!anim.GetBool("walking"))
                {
                    StartCoroutine("attack");
                }*/
            }
        }
    }

    //corrotina de ataque
    IEnumerator attack()
    {
        Debug.Log("entrou no atack");
        if (!waitFor && !hiting)
        {
            waitFor = true;
            Debug.Log("entrou no IF");
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);

            yield return new WaitForSeconds(0.4f);

            GetEnemiesList();

            foreach (Transform e in enemyList)
            {
                //aplica dano ao inimigo
                CombatEnemy enemy = e.GetComponent<CombatEnemy>();

                if (enemy != null)
                {
                    enemy.GetHit(damage);
                }
            }

            yield return new WaitForSeconds(1f);

            anim.SetInteger("transition", 0);

            anim.SetBool("attacking", false);

            waitFor = false; ;
        }
    }

    //lista de inimigos
    private void GetEnemiesList()
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

    //método para o player sofrer um dano
    public void GetHit(float damage)
    {
        health -= damage;
        if(health > 0)
        {
            //player ainda ta vivo
            StopCoroutine("attack");
            anim.SetInteger("transition", 3);
            hiting = true;
            StartCoroutine("RecoveryFromHit");
        }

        else
        {
            //player morre
            isDead = true;
            anim.SetTrigger("die");
        }
    }

    //player recuperando do dano
    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("transition", 0);
        hiting = false;
        waitFor = false;
        anim.SetBool("attacking", false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}
