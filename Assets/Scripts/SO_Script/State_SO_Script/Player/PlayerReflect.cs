using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="PlayerReflect", menuName ="ScriptableObject/Player/PlayerReflect", order = 0)]
public class PlayerReflect : PlayerState
{
    public override void OnAwake()
    {
        base.OnAwake();
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
    //    // ���뵯��״̬
    //    m_canParry = false;
    //    m_isParrying = true;
    //    gameObject.tag = "newTag";

    //    // ��������
    //    animator.SetTrigger("Parry");
    //    if (parryEffect != null)
    //        Instantiate(parryEffect, transform.position + Vector3.up, Quaternion.identity);

    //    // ������Чʱ��
    //    yield return new WaitForSeconds(parryDuration);

    //    // ��������״̬
    //    m_isParrying = false;
    //    gameObject.tag = "Player";

    //    // ��ȴʱ��
    //    yield return new WaitForSeconds(parryCooldown - parryDuration);
    //    m_canParry = true;
    //}

    //void OnTriggerEnter(Collider other)
    //{

    //    // ����ӵ���ײ
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
    //        // ��ת�ӵ��ĳ���
    //        bullet.transform.forward = -bullet.transform.forward;
    //        // ��ת������ٶȷ���
    //        bulletRb.velocity = -bulletRb.velocity;
    //    }

    //    // �޸��ӵ�����
    //    // bullet.speed *= 1.2f;  // ���ӷ����ٶ�
    //    // bullet.damage *= 2;    // ���ӷ����˺�
    //    // bullet.ownerTag = "Enemy"; // �޸��˺�Ŀ��

    //    // �����Ӿ�Ч��
    //    bullet.GetComponent<MeshRenderer>().material.color = Color.red;
    //}
}
