using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    none,
    spike,
    ball,
}

public class Attacker : MonoBehaviour, IAttack
{
    [SerializeField] private AttackType attackType;
    public AttackType AttackType
    {
        get => attackType;
        set => attackType = value;
    }

    [SerializeField] private int damage;
    public int Damage {
        get => damage; 
        set => damage = value; 
    }
    [SerializeField] private Element elementType;
    public Element ElementType { 
        get => elementType;
        set
        {
            elementType = value; 
            elementType.SetElementColor(GetComponent<MeshRenderer>().material);
        }
    }
    public int reactionStage = 3;
    
    [SerializeField] private float reactionRange;
    public float ReactionRange => reactionRange;

    public void ReactionAttack(IElement t)
    {
        if (AttackType == AttackType.spike)
        {
            damage *= 2;
        }

        if (AttackType == AttackType.ball)
        {
            if (t is ICanBeAttack target)
            {
                target.OnHitCallback += OnBoomDo;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var list = other.GetComponents<ICanBeAttack>();
        foreach (var target in list)
        {
            var obj = target as BaseObject;
            if (obj.enabled && obj.canInteract)
            {
                if (obj is IElement elementObject)
                {
                    elementObject.Reaction(this, reactionStage);
                }
                target.OnAttackedInvoke(this);
                break;
            }
        }
    }

    private GameObject boomWavePrefab;
    private void OnBoomDo()
    {
        var boomWave = Instantiate(boomWavePrefab, transform.position, transform.rotation);
        boomWave.GetComponent<Attacker>().Damage = this.Damage;
        StartCoroutine(BoomSpread(boomWave.transform));
    }

    private IEnumerator BoomSpread(Transform waveTransform)
    {
        yield return waveTransform.TransformShape(Vector3.one * ReactionRange, 10f);
        Debug.Log("Boom Over");
        Destroy(waveTransform.gameObject);
        yield return null;
    }

    private void Awake()
    {
        boomWavePrefab = Resources.Load<GameObject>($"Prefab/Item/BoomWave");
    }
}
