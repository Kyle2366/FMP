using UnityEngine;
public class FakeEnemyFInisher : MonoBehaviour
{
    Animator anim;
    public void Finisher() { anim.Play("Stealth Assassination"); }
}