using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniTestScript : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BackAnimation()
    {
        bool nowBack = _animator.GetBool("back");
        _animator.SetBool("back", !nowBack);
        _animator.SetBool("right", false);
        _animator.SetBool("left", false);
    }

    public void RightAnimation()
    {
        bool nowRight = _animator.GetBool("right");
        _animator.SetBool("right", !nowRight);
        _animator.SetBool("back", false);
        _animator.SetBool("left", false);
    }

    public void LeftAnimation()
    {
        bool nowLeft = _animator.GetBool("left");
        _animator.SetBool("left", !nowLeft);
        _animator.SetBool("right", false);
        _animator.SetBool("back", false);
    }

}