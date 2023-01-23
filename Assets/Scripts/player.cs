using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public CharacterController controller;

    public bool hiting;
    public bool waitFor;
    public bool isDead;

    public float colliderRadius;
    public float damage = 20;
    public float health;


    private Animator anim;

    public List<Transform> enemyList = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDead)
        {
            GetMouseInput();
        }
    }

    //método para andar e atacar

    void GetMouseInput()
    {
        if(controller.isGrounded)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine("attack");
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
