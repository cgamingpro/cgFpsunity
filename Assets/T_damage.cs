using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class T_damage : MonoBehaviour
{
    public float health = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoDamage(float damage )
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }

    }
    void Die()
    {
        Destroy(gameObject);
    }
}
