using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    /// — –»œ“  ŒÕ“–ŒÀ»–”ﬁŸ»… œŒ¬≈ƒ≈Õ»≈ œ–Œ“»¬Õ» ¿
    /// (¿Õ»Ã¿÷»ﬂ, œŒ¬Œ–Œ“, ƒ¬»∆≈Õ»≈)

    [SerializeField] private float stoppingDistance = 3;
    private float timeOfLastAttack = 0;
    private bool hasStopped = false;
    private bool seePlayer = false;


    private NavMeshAgent agent = null;
    private Animator anim = null;
    private Rigidbody body = null;
    private ZombieStats stats = null;
    private Transform target;

    public GameObject player;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        if (seePlayer) 
        {
            Debug.Log(agent.velocity.magnitude);
            MoveToTarget();
        }
        anim.SetFloat("Speed", agent.velocity.magnitude);
        
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
        //anim.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);
        //anim.SetFloat("Speed", body.velocity.magnitude);
        RotateToTarget();

        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= agent.stoppingDistance)
        {
            anim.SetFloat("Speed", 0f);
            //Attack
            if(!hasStopped)
            {
                hasStopped = true;
                timeOfLastAttack = Time.time;
            }

            if(Time.time >= timeOfLastAttack + stats.attackSpeed)
            {
                timeOfLastAttack = Time.time;
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                AttackTarget(targetStats);
            }
        }
        else
        {
            if(hasStopped)
            {
                hasStopped = false;
            }
        }
    }

    private  void RotateToTarget()
    {
        //transform.LookAt(target);

        Vector3 direction = target.position - transform.position;
        direction.y = 0;    // ·ÎÓ˜ËÏ ‚‡˘ÂÌËÂ ÔÓ ”
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    private void AttackTarget(CharacterStats statsToDamage)
    {
        anim.SetTrigger("attack");
        stats.DealDamage(statsToDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            seePlayer = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            seePlayer = false;
        }
            
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody>();
        stats = GetComponent<ZombieStats>();
        target = PlayerController.instance;
        player = PlayerController.player;
    }

    
}
