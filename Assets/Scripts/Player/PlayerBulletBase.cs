using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void shoot(Collision collision)
    {
        // ºöÂÔÍæ¼ÒÅö×²
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enitity>() != null)
                collision.gameObject.GetComponent<Enitity>().TakeDamage(damage);
            if (collision.gameObject.GetComponent<EnemyController>() != null)
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        Destroy(gameObject); // Ïú»Ù×Óµ¯
    }
}
