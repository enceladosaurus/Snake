using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class SnakeAnimController : MonoBehaviour
{
    private static readonly int Swallow = Animator.StringToHash("Swallow");
    private Animator animator = null;
   
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlaySwallowAnimation()
    {
        animator.SetTrigger(Swallow);
    }
}
