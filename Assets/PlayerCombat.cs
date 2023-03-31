using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator anim;

    [Header("Bools")]
    bool canAttack;

    [Header("Attack Counter")]
    int attackCount = 0;
    int attackLimit = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PunchCombo());
    }
    IEnumerator PunchCombo()
    {
        print("punch");
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (attackCount == 0 && canAttack)
            {
                anim.Play("Punch 1");
                yield return new WaitForSeconds(2f);
                canAttack = true;

            }
            else if (attackCount == 1 && canAttack)
            {
                anim.Play("Roundhouse");
                yield return new WaitForSeconds(2f);
                canAttack = true;

            }
            else if (attackCount == 2 && canAttack)
            {
                anim.Play("Elbow");
                canAttack = false;
                yield return new WaitForSeconds(2f);
                canAttack = true;

               
            }
            else if (attackCount == 3 && canAttack)
            {
                anim.Play("SideKick");
                yield return new WaitForSeconds(2f);
                canAttack = true;

            }
            
            if(attackCount > attackLimit)
            {
                attackCount = 0;
            }
            else
            {
                attackCount++;
            }
        }
    }
}
