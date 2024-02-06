using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundsProvider : MonoBehaviour
{
    [SerializeField]
    AudioSource audioNavigation;
    [SerializeField]
    AudioSource audioClick;

    public void PlayAudioNavigation(){
        audioNavigation.Play();
    }

    public void PlayAudioClick(){
        audioClick.Play();
    }
}
