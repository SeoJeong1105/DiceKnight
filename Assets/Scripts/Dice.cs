using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public Action<int> OnDiceStopped;
    public Action<bool, int> OnDiceSelected;

    [SerializeField] Transform[] _diceSides;
    [SerializeField] float _force = 5f;
    [SerializeField] float _torque = 5f;

    public Material material;
    Material _material;
    
    Rigidbody _rigidbody;

    bool isRolling = false;
    bool isSelected = false;
    bool isSelectable = false;

    int result;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<MeshRenderer>().material;
    }

    void FixedUpdate() 
    {
        if(GetComponent<Rigidbody>().IsSleeping() && isRolling)
        {
            result = GetSideFacingUp();
            OnDiceStopped.Invoke(result);
        }
    }

    public void RollDice()
    {
        Vector3 force = new Vector3(0f, _force, 0f);
        Vector3 torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * _torque;

        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(torque, ForceMode.Impulse);

        isRolling = true;
    }

    int GetSideFacingUp()
    {
        Transform upSide = null;
        float maxDot = -1;

        foreach(Transform side in _diceSides)
        {
            float dot = Vector3.Dot(side.up, Vector3.up);
            
            if (dot <= maxDot) continue;
            
            maxDot = dot;
            upSide = side;
        }

        isRolling = false;

        if(upSide != null)
            return int.Parse(upSide.name);
        return 0;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetDice()
    {
        isSelected = false;
        isRolling = false;
        this.gameObject.GetComponent<MeshRenderer>().material = _material;
        result = 0;

        return;
    }

    public void OnMouseDown()
    {
        if(isRolling || !isSelectable) return;
        
        isSelected = !isSelected;

        if (isSelected)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = material;
            OnDiceSelected.Invoke(true, result);
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material = _material;
            OnDiceSelected.Invoke(false, result);
        }

        return;
    }

    public void SelectDice()
    {
        isSelected = true;
        this.gameObject.GetComponent<MeshRenderer>().material = material;
    }

    public void SetSelectable(bool b)
    {
        isSelectable = b;
        return;
    }

    public int GetResult()
    {
        return result;
    }
}
