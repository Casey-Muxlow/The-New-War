using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnim : MonoBehaviour
{
    public Animator animator;
    public bool _localAuto;
    private TDActiveElement activeParent;
    private bool _isIn;

    public void Start()
    {
        // find reference for vars
        activeParent = this.GetComponent<TDActiveElement>();

        // check if global auto enabled
        if (activeParent.Automatic)
            _localAuto = true;
    }
    public void Update()
    {
        if (activeParent.ActiveCollider.actived)
        {
            if (_localAuto)
            {
                if (activeParent.AutoOnExit)
                    {
                    //Debug.Log("Activando animacion");
                    animator.SetBool("Opened", false);
                    animator.SetTrigger("Actived");
                    }
                else
                {
                    if (!_isIn)
                    {
                        if (animator.GetBool("Opened"))
                        {
                            //Debug.Log("Activando animacion");
                            animator.SetBool("Opened", false);
                            animator.SetTrigger("Actived");
                        }
                        else
                        {
                            //Debug.Log("Activando animacion de salida");
                            animator.SetTrigger("Actived");
                            animator.SetBool("Opened", true);
                        }
                    }
                }

            }
            else if (Input.GetKeyDown(activeParent.ActiveElementKey))
            {
                if (animator.GetBool("Opened"))
                {
                    //Debug.Log("Activando animacion");
                    animator.SetBool("Opened", false);
                    animator.SetTrigger("Actived");
                }
                else
                {
                    //Debug.Log("Activando animacion de salida");
                    animator.SetTrigger("Actived");
                    animator.SetBool("Opened", true);
                }
            }
            _isIn = true;
        }
        else if (activeParent.ActiveCollider.hasexit)
        {
            _isIn = false;

            if(_localAuto && activeParent.AutoOnExit)
            {
                //Debug.Log("Activando animacion de salida");
                animator.SetTrigger("Actived");
                animator.SetBool("Opened", true);
            }
        }
    }
}
