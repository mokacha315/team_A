using UnityEngine;

using UnityEngine;

public class BuffItem : MonoBehaviour
{
    public int attackUpValue = 2;

    public void ApplyBuff(GameObject player)
    {
        PlayerAttack atk = player.GetComponent<PlayerAttack>();
        if (atk != null)
        {
            atk.currentWeapon.attackPower += attackUpValue;
            Debug.Log("攻撃力アップ！ +" + attackUpValue);
        }
    }
}