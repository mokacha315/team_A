using UnityEngine;
using UnityEngine.UI;
public class AttackUI : MonoBehaviour
{
    public SwordHit swordHit;
    public Text attackText;


    void Update()
    {
        SwordHit currentSword = FindObjectOfType<SwordHit>();

        if (currentSword != null)
        {
            attackText.text = "ATK:" + currentSword.CurrentDamage;
        }
    }
    }

