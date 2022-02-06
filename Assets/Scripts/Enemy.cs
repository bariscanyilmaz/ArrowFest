using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Animator _animator;
    [SerializeField] int _damage;
    public void Die() => _animator.SetBool("IsDead", true);
    
    public int TakeDamage(int arrowCount) =>arrowCount-_damage;
}
