using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CombatEnemy : MonoBehaviour
{
    [SerializeField] player player1;

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

    public bool walking;
    public bool attacking;
    public bool hiting;

    public bool waitFor;
    public bool playerDead;
    public bool death;

    [Header("WayPoints")]
    public List<Transform> wayPoints = new List<Transform>();
    public int currentPathIndex;
    public float pathDistance;

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
        if (totalHearth > 0 && player1.isDead == false)
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
                anim.SetBool("Run Forward", false);
                walking = false;
                attacking = false;
                //agent.isStopped = true;
                moveTo();
            }
        }
        else
        {
            anim.SetBool("Run Forward", false);
            anim.SetBool("Claw Attack", false);
            walking = false;
            attacking = false;
            agent.isStopped = true;
        }

    }

    void moveTo()
    {
        if(wayPoints.Count > 0)
        {
            float distance = Vector3.Distance(wayPoints[currentPathIndex].position, transform.position);
            agent.destination = wayPoints[currentPathIndex].position;

            if (distance <= pathDistance)
            {
                //parte para o próximo ponto
                currentPathIndex = Random.Range(0, wayPoints.Count);
            }
            anim.SetBool("Run Forward", true);
            walking = true;
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
            yield return new WaitForSeconds(1.3f);
            GetPlayer();
            yield return new WaitForSeconds(1.5f);
            waitFor = false;
        }

        if(playerDead)
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

    IEnumerator die()
    {
        Debug.Log("esperando");
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
        
    }

    public void GetHit(float damage)
    {
        totalHearth -= damage;

        if (totalHearth > 0)
        {
            //inimigo vivo
            StopCoroutine("Attack");
            anim.SetBool("Take Damage", true);
            hiting = true;
            StartCoroutine("recoveryFromHit");
        }

        else
        {
            //inimigo morre
            anim.SetBool("Die", true);
            StartCoroutine("die");
        }
    }

    IEnumerator recoveryFromHit()
    {
        yield return new WaitForSeconds(2f);
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}
