using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    public Rigidbody Rb => _rb;

    public void Init()
    {

    }
}
