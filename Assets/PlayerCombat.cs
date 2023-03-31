using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator anim;


    [Header("Attack Counter")]
    int attackCount = 0;
    int attackLimit = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
            if (attackCount == 0)
            {
                anim.Play("Punch 1");
                yield return new WaitForSeconds(.5f);
         
               
            }
            else if (attackCount == 1)
            {
                anim.Play("Roundhouse");
                yield return new WaitForSeconds(.5f);
             
              
            }
            else if (attackCount == 2)
            {
                anim.Play("Elbow");
                yield return new WaitForSeconds(1);

               
            }
            if (attackCount == 3)
            {
                anim.Play("SideKick");
                yield return new WaitForSeconds(.5f);

                
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
