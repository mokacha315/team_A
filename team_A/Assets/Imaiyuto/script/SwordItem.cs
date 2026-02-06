using UnityEngine;

public class SwordItem : MonoBehaviour
{
    public WeaponData newWeapon;  //ドロップした武器データ

    public void Equip(GameObject player)
    {
        PlayerAttack atk = player.GetComponent<PlayerAttack>();

        if (atk != null)
        {
            atk.EquipWeapon(newWeapon);
            Debug.Log("武器を装備した！ → " + newWeapon.weaponName);
        }
        else
        {
            Debug.LogWarning("PlayerAttack が player に見つからない");
        }
    }
    //外部からWeaponDataに返す関数
    public WeaponData GetWeaponData()
    {
        return newWeapon;
    }
}