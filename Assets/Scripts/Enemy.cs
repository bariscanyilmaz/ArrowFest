using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Animator _animator;
    public void Die() => _animator.SetBool("IsDead", true);
    

}
