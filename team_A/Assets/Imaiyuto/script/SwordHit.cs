using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public int damage = 1; // 武器の基本ダメージ
    public PlayerAttack player; // プレイヤーの参照
    void Start()
    {
        player = FindObjectOfType<PlayerAttack>();
    }
    public int CurrentDamage => damage + player.extraDamage;
}




