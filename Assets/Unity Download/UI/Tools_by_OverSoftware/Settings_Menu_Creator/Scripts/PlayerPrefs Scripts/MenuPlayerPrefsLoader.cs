using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerPrefsLoader : MonoBehaviour
{
    public void LoadMenuSubOptionValuesFromPlayerPrefs(string[] sPlayerPrefsStrings, SubOptionsCreator.classSubOption[] aSubOptions, string sPlayerPrefsFirstTimeSaved){
        Debug.Log("Loading PlayerPrefs");
        for(int i = 0; i < aSubOptions.Length; i++){
            if(PlayerPrefs.GetInt(sPlayerPrefsFirstTimeSaved) != 0)
            {
                sPlayerPrefsStrings[i] = (i + aSubOptions[i].GetName());
                aSubOptions[i].SetLastIndexUsed(PlayerPrefs.GetInt(sPlayerPrefsStrings[i]));
            }
            else
                aSubOptions[i].SetLastIndexUsed(aSubOptions[i].GetIndexDefault());
        }
   }
}
