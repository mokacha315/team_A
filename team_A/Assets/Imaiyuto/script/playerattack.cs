using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab;// 剣のプレハブ (常に表示される見た目)
    public GameObject sword_effectPrefab;//剣のエフェクトのプレハブ (攻撃判定を持つ)
    public float attackDuration = 0.2f;    // 攻撃判定の持続時間
    public Vector2 swordOffset = new Vector2(1.0f, 0f); // 基準のオフセット（右方向）
    public HeroController heroController;//HeroController参照取得

    private bool inAttack = false;
    private GameObject sword;
    private GameObject sword_effect;
    private Transform swordTransform; // sword の Transform
    private Transform effectTransform; // sword_effect の Transform


    void Start()
    {
        //HeroControllerへの参照取得
        if (heroController == null)
        {
            heroController = GetComponent<HeroController>();
        }
        // 剣と剣のエフェクトをプレイヤーの子オブジェクトとして生成
        sword = Instantiate(swordPrefab, transform);
        sword_effect = Instantiate(sword_effectPrefab, transform);
        swordTransform = sword.transform;
        effectTransform = sword_effect.transform;

        // 常に剣のモデルを表示する
        sword.SetActive(true);
        // 攻撃判定を持つエフェクトは初期は非表示
        sword_effect.SetActive(false);

        // 剣の初期位置をプレイヤーのそばに設定（オフセットを適用して自然に持つ）
        // 剣を持つ位置を調整 (例: プレイヤーから少し離れた位置)
        // ここでは、Attack()で行う角度計算を流用し、ニュートラルな位置に設定します
        if (heroController != null)
        {
            // heroControllerのangleZが0（右）または180（左）の場合に合わせる
            float angleRad = (heroController.angleZ != 0 ? heroController.angleZ : 0) * Mathf.Deg2Rad;
            Vector3 initialOffset = new Vector3(
                Mathf.Cos(angleRad) * (swordOffset.x * 0.5f), // 攻撃時より少し近く
                Mathf.Sin(angleRad) * (swordOffset.x * 0.5f),
                0
            );
            swordTransform.localPosition = initialOffset;
        }
        else
        {
            // heroControllerがない場合の初期位置
            swordTransform.localPosition = new Vector3(swordOffset.x * 0.5f, 0, 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            Attack();
        }

        if (heroController != null)
        {
            // 常に表示される剣の回転を設定
            sword.transform.rotation = Quaternion.Euler(0, 0, heroController.angleZ);

            // 常に表示される剣のZ座標を設定 (キャラクターの前に出す/後ろに回す)
            float swordz = -1; // 手前
            if (heroController.angleZ > 45 && heroController.angleZ < 150)
            {
                // 上向きの時は奥に回す
                swordz = 100;
            }
            else
            {
                swordz = -1;
            }

            // 剣の位置のZ軸だけを調整
            Vector3 currentPos = sword.transform.position;
            sword.transform.position = new Vector3(currentPos.x, currentPos.y, swordz);

            // エフェクトの回転は Update() では設定せず、Attack() で一度だけ設定します。
            // 常に表示される剣の位置を毎フレーム更新（静的なオフセットのみで）
            float angleRad = heroController.angleZ * Mathf.Deg2Rad;
            Vector3 initialOffset = new Vector3(
                Mathf.Cos(angleRad) * (swordOffset.x * 0.5f), // 攻撃時より少し近く
                Mathf.Sin(angleRad) * (swordOffset.x * 0.5f),
                0
            );
            swordTransform.localPosition = initialOffset;
        }
    }

    void Attack()
    {
        if (heroController == null) return;

        inAttack = true;
        // 剣のモデルは常に表示

        // 攻撃判定を持つエフェクトを表示
        sword_effect.SetActive(true);

        // エフェクトの回転をプレイヤーの向きに合わせて設定
        sword_effect.transform.rotation = Quaternion.Euler(0, 0, heroController.angleZ);

        // プレイヤーの向き（angleZ）に合わせてエフェクトの相対位置を計算（swordOffset.x分離れた位置）
        float angleRad = heroController.angleZ * Mathf.Deg2Rad + 45f;//斬撃の角度

        // エフェクトのオフセット（剣先、攻撃判定の位置）
        Vector3 rotatedOffset = new Vector3(
            Mathf.Cos(angleRad) * swordOffset.x,
            Mathf.Sin(angleRad) * swordOffset.x,
            0
        );

        // エフェクトのローカル座標を設定
        effectTransform.localPosition = rotatedOffset;

        // エフェクトのZ座標は剣と同じか、少し手前/奥
        Vector3 currentSwordPos = sword.transform.position;
        sword_effect.transform.position = new Vector3(
            currentSwordPos.x + rotatedOffset.x,
            currentSwordPos.y + rotatedOffset.y,
            currentSwordPos.z - 0.01f // 剣より少し手前に出す
        );


        Debug.Log($"攻撃開始！ エフェクトの位置（ローカル）: {effectTransform.localPosition} 角度: {heroController.angleZ}");

        Invoke(nameof(StopAttack), attackDuration);
    }

    void StopAttack()
    {
        // 剣のモデルは非表示にしない

        // 攻撃判定を持つエフェクトを非表示にする
        sword_effect.SetActive(false);
        inAttack = false;
        Debug.Log("攻撃終了！");
    }
}

