using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [Header("この武器のダメージ")]
    public int damage = 1;

    private int extraDamage = 0;

    //現在のダメージ
    public int CurrentDamage => damage + extraDamage;

    //バフを拾ってダメージ増加
    public void AddDamage(int amount)
    {
        extraDamage += amount;
    }
}





