using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public CharacterController controller;
    public controlGame over; 

    public bool isWater;
    public bool hiting;
    public bool waitFor;
    public bool isDead;
    public bool isMobile;

    public float colliderRadius;
    public float damage = 20;
    public float health;

    private Animator anim;
    [SerializeField] private AudioSource passosAudio;
    [SerializeField] private AudioClip[] passosAudioClip;
    [SerializeField] private AudioClip[] passosAguaClip;

    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private AudioClip[] swordAudioClip;

    [SerializeField] private AudioSource deadAudio;
    [SerializeField] private AudioClip deadAudioClip;

    [SerializeField] private AudioSource impactAudio;
    [SerializeField] private AudioClip impactAudioClip;


    public List<Transform> enemyList = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead && over.isPause == false)
        {
            GetMouseInput();
        }
    }

    //método para andar e atacar

    public void GetMouseInput()
    {
        if (controller.isGrounded)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) && isMobile == false)
            {
                StartCoroutine("attack");
            }
        }

    }

    public void GeButton()
    {
        if(controller.isGrounded && !isDead)
        {
            if(isMobile == true)
            {
                StartCoroutine("attack");
            }
        }
    }


    //corrotina de ataque
    IEnumerator attack()
    {
        //Debug.Log("entrou no atack");
        if (!waitFor && !hiting)
        {
            waitFor = true;
            //Debug.Log("entrou no IF");
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);

            yield return new WaitForSeconds(0.9f);

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

            yield return new WaitForSeconds(0.9f);

            anim.SetInteger("transition", 0);

            anim.SetBool("attacking", false);

            waitFor = false;
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

        if(health <=0)
        {
            //player morre
            isDead = true;
            anim.SetTrigger("die");
            over.gameOver();
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

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "agua")
        {
            isWater = true;
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.tag == "agua")
        {
            isWater = false;
        }
    }

    private void passos()
    {
        if(isWater == true)
        {
            passosAudio.PlayOneShot(passosAguaClip[Random.Range(0, passosAguaClip.Length)]);
        }
        else
        {
            passosAudio.PlayOneShot(passosAudioClip[Random.Range(0, passosAudioClip.Length)]);
        }
        
    }

    private void sword()
    {
        swordAudio.PlayOneShot(swordAudioClip[Random.Range(0, swordAudioClip.Length)]);
    }

    private void impact()
    {
        swordAudio.PlayOneShot(impactAudioClip);
    }

    private void dead()
    {
        swordAudio.PlayOneShot(deadAudioClip);
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}
