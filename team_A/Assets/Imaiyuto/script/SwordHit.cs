using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public int damage = 1;
    private PlayerAttack player;

    void Awake()
    {
        player = FindObjectOfType<PlayerAttack>();
    }

    public int CurrentDamage
    {
        get
        {
            if (player == null) return damage;
            return damage + player.extraDamage;
        }
    }
}
