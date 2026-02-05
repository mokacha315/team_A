using UnityEngine;
/// <summary>
/// 
/// </summary>
public class DropSwordItem : MonoBehaviour
{
    public WeaponData weaponToGive;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerAttack pa = col.GetComponent<PlayerAttack>();

            if (pa != null)
            {
                pa.EquipWeapon(weaponToGive);
            }

            Destroy(gameObject);
        }
    }
}