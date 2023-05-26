using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStates
{

    Idle,
    Move,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Patrol,
    Chase,
    Damaged,
    Search,
    Die,
    Jump,
    TakeDamage,
    Block,
    Finished,


}
public class EnemyController : MonoBehaviour
{
    // [SerializeField] public Transform leftEdge;
    // [SerializeField] public Transform rightEdge;
    [SerializeField] private Transform enemy;

    [Header("References")]
    public EnemyStates enemyState, previousState;
    PlayerCombat playerScript;
    GameObject player;


    [Header("Combat")]
    public float maxHealth = 100;
    public float currentHealth;
    public float damage;
    public float minDistance = 1f;
    public bool canAttack;
    public bool canMove;
    public float enemyDamage = 1f;
    public bool canDamage;

    [Header("Patrol")]
    public Vector3 destination;
    public Transform Player, patrol;
    public NavMeshAgent agent;
    public bool spotted;
    public float searchTime;



    [Header("animation")]
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        canDamage = false;  
        canMove = true;
        canAttack = true;   
        spotted = false;
        enemyState = EnemyStates.Patrol;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerCombat>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DoLogic();
        print("ENEMY HEALTH = " + currentHealth);
        
        print(enemyState);
        StopAttack();
      
    }

    void StopAttack()
    {
        if(playerScript.isAttacking)
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
    }

    public void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            enemyState = EnemyStates.Die;
        }
    }
    void DoLogic()
    {
        if(enemyState == EnemyStates.Idle)
        { 
           DoPatrol();
           CheckHealth();
        }

        if (enemyState == EnemyStates.Patrol)
        {
            
            DoPatrol();
            CheckHealth();

        }
        if (enemyState == EnemyStates.Chase)
        {
         
            DoChase();
            CheckForAttack();
            CheckHealth();

        }
        if (enemyState == EnemyStates.Damaged)
        {

            TakeDamage();
            CheckHealth();
        }
        if (enemyState == EnemyStates.Attack1)
        {
            CheckForAttack();
            CheckHealth();



        }
        if(enemyState==EnemyStates.Die)
        {
            Death();
        }
        if(enemyState==EnemyStates.Finished)
        {
            BeingFinished();
        }
    }

    void CheckForAttack()
    {
        if (canAttack && spotted == true)
        {
            switch (previousState)
            {
                case EnemyStates.Attack1:

                    enemyState = EnemyStates.Attack2;
                    previousState = enemyState;
                    anim.Play("Kicking");

                    Invoke("DamageEnded", .5f);
                    Invoke("AttackEnded", 3.5f);

                    print("current enemy state=" + enemyState);

                    break;
                case EnemyStates.Attack2:

                    enemyState = EnemyStates.Attack3;
                    previousState = enemyState;
                    anim.Play("Boxing");

                    Invoke("DamageEnded", .5f);
                    Invoke("AttackEnded", 2.5f);

                    print("current enemy state=" + enemyState);

                    break;
                    
                    case EnemyStates.Attack3:

                    enemyState = EnemyStates.Attack4;
                    previousState = enemyState;
                    anim.Play("Chapa 2");

                    Invoke("DamageEnded", .5f);
                    Invoke("AttackEnded", 1.5f);

                    print("current enemy state=" + enemyState);
                    break;
                    
                    case EnemyStates.Attack4:

                    enemyState = EnemyStates.Attack1;
                    previousState = enemyState;
                    anim.Play("Kicking");

                    Invoke("DamageEnded", .5f);
                    Invoke("AttackEnded", 2.5f);

                    print("current enemy state=" + enemyState);
                    break;
                default:

                  
                    enemyState = EnemyStates.Attack1;
                    previousState = enemyState;
                    anim.Play("Leg Sweep");
                    canMove = false;
                    canAttack = false;
                    Invoke("AttackEnded", 2.5f);

                    print("current enemy state=" + enemyState);
                    Invoke("CanAttack", 3.5f);
                    break;

            }
        }
       
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && canDamage)
        {
            col.gameObject.GetComponent<PlayerCombat>().TakeDamage();
        }

    }
    void AttackEnded()
    {
        enemyState = EnemyStates.Idle;
        canMove = true;
    }

    void DoPatrol()
    {
        if (spotted == false && canMove)
        {
            enemy.GetComponent<NavMeshAgent>().speed = 1.5f;
            anim.Play("Walking (2)");
            destination = patrol.position;
            agent.destination = destination;
        }
        if (spotted == true && canMove)
        {

           enemyState = EnemyStates.Chase;
        }
    }
    void DoChase()
    {
        if (spotted == true && canMove && canAttack == true)
        {
            enemy.GetComponent<NavMeshAgent>().speed = 3;
            anim.Play("Run (4)");
            print("CHASING");
            transform.LookAt(Player);
            destination = Player.position;
            agent.destination = destination;

            if (Vector3.Distance(enemy.transform.position, player.transform.position) < minDistance)
            {
                canMove = false;
                enemy.transform.LookAt(Player);
                enemyState = EnemyStates.Attack1;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spotted = true;
        }

    }
   

    public void TakeDamage()
    {
        anim.Play("Reaction");
        print("ENEMY HIT ");
        currentHealth -= playerScript.playerDamage;
        enemyState = EnemyStates.Patrol;
    }
    void Death()
    {
        canMove = false;
        anim.Play("Dying");
        canAttack = false;
        Invoke("deleteObject", 3f);
    }
    void deleteObject()
    {
        gameObject.SetActive(false);    
    }
    void CanAttack()
    {
        canAttack = true;
    }
    public void DamageStarted()
    {
        canDamage = true;
    }
    void DamageEnded()
    {
        canDamage = false;
    }
    public void BeingFinished()
    { 
        gameObject.SetActive(false );   

    }
}