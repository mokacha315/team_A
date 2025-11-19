using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int attackPower;
    public GameObject swordPrefab;
    public GameObject swordEffectPrefab;
}