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
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool nowBack = _animator.GetBool("back");
            _animator.SetBool("back", !nowBack);
            _animator.SetBool("right", false);
            _animator.SetBool("left", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bool nowRight = _animator.GetBool("right");
            _animator.SetBool("right", !nowRight);
            _animator.SetBool("back", false);
            _animator.SetBool("left", false);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            bool nowLeft = _animator.GetBool("left");
            _animator.SetBool("left", !nowLeft);
            _animator.SetBool("right", false);
            _animator.SetBool("back", false);
        }
    }
}
