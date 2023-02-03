using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool IsLiquid => isLiquid;

    [SerializeField] private bool isLiquid = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public Klonk.EntityDef GetDef()
    {
        throw new NotImplementedException();
    }
}