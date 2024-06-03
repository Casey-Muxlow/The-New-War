using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTransform : MonoBehaviour
{
    [System.Serializable]
    public class Target
    {
        public GameObject TargetGO;
        public Vector3 TranslateGO = new Vector3(0,0,0);
        public Quaternion RotateGO = new Quaternion(0, 0, 0, 100f);
        public bool Scalable = false;
        public Vector3 ScaleGO = new Vector3(1, 1, 1);
        [HideInInspector]
        public Vector3 originalTf;
        [HideInInspector]
        public Vector3 differenceSc;
    }
    public List<Target> Targets;
    public float Duration;
    public bool done;
    public bool _localAuto = false;
    private TDActiveElement activeParent;
    private bool _ismoving = false;
    private float _timer = 0f;
    private float _percentage = 0;
    private bool _waitforit = false;
    private bool _isIn;
    private bool _isOut;

    void Start()
    {
        // find reference for vars
        activeParent = this.GetComponent<TDActiveElement>();

        //check if movement is done at start
            foreach (Target tar in Targets)
        {
            // store original position
            tar.originalTf = tar.TargetGO.transform.localPosition;
            // save start scale
            if (tar.Scalable)
            {
                tar.differenceSc = (tar.ScaleGO - tar.TargetGO.transform.localScale);
                tar.ScaleGO = tar.TargetGO.transform.localScale;
            }
            if (done)
            {
                // Move position
                tar.TargetGO.transform.localPosition = (tar.originalTf + tar.TranslateGO);
                // Rotate
                tar.TargetGO.transform.localRotation = new Quaternion(tar.RotateGO.x, tar.RotateGO.y, tar.RotateGO.z, tar.RotateGO.w);
                // Scale
                if (tar.Scalable)
                    tar.TargetGO.transform.localScale = tar.ScaleGO + tar.differenceSc;
            }
        }

        // check if global auto enabled
        if (activeParent.Automatic)
            _localAuto = true;
    }
    void Update()
    {
        // check if ready for action
        if (activeParent.ActiveCollider.actived)
        {
            // action when auto mode
            if (_localAuto)
            {
                // check if just entered
                if (!_isIn)
                {
                    _waitforit = false;
                    _ismoving = true;
                }
                _isIn = true;
            }
            // action when keycode pressed
            else if (Input.GetKeyDown(activeParent.ActiveElementKey))
            {
                _ismoving = true;
                _waitforit = false;
                if (activeParent.AutoOnExit && !done)
                _isIn = true;
            }
        }
        // exit action when automatic onexit enable
        else if (activeParent.ActiveCollider.hasexit == true)
        {
            if (activeParent.AutoOnExit && _isIn)
            {
                _isOut = true;
            }
            _isIn = false;
        }
        // active close if exit collider before anim ends
        if (activeParent.AutoOnExit && _isOut && !_ismoving)
        {
            _waitforit = false;
            _ismoving = true;
            _isOut = false;
        }

        // Action
        if (_ismoving & !_waitforit)
        {
            // Get time updated and fixed
            _timer = _timer + Time.deltaTime;
            //set percentage of movement done
            if (!done)
                _percentage = (_timer / Duration);
            else
                _percentage = (1 - (_timer / Duration));

            //stop movement when time is over.
            if (_percentage > 1f)
            {
                _ismoving = false;
                _percentage = 1;
                _timer = 0f;
                done = true;
                _waitforit = true;
                Debug.Log("Door Opened");
            }
            else
                if (_percentage < 0f)
            {
                _ismoving = false;
                _percentage = 0;
                _timer = 0f;
                done = false;
                _waitforit = true;
                Debug.Log("Door Closed");
            }
            foreach (Target tar in Targets)
            {
                // Move position
                tar.TargetGO.transform.localPosition = tar.originalTf + (_percentage * tar.TranslateGO);
                // Rotate
                tar.TargetGO.transform.localRotation = new Quaternion(tar.RotateGO.x * _percentage, tar.RotateGO.y * _percentage, tar.RotateGO.z * _percentage, tar.RotateGO.w);
                // Scale
                if (tar.Scalable)
                tar.TargetGO.transform.localScale = tar.ScaleGO + (_percentage * tar.differenceSc);
            }
        }
    }
}
