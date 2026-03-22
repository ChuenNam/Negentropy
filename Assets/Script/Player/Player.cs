using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    common,
    fire,
    electricity
}



public class Player : MonoBehaviour
{
    [Header("资源")]
    public int MaxHP;
    public int MaxEP;
    [Space]
    public int HP;
    public int EP;
    public Element element;
    
    void Start()
    {
        HP = MaxHP;
        MaxHP = MaxEP;
    }

    public void SetElement(Element e)
    {
        element = e;
    }



}
