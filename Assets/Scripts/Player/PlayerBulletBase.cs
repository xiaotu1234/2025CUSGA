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
    protected virtual void shoot(Collider other)
    {
        // ºöÂÔÍæ¼ÒÅö×²
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<Enitity>() != null)
                other.gameObject.GetComponent<Enitity>().TakeDamage(damage);
            if (other.gameObject.GetComponent<EnemyController>() != null)
                other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        Destroy(gameObject); // Ïú»Ù×Óµ¯
    }
}
