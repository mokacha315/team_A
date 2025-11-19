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
    public int AttackPower = 1;//攻撃力

    private bool inAttack = false;
    private GameObject sword;
    private GameObject sword_effect;
    private Transform swordTransform; // sword の Transform
    private Transform effectTransform; // sword_effect の Transform
    private SpriteRenderer swordSpriteRenderer; // ★ 追加: 剣のSpriteRendererをキャッシュ


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

        // ★ 追加: 剣のSpriteRendererを取得してキャッシュ
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        if (swordSpriteRenderer == null)
        {
            Debug.LogError("Sword prefab must have a SpriteRenderer component.");
        }

        // 常に剣のモデルを表示する
        sword.SetActive(true);
        // 攻撃判定を持つエフェクトは初期は非表示
        sword_effect.SetActive(false);

        // 剣の初期位置をプレイヤーのそばに設定（オフセットを適用して自然に持つ）
        if (heroController != null)
        {
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
            // ★ 修正: Y軸回転 (flipY) のロジックを削除し、SpriteRenderer.flipX を使用
            float finalAngleZ = heroController.angleZ;
            // float flipY = 0f; // この行は不要

            if (swordSpriteRenderer != null)
            {
                if (heroController.direction == 0) // direction == 0は下
                {
                    swordSpriteRenderer.flipX = true; // 剣のSpriteをX軸で反転
                    swordSpriteRenderer.flipY = false;
                }
                else if (heroController.direction == 1)//左向き
                {
                    swordSpriteRenderer.flipX = false; // 剣のSpriteをX軸で反転
                    swordSpriteRenderer.flipY = true;
                }

                else if (heroController.direction == 2)//上向き
                {
                    swordSpriteRenderer.flipY = false;
                }
                else//右
                {
                    swordSpriteRenderer.flipX = false;
                    swordSpriteRenderer.flipY = false;

                }


            }

            // 剣の回転はZ軸のみで制御し、Y軸は常に0に
            float swordBaseOffset = 0f; // 必要に応じて調整
            sword.transform.rotation = Quaternion.Euler(0, 0, finalAngleZ + swordBaseOffset);
            // ↑ ここで Y軸回転は 0 に固定します。

            // ... (Z軸の並び順調整のコードはそのまま)
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
        // ★ エフェクトの回転もY軸回転は0に固定し、flipXで反転させるように変更
        float offsetAngle = 90.0f;
        sword_effect.transform.rotation = Quaternion.Euler(180, 180, heroController.angleZ + offsetAngle); // Y軸を0に

        // ★ エフェクトのSpriteRendererもflipXで反転させる
        SpriteRenderer effectSpriteRenderer = sword_effect.GetComponent<SpriteRenderer>();
        if (effectSpriteRenderer != null)
        {
            if (heroController.direction == 1) // 左向きの場合
            {
                effectSpriteRenderer.flipX = true;
            }
            else
            {
                effectSpriteRenderer.flipX = false;
            }
        }


        // プレイヤーの向き（angleZ）に合わせてエフェクトの相対位置を計算（swordOffset.x分離れた位置）
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;//斬撃の角度

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