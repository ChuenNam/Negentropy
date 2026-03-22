using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavier : MonoBehaviour
{
    public Transform follower;

    public Transform startPos;
    public Transform atkTarget;
    
    public float distance = 5f; // 离Y轴的距离参数
    public float rotationSpeed = 50f; // 旋转速度
    public float attackMoveSpeed = 10f; // 攻击移动速度
    
    public MeshFilter meshFilter;
    public List<Mesh> meshes;
    
    private bool isAttacking = false; // 是否正在攻击
    private bool isReturning = false; // 是否正在返回

    public void OnAtkE(InputValue value)
    {
        var baseEnemy = GetComponent<Attack>().target;
        if (baseEnemy) 
            atkTarget = baseEnemy.transform;
        
        if (isAttacking || isReturning) return;

        if (Player.Instance.EP < 2)
        {
            Debug.Log("能量不足");
            return;
        }
        Player.Instance.MinusEP(2);
        meshFilter.mesh = meshes[1];
        GetComponent<Animator>().SetBool("isAttack", true);
        transform.LookAt(new Vector3(atkTarget.transform.position.x, 0, atkTarget.transform.position.z));
        StartCoroutine(AttackCoroutine());
    }

    public void OnAtkQ(InputValue value)
    {
        var baseEnemy = GetComponent<Attack>().target;
        if (baseEnemy)
            atkTarget = baseEnemy.transform;
        
        if (isAttacking || isReturning) return;

        if (Player.Instance.EP < 2)
        {
            Debug.Log("能量不足");
            return;
        }
        Player.Instance.MinusEP(2);
        meshFilter.mesh = meshes[2];
        GetComponent<Animator>().SetBool("isAttack", true);
        transform.LookAt(new Vector3(atkTarget.transform.position.x, 0, atkTarget.transform.position.z));
        StartCoroutine(AttackCoroutine());
    }
    
    private IEnumerator AttackCoroutine()
    {
        if (atkTarget is null) yield break;
        
        isAttacking = true;
        GetComponent<Move>().canMove = false;
        // 攻击开始前，平滑移动到起始位置
        while (Vector3.Distance(follower.position, startPos.position) > 0.1f)
        {
            Vector3 direction = (startPos.position - follower.position).normalized;
            follower.position += direction * attackMoveSpeed * Time.deltaTime;
            yield return null;
        }
        
        // 移动到攻击目标
        while (atkTarget != null && Vector3.Distance(follower.position, atkTarget.position) > 0.1f)
        {
            Vector3 direction = (atkTarget.position - follower.position).normalized;
            follower.position += direction * attackMoveSpeed * Time.deltaTime;
            follower.LookAt(atkTarget);
            yield return null;
        }
        
        isAttacking = false;
        isReturning = true;
        
        // 返回到起始位置
        while (Vector3.Distance(follower.position, startPos.position) > 0.1f)
        {
            Vector3 direction = (startPos.position - follower.position).normalized;
            follower.position += direction * attackMoveSpeed * Time.deltaTime;
            follower.LookAt(startPos);
            yield return null;
        }
        
        isReturning = false;
        meshFilter.mesh = meshes[0];
        GetComponent<Move>().canMove = true;
        GetComponent<Animator>().SetBool("isAttack", false);
    }
    
    
    void Update()
    {
        // 如果正在攻击或返回，不执行旋转
        if (isAttacking || isReturning) return;
        
        // 计算旋转角度
        float angle = Time.time * rotationSpeed;
        
        // 计算新位置
        float x = transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad) * distance;
        float z = transform.position.z + Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float y = follower.position.y;
        
        // 设置位置
        follower.position = new Vector3(x, y, z);
        
        // 看向player
        //follower.LookAt(transform);
    }
}
