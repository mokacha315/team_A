
using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackPower { get { return currentWeapon.attackPower; } }
    public WeaponData currentWeapon;
    public float attackDuration { get { return currentWeapon.attackDuration; } }
    public float attackCooldown { get { return currentWeapon.attackCooldown; } }
    public int extraDamage = 0;

    public void AddDamage(int value) { extraDamage += value; }

    public Vector2 swordOffset = new Vector2(1.5f, 0f); //1.0から1.5に増やすとより前方に表示されます
    public HeroController heroController;

    public AudioSource audioSource;
    public AudioClip attackSE;

    private bool inAttack = false;
    private float nextAttackTiam = 0f;
    private GameObject sword;
    private GameObject sword_effect;
    private Transform swordTransform;
    private Transform effectTransform;
    private SpriteRenderer swordSpriteRenderer;
    /// <summary>
    /// 主人公の情報と武器の情報取得
    /// </summary>
    void Start()
    {
        if (heroController == null) heroController = GetComponent<HeroController>();
        EquipWeapon(currentWeapon);
    }
    /// <summary>
    /// もっている剣の向きや生成される場所の調整
    /// 武器を拾った時の処理
    /// </summary>
    void Update()
    {
        //攻撃の入力判定
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            if (Time.time >= nextAttackTiam)
            {
                Attack();
                nextAttackTiam = Time.time + attackCooldown;
            }
        }

        //剣の見た目更新
        if (heroController != null && swordTransform != null && swordSpriteRenderer != null)
        {
            UpdateSwordVisuals();
        }
    }

    //剣の向きと位置を更新する処理
    private void UpdateSwordVisuals()
    {
        float angle = heroController.angleZ;
        float normAngle = (angle % 360 + 360) % 360;
        float displayAngle = 0f;

        bool isDiagonalUp = Mathf.Abs(Mathf.Abs(angle % 90) - 45f) < 10f && (normAngle > 0 && normAngle < 180);
        bool isDiagonalLeftDown = Mathf.Abs(normAngle - 225f) < 20f;
        bool isDiagonalRightDown = Mathf.Abs(normAngle - 315f) < 20f;

        if (isDiagonalUp)
        {
            swordSpriteRenderer.flipX = false;
            swordSpriteRenderer.flipY = false;
            displayAngle = angle - 45f;
        }
        else if (isDiagonalLeftDown)
        {
            swordSpriteRenderer.flipX = false;
            swordSpriteRenderer.flipY = true;
            displayAngle = angle - 45f;
        }
        else if (isDiagonalRightDown)
        {
            swordSpriteRenderer.flipX = false;
            swordSpriteRenderer.flipY = false;
            displayAngle = angle + 45f;
        }
        else
        {
            swordSpriteRenderer.flipY = false;
            swordSpriteRenderer.flipX = (heroController.direction == 1 || heroController.direction == 2);
            displayAngle = 0f;
        }

        swordTransform.rotation = Quaternion.Euler(0, 0, displayAngle);

        int baseOrder = 100;
        swordSpriteRenderer.sortingOrder = (normAngle > 10 && normAngle < 170) ? baseOrder - 1 : baseOrder + 1;

        float angleRad = angle * Mathf.Deg2Rad;
        swordTransform.localPosition = new Vector3(
            Mathf.Cos(angleRad) * (swordOffset.x * 0.5f),
            Mathf.Sin(angleRad) * (swordOffset.x * 0.5f),
            -0.01f //Zを少しマイナスにしてキャラより手前に
        );
    }
    /// <summary>
    /// 攻撃の処理、攻撃判定を持っているエフェクトの向きやエフェクトの大きさ調整
    /// </summary>
    void Attack()
    {
        if (heroController == null || sword_effect == null || effectTransform == null) return;
        if (audioSource != null && attackSE != null) audioSource.PlayOneShot(attackSE);

        inAttack = true;
        sword_effect.SetActive(true);

        //エフェクトの回転
        float offsetAngle = 90f;
        effectTransform.rotation = Quaternion.Euler(180, 180, heroController.angleZ + offsetAngle);

        //エフェクトの位置（ワールド座標で計算）
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;

        //主人公の現在地（中心点）を取得
        Vector3 playerPos = transform.position;

        //向いている方向にオフセットを計算
        Vector3 worldOffset = new Vector3(
            Mathf.Cos(angleRad) * swordOffset.x,
            Mathf.Sin(angleRad) * swordOffset.x,
            -0.2f //Z軸でキャラより手前に
        );

        //ワールド座標として適用（これで親の反転の影響を受けなくなります）
        effectTransform.position = playerPos + worldOffset;

        Invoke(nameof(StopAttack), attackDuration);
    }
    /// <summary>
    /// 攻撃をした後攻撃判定を持つエフェクトを消す処理
    /// </summary>
    void StopAttack()
    {
        if (sword_effect != null) sword_effect.SetActive(false);
        inAttack = false;
    }
    /// <summary>
    /// 武器データの管理
    /// </summary>
    /// <param name="newWeapon"></param>
    public void EquipWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null) return;
        currentWeapon = newWeapon;

        if (sword != null) Destroy(sword);
        if (sword_effect != null) Destroy(sword_effect);

        sword = Instantiate(newWeapon.swordPrefab, transform);
        sword_effect = Instantiate(newWeapon.swordEffectPrefab, transform);

        sword.SetActive(true);
        sword_effect.SetActive(false);

        swordTransform = sword.transform;
        effectTransform = sword_effect.transform;
        swordSpriteRenderer = sword.GetComponent<SpriteRenderer>();

        SetInitialSwordPosition();
    }
    /// <summary>
    /// 不明 家でやりましょう
    /// </summary>
    void SetInitialSwordPosition()
    {
        if (swordTransform == null || heroController == null) return;
        float angleRad = heroController.angleZ * Mathf.Deg2Rad;
        swordTransform.localPosition = new Vector3(
            Mathf.Cos(angleRad) * (swordOffset.x * 0.5f),
            Mathf.Sin(angleRad) * (swordOffset.x * 0.5f),
            -0.01f
        );
    }
}