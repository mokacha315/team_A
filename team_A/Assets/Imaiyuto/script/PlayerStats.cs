using UnityEngine;

public class playerStats : MonoBehaviour
{
    //��b�U����
    public float baseAttackPower = 1f;
    //�o�t�ɂ��ǉ��U����
    private float attackbuff = 0f;
  
    public float CurrentAttackPower
    {
        get { return baseAttackPower; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
