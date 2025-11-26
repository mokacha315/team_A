using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;//持ってる剣の名前
    public int attackPower;//攻撃力
    public float attackDuration;//攻撃判定の持続時間
    public float attackCooldown; //攻撃のクールタイム（秒）
    public GameObject swordPrefab;//今もってる剣のプレハブ
    public GameObject swordEffectPrefab;//今もってる剣のエフェクトプレハブ
   
}