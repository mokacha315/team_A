using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;     //íœ‚·‚éŠÔw’è

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);   //íœİ’è
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);     //‰½‚©‚ÉÚG‚µ‚½‚çÁ‚·
    }
}
