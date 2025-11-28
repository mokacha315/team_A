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
    public float attackDuration { get {return currentWeapon.attackDuration; } } //攻撃判定の持続時間
    public float attackCooldown { get { return currentWeapon.attackCooldown; } }// 攻撃のクールタイム（秒）
    public Vector2 swordOffset = new Vector2(1.0f, 0f); //基準のオフセット（右方向）
    public HeroController heroController;//HeroController参照取得

    // プライベートフィールド（生成・キャッシュされたオブジェクトを保持）
    private bool inAttack = false;
    private float nextAttackTiam=0f;//クールタイムを数える用の変数
    private GameObject sword;//剣
    private GameObject sword_effect;//エフェクト
    private UnityEngine.Transform swordTransform;
    private UnityEngine.Transform effectTransform;
    private SpriteRenderer swordSpriteRenderer;


    void Start()
    {
        //HeroControllerへの参照取得
        if (heroController == null)
        {
            heroController = GetComponent<HeroController>();
        }

        //武器データを基に装備処理を実行
        EquipWeapon(currentWeapon);

        //Transformが正しくキャッシュされているかチェック
        if (swordTransform == null)
        {
            Debug.LogError("Weapon initialization failed! swordPrefab is likely missing in WeaponData asset.");
            return;
        }

        // 剣の初期位置設定ロジックを SetInitialSwordPosition に移動
        SetInitialSwordPosition();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            if (Time.time >= nextAttackTiam)
            {
                Attack();

                //攻撃した後次攻撃できるまでの時間を更新
                nextAttackTiam = Time.time+attackCooldown;
            }
        }

        //swordTransformがnullでないかチェックを追加
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
            float swordz = -10; // 手前
            if (heroController.angleZ > 45 && heroController.angleZ < 150)
            {
                swordz = 10; // 上向きの時は奥に回す
            }
            int sortingOrder=10;
            swordSpriteRenderer.sortingOrder = sortingOrder;
            // 剣の位置のZ軸だけを調整
            // Vector3 currentPos = swordTransform.position;
            //swordTransform.position = new Vector3(currentPos.x, currentPos.y, swordz);

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
    void Attack()
    {
        //nullチェック
        if (heroController == null || sword_effect == null || effectTransform == null) return;

        inAttack = true;
        sword_effect.SetActive(true);

        //エフェクトの回転を設定
        float offsetAngle = 90.0f;
        effectTransform.rotation = Quaternion.Euler(180, 180, heroController.angleZ + offsetAngle); // Y軸を0に

        //エフェクトのSpriteRendererもflipXで反転させる
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

        //エフェクトの相対位置を計算
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;
        Vector3 rotatedOffset = new Vector3(
            Mathf.Cos(angleRad) * swordOffset.x,
            Mathf.Sin(angleRad) * swordOffset.x,
            0
        );

        //エフェクトのローカル座標を設定
        effectTransform.localPosition = rotatedOffset;

        //エフェクトのZ座標は剣と同じか、少し手前/奥
        Vector3 currentSwordPos = swordTransform.position;
        effectTransform.position = new Vector3(
            currentSwordPos.x + rotatedOffset.x,
            currentSwordPos.y + rotatedOffset.y,
            currentSwordPos.z - 0.01f // 剣より少し手前に出す
        );


        Debug.Log($"攻撃開始！ エフェクトの位置（ローカル）: {effectTransform.localPosition} 角度: {heroController.angleZ}");

        Invoke(nameof(StopAttack), attackDuration);
    }
    void StopAttack()
    {
        //攻撃判定を持つエフェクトを非表示にする
        if (sword_effect != null)
        {
            sword_effect.SetActive(false);
        }
        inAttack = false;
        Debug.Log("攻撃終了！");
    }

    //武器入れ替えと持っていた武器の処理
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        //剣を拾う前に持っていた剣を削除
        if (sword != null) Destroy(sword);
        if (sword_effect != null) Destroy(sword_effect);

        //新しい剣を装備
        sword = Instantiate(newWeapon.swordPrefab, transform);
        sword_effect = Instantiate(newWeapon.swordEffectPrefab, transform);

        sword.SetActive(true);
        sword_effect.SetActive(false);

        swordTransform = sword.transform;
        effectTransform = sword_effect.transform;
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();

        // ★★★ 武器を生成し直した後、必ず初期位置を設定する！ ★★★
        SetInitialSwordPosition();

        Debug.Log("武器装備: " + newWeapon.weaponName + " / 攻撃力: " + newWeapon.attackPower);

    }
    void SetInitialSwordPosition()
    {
        // Transformが正しくキャッシュされているかチェック
        if (swordTransform == null)
        {
            Debug.LogError("Sword Transform is null. Cannot set initial position.");
            return;
        }

        //剣の初期位置をプレイヤーのそばに設定（オフセットを適用して自然に持つ）
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

}