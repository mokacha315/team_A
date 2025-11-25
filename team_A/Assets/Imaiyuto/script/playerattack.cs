using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Diagnostics;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAttack : MonoBehaviour
{

    public int attackPower { get { return currentWeapon.attackPower; } } //攻撃力を currentWeapon から取る
    public WeaponData currentWeapon;//今持っている武器
    public float attackDuration = 0.2f;    //攻撃判定の持続時間
    public Vector2 swordOffset = new Vector2(1.0f, 0f); //基準のオフセット（右方向）
    public HeroController heroController;//HeroController参照取得

    private bool inAttack = false;
    private GameObject sword;
    private GameObject sword_effect;
    private UnityEngine.Transform swordTransform;// sword の Transform 
    private UnityEngine.Transform effectTransform; // sword_effect の Transform
    private SpriteRenderer swordSpriteRenderer; // ★ 追加: 剣のSpriteRendererをキャッシュ



    void Start()
    {
        // 1. HeroControllerへの参照取得 (これはどこにあっても問題なし)
        if (heroController == null)
        {
            heroController = GetComponent<HeroController>();
        }

        // 2. 武器データを基に装備処理を実行
        //    ==> EquipWeapon内で、現在の currentWeapon に基づいて剣(sword)とエフェクト(sword_effect)が生成されます。
        EquipWeapon(currentWeapon);

        // ===================================================================
        // 以降の処理は、EquipWeaponによって sword/sword_effect が生成された後でなければ実行できません
        // ===================================================================

        // nullチェック: プレハブが設定されていない場合（剣が表示されない最たる原因）に備えて確認
        if (sword == null || sword_effect == null)
        {
            Debug.LogError("Weapon initialization failed! Check if swordPrefab and swordEffectPrefab are set in the WeaponData asset.");
            return; // エラーがあれば、これ以降の初期化を中断
        }

        // 3. 生成されたオブジェクトの Transform を取得してキャッシュ
        swordTransform = sword.transform;
        effectTransform = sword_effect.transform;

        // 4. 生成された剣の SpriteRenderer を取得してキャッシュ
        //    (EquipWeapon内で古い剣が破棄され、新しい剣が作られているため、ここで再取得が必要)
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();
        if (swordSpriteRenderer == null)
        {
            Debug.LogError("Sword prefab must have a SpriteRenderer component.");
        }

        // 5. 剣の表示設定 (EquipWeapon内で既に設定されていますが、念のため)
        sword.SetActive(true);
        sword_effect.SetActive(false);

        // 6. 剣の初期位置をプレイヤーのそばに設定
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
    public void ChangeSword(GameObject newSwordPrefab)//剣の種類入れ替え
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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            Attack();
        }

        if (heroController != null)
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
        //剣のモデルは常に表示
        //攻撃判定を持つエフェクトを表示
        sword_effect.SetActive(true);

        //エフェクトの回転をプレイヤーの向きに合わせて設定
        //エフェクトの回転もY軸回転は0に固定し、flipXで反転させるように変更
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

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        if (sword != null) Destroy(sword);
        if (sword_effect != null) Destroy(sword_effect);

        sword = Instantiate(newWeapon.swordPrefab, transform);
        sword_effect = Instantiate(newWeapon.swordEffectPrefab, transform);

        sword.SetActive(true);
        sword_effect.SetActive(false);

        Debug.Log("武器装備: " + newWeapon.weaponName + " / 攻撃力: " + newWeapon.attackPower);
    }
}