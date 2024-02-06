using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubOptionsPositionConfigurator : MonoBehaviour
{
    [Header("Suboptions config.")]
    [SerializeField]
    float fFirstSubOptionPositionX = -400;
    [SerializeField]
    float fFirstSubOptionPositionY = 300;
    [SerializeField]
    int fSubOptionSpaceY = 90;


    public Vector2 GetPositionForIndex(int iIndex){
        return new Vector2(fFirstSubOptionPositionX, fFirstSubOptionPositionY - (iIndex*fSubOptionSpaceY));
    }

    public float GetSpaceBetweenOptions(){
        return fSubOptionSpaceY;
    }
}
