using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="PlayerReflect", menuName ="ScriptableObject/Player/PlayerReflect", order = 0)]
public class PlayerReflect : PlayerState
{

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    //void HandleParryInput()
    //{
    //    if (Input.GetMouseButtonDown(1) && m_canParry)
    //    {
    //        StartCoroutine(ParryAction());
    //    }
    //}
    //System.Collections.IEnumerator ParryAction()
    //{
    //    // 进入弹反状态
    //    m_canParry = false;
    //    m_isParrying = true;
    //    gameObject.tag = "newTag";

    //    // 触发动画
    //    animator.SetTrigger("Parry");
    //    if (parryEffect != null)
    //        Instantiate(parryEffect, transform.position + Vector3.up, Quaternion.identity);

    //    // 弹反有效时间
    //    yield return new WaitForSeconds(parryDuration);

    //    // 结束弹反状态
    //    m_isParrying = false;
    //    gameObject.tag = "Player";

    //    // 冷却时间
    //    yield return new WaitForSeconds(parryCooldown - parryDuration);
    //    m_canParry = true;
    //}

    //void OnTriggerEnter(Collider other)
    //{

    //    // 检测子弹碰撞
    //    if (m_isParrying && other.gameObject.CompareTag("Bullet_Enemy"))
    //    {
    //        ReflectBullet(other.gameObject);

    //    }
    //}

    //void ReflectBullet(GameObject bullet)
    //{
    //    if (bullet == null) return;

    //    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    //    if (bulletRb != null)
    //    {
    //        // 反转子弹的朝向
    //        bullet.transform.forward = -bullet.transform.forward;
    //        // 反转刚体的速度方向
    //        bulletRb.velocity = -bulletRb.velocity;
    //    }

    //    // 修改子弹属性
    //    // bullet.speed *= 1.2f;  // 增加反弹速度
    //    // bullet.damage *= 2;    // 增加反弹伤害
    //    // bullet.ownerTag = "Enemy"; // 修改伤害目标

    //    // 添加视觉效果
    //    bullet.GetComponent<MeshRenderer>().material.color = Color.red;
    //}
}
