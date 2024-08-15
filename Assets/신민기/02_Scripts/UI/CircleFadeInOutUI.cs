using System.Collections;
using UnityEngine;

public class CircleFadeInOutUI : MonoBehaviour
{
    Animator  animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }

}
