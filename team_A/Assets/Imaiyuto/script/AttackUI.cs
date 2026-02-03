using UnityEngine;
using UnityEngine.UI;

public class AttackUI : MonoBehaviour
{
    public Image tensImage;   // 10の位
    public Image onesImage;   // 1の位
    public Sprite[] numbers; // 0〜9

    PlayerAttack player;
    
    /// <summary>
    /// 起動時の攻撃力を参照している
    /// </summary>
    void Start()
    {
        // 起動時にシーン内から PlayerAttack を自動で探して記憶する
        player = FindObjectOfType<PlayerAttack>();
    }

    /// <summary>
    /// 攻撃力を表示するためのもの
    /// </summary>
    void Update()
    {
        int baseAtk;

        if (player.currentWeapon != null)
        {
            baseAtk = player.attackPower;
        }
        else
        {
            baseAtk = 1;
        }

        int totalAtk = baseAtk + player.extraDamage;
        totalAtk = Mathf.Clamp(totalAtk, 0, 99);

        int tens = totalAtk / 10;
        int ones = totalAtk % 10;

        if (tens == 0)
        {
            tensImage.gameObject.SetActive(false);
        }
        else
        {
            tensImage.gameObject.SetActive(true);
            tensImage.sprite = numbers[tens];
        }

        onesImage.sprite = numbers[ones];
    }

        
 }