using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum States
{
    Idle,
    Move,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Die,
    Jump,
    Over,
    TakeDamage,
    Block,
}

public class PlayerCombat : MonoBehaviour
{
    public float speed = 1f;
    public Animator animator;
    public bool canMove;
    public CharacterController controller;

    [Header("References")]
    public Animator anim;
    EnemyController enemyScript;
    GameObject enemy;
    public float minDistance = 10f;
    float yRotation = 0f;
    public int respawn;
    public GameObject finisher;
    LayerMask enemyChar;
    public Transform playerPos;
    public Transform enemyPos;
    public GameObject m_GotHitScreen;
    public GameObject diedScreen;
    public GameObject endOfGameScreen;
    public GameObject endOfGame;
    States playerState, previousState;
    public GameObject hintScreen;
    public GameObject audioManager;
    AudioManager aM;

    [Header("RayCast")]
    Ray ray;
    bool rayhit;

    [Header("Bools")]
    bool canAttack;
    public bool isAttacking;
    public bool canFinish;
    public bool isGrounded;
    [Header("Damage")]
    public float playerDamage = 1f;
    public bool canDamage;
    public float playerHealth;
    public float PlayerCurrentHealth;

    public string tagToDectect = "Enemy";
    public GameObject[] allEnemies;
    public GameObject closestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        diedScreen.SetActive(false);    
        allEnemies = GameObject.FindGameObjectsWithTag(tagToDectect);
        ray = new Ray(transform.position, transform.forward);
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        isAttacking = false;
        canAttack = true;
        previousState = States.Idle;
        PlayerCurrentHealth = playerHealth;
    }
    private void FixedUpdate()
    {
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.up * -9.81f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(m_GotHitScreen != null)
        {
            if (m_GotHitScreen.GetComponent<Image>().color.a > 0)
            {
                var color = m_GotHitScreen.GetComponent<Image>().color;

                color.a -= .01f;

                m_GotHitScreen.GetComponent<Image>().color = color;
            }
        }
        closestEnemy = ClosestEnemy();
        print(closestEnemy.name);
        DoLogic();
        Tracking();
        Finisher();
        DisplayFinisher();
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" && canDamage)
        {
            col.gameObject.GetComponent<EnemyController>().TakeDamage();
            col.gameObject.GetComponent<EnemyController>().enemyState = EnemyStates.Damaged;
        }
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "GameEnd")
        {
            EndOfGame();
      
        }
    }

    GameObject ClosestEnemy()
    {
        GameObject closestHere = gameObject;
        float leastDistance = Mathf.Infinity;

        foreach(var enemy in allEnemies)
        {
            float distanceHere = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceHere < leastDistance)
            {
                leastDistance = distanceHere;
                closestHere = enemy;
            }
        }
        return closestHere;
    }


    void DoLogic()
    {
        if (playerState == States.Idle)
        {
            MovePlayer();
            CheckForAttack();
            Die();
        }

        if (playerState == States.Move)
        {
            MovePlayer();
            CheckForAttack();
            Die();
        }
        if(playerState == States.Die)
        {

        }
        if(playerState == States.Over)
        {

        }
    }
   
    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        animator.SetFloat("x", x);
        animator.SetFloat("y", z);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = 3f;
        else
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
                speed = 1f;
        }
    }

    void CheckForAttack()
    {   
        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            print("previous state=" + previousState);
            isAttacking = true;
            switch (previousState)
            {
                case States.Attack1:
                    playerState = States.Attack2;
                    previousState = playerState;
                    anim.Play("Roundhouse");
               
                    Invoke("DamageEnded", .5f);
                    if(closestEnemy.GetComponent<EnemyController>().enemyState == EnemyStates.Damaged)
                    {
                        Invoke("Attackended", 1.4f);
                    }
                    else
                    {
                        Invoke("AttackEnded", 2.4f);
                    }

                    Invoke("DamageEnded", .5f);
                    break;
                case States.Attack2:
                    
                    playerState = States.Attack3;
                    previousState = playerState;
                    anim.Play("Elbow");
                    if (closestEnemy.GetComponent<EnemyController>().enemyState == EnemyStates.Damaged)
                    {
                        Invoke("Attackended", .6f);
                    }
                    else
                    {
                        Invoke("AttackEnded", 1.4f);
                    }

                    Invoke("DamageEnded", .5f);
                    break;
                case States.Attack3:
                    
                    playerState = States.Attack4;
                    previousState = playerState;
                    anim.Play("SideKick");
                    if (closestEnemy.GetComponent<EnemyController>().enemyState == EnemyStates.Damaged)
                    {
                        Invoke("Attackended", 1f);
                    }
                    else
                    {
                        Invoke("AttackEnded", 2.1f);
                    }
                    Invoke("DamageEnded", .5f);
                   
              
                    print("current state=" + playerState);
                    break;
                case States.Attack4:
                    
                    playerState = States.Attack1;
                    previousState = playerState;
                    anim.Play("Punch 1");
                    if (closestEnemy.GetComponent<EnemyController>().enemyState == EnemyStates.Damaged)
                    {
                        Invoke("Attackended", .6f);
                    }
                    else
                    {
                        Invoke("AttackEnded", 1.2f);
                    }
                    Invoke("DamageEnded", .5f);
            
    
                    print("current state=" + playerState);
                    break;
                default:
                    
                    playerState = States.Attack1;
                    previousState = playerState;
                    anim.Play("Punch 1");
                    if (closestEnemy.GetComponent<EnemyController>().enemyState == EnemyStates.Damaged)
                    {
                        Invoke("Attackended", .6f);
                    }
                    else
                    {
                        Invoke("AttackEnded", 1.2f);
                    }
                    Invoke("DamageEnded", .5f);
                
  
                    print("current state=" + playerState);
                    break;
            }
        }
    }

    void AttackEnded()
    {
        playerState = States.Idle;
        isAttacking = false;
    }
    
    public void DamageStarted()
    {
        canDamage = true;
    }
    void DamageEnded()
    {
        canDamage = false;
    }

    public void TakeDamage()
    {
        var color = m_GotHitScreen.GetComponent<Image>().color;
        color.a = .8f;

        m_GotHitScreen.GetComponent<Image>().color = color;

        print("Player HIT ");
        print("PLAYER HEALTH = "+PlayerCurrentHealth);
        float enemyDamage = closestEnemy.GetComponent<EnemyController>().enemyDamage;
        PlayerCurrentHealth -= enemyDamage;
    }
    public void CheckHealth()
    {
        if (PlayerCurrentHealth <= 0)
        {
            anim.Play("Death");
            Invoke("Death", 3f);
        }
    }

    void Tracking()
    {
        if (Input.GetKey(KeyCode.CapsLock))
        {
            transform.LookAt(closestEnemy.transform.position, Vector3.up);
        }
    }
    void DisplayFinisher()
    {
        float dist = Vector3.Distance(playerPos.GetComponent<CharacterController>().transform.position, closestEnemy.transform.position);

        if (closestEnemy.GetComponent<EnemyController>().spotted == false && dist<minDistance )
        {
            canFinish = true;
        }
        else
        {
            canFinish= false;
            hintScreen.SetActive(false);
        }
        if (canFinish)
        {
            hintScreen.SetActive(true);
        }
    }
    void Finisher()
    {
        if (Input.GetKeyDown(KeyCode.E) && closestEnemy.GetComponent<EnemyController>().spotted == false)
        {


            float dist = Vector3.Distance(playerPos.GetComponent<CharacterController>().transform.position, closestEnemy.transform.position);

            print("dist=" + dist + "  mindist=" + minDistance);

            if (dist < minDistance)
            {
                closestEnemy.GetComponent<EnemyController>().canAttack = false;
                closestEnemy.GetComponent<EnemyController>().canMove = false;
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                finisher.SetActive(true);
                canMove = false;
                closestEnemy.GetComponent<EnemyController>().enemyState = EnemyStates.Finished;
                //gameObject.GetComponentInChildren<FakeEnemyFInisher>().Finisher();
                Invoke("FinisherEnd", 7f);
            }
        }
    }

    public void FinisherEnd()
    {
        canMove = true;
        print("MESH ON");
        finisher.SetActive(false);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }
    public void Die()
    {
        if (PlayerCurrentHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            playerState = States.Die;
            diedScreen.SetActive(true);
            
        }
    }
    public void EndOfGame()
    {
        endOfGameScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        playerState = States.Over;
    }
}