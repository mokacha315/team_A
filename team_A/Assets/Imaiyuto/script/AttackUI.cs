using UnityEngine;
using UnityEngine.UI;

public class AttackUI : MonoBehaviour
{
    private PlayerAttack player;
    public Text attackText;
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
            if (player != null && attackText != null)
            {
                int baseAtk = 0;

                // 武器データが存在し、かつ参照が生きているかチェック
                if (player.currentWeapon != null)
                {
                    baseAtk = player.attackPower;
                }
                else
                {
                    // 武器がない場合や初期化前の暫定値
                    baseAtk = 1;
                }

                int totalAtk = baseAtk + player.extraDamage;
                attackText.text = "ATK:" + totalAtk;
            }
        }
    }