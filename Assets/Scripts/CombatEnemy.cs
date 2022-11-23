using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class CombatEnemy : MonoBehaviour
{
    [Header("Atributtes")]
    public float totalHearth = 100;
    public float attackDamage;
    public float movementeSpeed;
    public float lookRadius;
    public float colliderRadius = 2f;
    public float rotationSpeed;

    [Header("Components")]
    private Animator anim;
    private CapsuleCollider capsule;
    private NavMeshAgent agent;



    [Header("Other")]
    private Transform Player;

    private bool walking;
    private bool attacking;
    private bool hiting;

    private bool waitFor;
    public bool playerDead;


    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalHearth > 0)
        {
            float distance = Vector3.Distance(Player.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.isStopped = false;

                if (!attacking)
                {
                    agent.SetDestination(Player.position);
                    anim.SetBool("Run Forward", true);
                    walking = true;
                }

                //o  personagem está no raio de ação
                if (distance <= agent.stoppingDistance)
                {
                    //método atack
                    StartCoroutine("Attack");
                    lookTarget();
                }
                else
                {
                    attacking = false;
                }
            }
            else
            {
                //o personagem está fora do raio de ação
                walking = false;
                attacking = false;
                agent.isStopped = true;
                anim.SetBool("Run Forward", false);
            }
        }

    }

    IEnumerator Attack()
    {
        if (!waitFor && !hiting && !playerDead)
        {
            waitFor = true;
            attacking = true;
            walking = false;

            anim.SetBool("Run Forward", false);
            anim.SetBool("Claw Attack", true);
            yield return new WaitForSeconds(1f);
            GetPlayer();
            yield return new WaitForSeconds(0.5f);
            waitFor = false;
        }

        if (playerDead)
        {
            anim.SetBool("Run Forward", false);
            anim.SetBool("Claw Attack", false);
            walking = false;
            attacking = false;
            agent.isStopped = true;

        }

    }

    void GetPlayer()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
                //aplicasse dano
                c.gameObject.GetComponent<player>().GetHit(attackDamage);
                playerDead = c.gameObject.GetComponent<player>().isDead;
            }
        }

    }

    public void GetHit(float damage)
    {
        totalHearth -= damage;

        if (totalHearth > 0)
        {
            //inimigo vivo
            StopCoroutine("Attack");
            anim.SetTrigger("Take Damage");
            hiting = true;
            StartCoroutine("recoveryFromHit");
        }

        else
        {
            //inimigo morre
            anim.SetTrigger("Die");
        }
    }

    IEnumerator recoveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Run Forward", false);
        anim.SetTrigger("Claw Attack");
        hiting = false;
        waitFor = false;
    }

    void lookTarget()
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
