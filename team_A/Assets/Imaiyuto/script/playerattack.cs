using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAttack : MonoBehaviour
{
    // 公開フィールド
    public int attackPower { get { return currentWeapon.attackPower; } } //攻撃力を currentWeapon から取る
    public WeaponData currentWeapon;//今持っている武器
    public float attackDuration = 0.2f;    //攻撃判定の持続時間
    public Vector2 swordOffset = new Vector2(1.0f, 0f); //基準のオフセット（右方向）
    public HeroController heroController;//HeroController参照取得

    // プライベートフィールド（生成・キャッシュされたオブジェクトを保持）
    private bool inAttack = false;
    private GameObject sword;
    private GameObject sword_effect;
    private UnityEngine.Transform swordTransform;
    private UnityEngine.Transform effectTransform;
    private SpriteRenderer swordSpriteRenderer;


    void Start()
    {
        // 1. HeroControllerへの参照取得
        if (heroController == null)
        {
            heroController = GetComponent<HeroController>();
        }

        // 2. 武器データを基に装備処理を実行
        //    EquipWeapon内で、剣とエフェクトの生成と、Transform/SpriteRendererのキャッシュを全て完了させます。
        EquipWeapon(currentWeapon);

        // Transformが正しくキャッシュされているかチェック
        if (swordTransform == null)
        {
            Debug.LogError("Weapon initialization failed! swordPrefab is likely missing in WeaponData asset.");
            return;
        }

        // 剣の初期位置をプレイヤーのそばに設定（オフセットを適用して自然に持つ）
        if (heroController != null)
        {
            float angleRad = (heroController.angleZ != 0 ? heroController.angleZ : 0) * Mathf.Deg2Rad;
            Vector3 initialOffset = new Vector3(
                Mathf.Cos(angleRad) * (swordOffset.x * 0.5f), //攻撃時より少し近く
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

    // --- Update ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            Attack();
        }

        // swordTransformがnullでないかチェックを追加
        if (heroController != null && swordTransform != null)
        {
            //剣本体の向き
            float finalAngleZ = heroController.angleZ;

            if (swordSpriteRenderer != null)
            {
                if (heroController.direction == 0) // direction == 0は下
                {
                    swordSpriteRenderer.flipX = true;
                    swordSpriteRenderer.flipY = false;
                }
                else if (heroController.direction == 1)//左向き
                {
                    swordSpriteRenderer.flipX = false;
                    swordSpriteRenderer.flipY = true;
                }
                else if (heroController.direction == 2)//上向き
                {
                    swordSpriteRenderer.flipY = false;
                    swordSpriteRenderer.flipX = false;
                }
                else//右
                {
                    swordSpriteRenderer.flipX = false;
                    swordSpriteRenderer.flipY = false;
                }
            }

            // 剣の回転はZ軸のみで制御し、Y軸は常に0に
            float swordBaseOffset = 0f;
            swordTransform.rotation = Quaternion.Euler(0, 0, finalAngleZ + swordBaseOffset);

            // Z軸の並び順調整
            float swordz = -1; // 手前
            if (heroController.angleZ > 45 && heroController.angleZ < 150)
            {
                swordz = 100; // 上向きの時は奥に回す
            }
            else
            {
                swordz = -1;
            }
            // 剣の位置のZ軸だけを調整
            Vector3 currentPos = swordTransform.position;
            swordTransform.position = new Vector3(currentPos.x, currentPos.y, swordz);

            // 常に表示される剣の位置を毎フレーム更新
            float angleRad = heroController.angleZ * Mathf.Deg2Rad;
            Vector3 initialOffset = new Vector3(
                Mathf.Cos(angleRad) * (swordOffset.x * 0.5f), // 攻撃時より少し近く
                Mathf.Sin(angleRad) * (swordOffset.x * 0.5f),
                0
            );
            swordTransform.localPosition = initialOffset;
        }
    }

    // --- Attack ---
    void Attack()
    {
        // nullチェック
        if (heroController == null || sword_effect == null || effectTransform == null) return;

        inAttack = true;
        sword_effect.SetActive(true);

        // エフェクトの回転を設定
        float offsetAngle = 90.0f;
        effectTransform.rotation = Quaternion.Euler(180, 180, heroController.angleZ + offsetAngle); // Y軸を0に

        // エフェクトのSpriteRendererもflipXで反転させる
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

        // エフェクトの相対位置を計算
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;
        Vector3 rotatedOffset = new Vector3(
            Mathf.Cos(angleRad) * swordOffset.x,
            Mathf.Sin(angleRad) * swordOffset.x,
            0
        );

        // エフェクトのローカル座標を設定
        effectTransform.localPosition = rotatedOffset;

        // エフェクトのZ座標は剣と同じか、少し手前/奥
        Vector3 currentSwordPos = swordTransform.position;
        effectTransform.position = new Vector3(
            currentSwordPos.x + rotatedOffset.x,
            currentSwordPos.y + rotatedOffset.y,
            currentSwordPos.z - 0.01f // 剣より少し手前に出す
        );


        Debug.Log($"攻撃開始！ エフェクトの位置（ローカル）: {effectTransform.localPosition} 角度: {heroController.angleZ}");

        Invoke(nameof(StopAttack), attackDuration);
    }

    // --- StopAttack ---
    void StopAttack()
    {
        // 攻撃判定を持つエフェクトを非表示にする
        if (sword_effect != null)
        {
            sword_effect.SetActive(false);
        }
        inAttack = false;
        Debug.Log("攻撃終了！");
    }

    // --- EquipWeapon (武器入れ替えとキャッシュ処理) ---
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        // 古い剣とエフェクトを破棄
        if (sword != null) Destroy(sword);
        if (sword_effect != null) Destroy(sword_effect);

        // nullチェック: プレハブが設定されていない場合は処理を中断し、変数をリセット
        if (newWeapon == null || newWeapon.swordPrefab == null || newWeapon.swordEffectPrefab == null)
        {
            Debug.LogError($"WeaponData '{newWeapon?.weaponName}' is missing required prefabs. Cannot equip.");
            sword = null;
            sword_effect = null;
            swordTransform = null;
            effectTransform = null;
            swordSpriteRenderer = null;
            return;
        }

        // 新しい剣とエフェクトを生成
        sword = Instantiate(newWeapon.swordPrefab, transform);
        sword_effect = Instantiate(newWeapon.swordEffectPrefab, transform);

        // TransformとSpriteRendererをキャッシュ
        if (sword != null)
        {
            swordTransform = sword.transform;
            swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        }
        if (sword_effect != null)
        {
            effectTransform = sword_effect.transform;
        }

        // 剣のSpriteRendererがない場合のログ
        if (swordSpriteRenderer == null)
        {
            Debug.LogWarning($"Sword prefab for '{newWeapon.weaponName}' is missing a SpriteRenderer component.");
        }

        // 表示設定
        sword.SetActive(true);
        sword_effect.SetActive(false);

        Debug.Log("武器装備: " + newWeapon.weaponName + " / 攻撃力: " + newWeapon.attackPower);
    }

    // --- ChangeSword ---
    public void ChangeSword(GameObject newSwordPrefab) //剣の種類入れ替え
    {
        // 現在の剣を削除
        if (sword != null)
        {
            Destroy(sword);
        }

        // 新しい剣を生成
        sword = Instantiate(newSwordPrefab, transform);
        swordTransform = sword.transform;

        // SpriteRenderer 再取得
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();

        // 常に表示
        sword.SetActive(true);

        Debug.Log("剣を差し替えました！");
    }
}