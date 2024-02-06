using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerPrefsSaver : MonoBehaviour
{
    [SerializeField]
    DictionaryCreator myDictionary;
    public void SaveMenuSubOptionValuesInPlayerPrefs(SubOptionsCreator.classSubOption[] aSubOptions, string sPlayerPrefsFirstTimeSaved,int iFather){
        PlayerPrefs.SetInt(sPlayerPrefsFirstTimeSaved, 1);
        for(int i = 0; i < aSubOptions.Length; i++){
            if(myDictionary.GetIndexFromValue(aSubOptions[i].GetFather()) == iFather)
                PlayerPrefs.SetInt(i+ aSubOptions[i].GetName(), aSubOptions[i].GetLastIndexUsed());
        }
    }
}
