using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public GameObject sword;       // Œ•ƒIƒuƒWƒFƒNƒg
    public float attackDelay = 0.2f;  // UŒ‚”»’è‚Ì‘±ŠÔ
    private bool isAttack = false;//UŒ‚’†‚©‚Ç‚¤‚©‚ğ‹L˜^

    public void Attack()
    {
        //UŒ‚’†‚Å‚Í‚È‚¢
        if (isAttack == false)
        {
            isAttack = true;//UŒ‚ƒtƒ‰ƒO
                            //Œ•‚ÅUŒ‚‚·‚é

        }
    }

}

//UŒ‚
