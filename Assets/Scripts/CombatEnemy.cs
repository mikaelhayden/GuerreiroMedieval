using UnityEngine;
using UnityEngine.AI;

public class CombatEnemy : MonoBehaviour
{
    [Header("Atributtes")]
    public float totalHearth;
    public float attackDamage;
    public float movementeSpeed;
    public float lookRadius;

    [Header("Components")]
    private Animator anim;
    private CapsuleCollider capsule;
    private NavMeshAgent agent;

    [Header("Other")]
    private Transform Player;


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
        float distance = Vector3.Distance(Player.position, transform.position);

        if (distance <= lookRadius)
        {
            //o  personagem está no raio de ação
            Debug.Log("Dentro");
        }
        else
        {
            //o personagem está fora do raio de ação
            Debug.Log("Fora");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
