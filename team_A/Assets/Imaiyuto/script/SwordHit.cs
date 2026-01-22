using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public int damage = 1;
    private PlayerAttack player;
    /// <summary>
    /// Œ»İ‚ÌUŒ‚—Íæ“¾
    /// </summary>
    void Awake()
    {
        player = FindObjectOfType<PlayerAttack>();
    }
    /// <summary>
    /// “G‚É—^‚¦‚éƒ_ƒ[ƒWŒvZ
    /// </summary>
    public int CurrentDamage
    {
        get
        {
            if (player == null) return damage;
            return damage + player.extraDamage;
        }
    }
}
