using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerPrefsCoordinator : MonoBehaviour
{
    [SerializeField]
    MenuPlayerPrefsLoader myPPLoader;
    [SerializeField]
    MenuPlayerPrefsSaver myPPSaver;
    [SerializeField]
    string sPlayerPrefsFirstTimeSavedKey = "PlayerPrefsFirstTimeSaved";

    [SerializeField]
    PlayerPrefsAndSubOptionsComunicator myPpAndSubOptComunicator;

    public string[] GetPlayerPrefsStrings(){
        return sPlayerPrefsStrings;
    }

    public void CreateArrayOfPlayerPrefs(SubOptionsCreator.classSubOption[] aSubOptions){
        sPlayerPrefsStrings = new string[aSubOptions.Length];
    }

    private string[] sPlayerPrefsStrings;
    public void LoadValuesFromPlayerPrefs(SubOptionsCreator.classSubOption[] aSubOptions){
        CreateArrayOfPlayerPrefs(aSubOptions);
        myPPLoader.LoadMenuSubOptionValuesFromPlayerPrefs(sPlayerPrefsStrings, aSubOptions, sPlayerPrefsFirstTimeSavedKey);
    }

    public void SaveValuesInPlayerPrefs(SubOptionsCreator.classSubOption[] aSubOptions, int iFather){
        myPPSaver.SaveMenuSubOptionValuesInPlayerPrefs(aSubOptions, sPlayerPrefsFirstTimeSavedKey, iFather);
    }

    public void ResetMenuPlayerPrefs(){
        SubOptionsCreator.classSubOption[] aSubOptions = myPpAndSubOptComunicator.GetSubOptions();
        Debug.Log("Deleting PlayerPrefs");
        PlayerPrefs.DeleteKey(sPlayerPrefsFirstTimeSavedKey);
        for(int i = 0; i < aSubOptions.Length; i++){
            PlayerPrefs.DeleteKey(aSubOptions[i].GetName());
        }
    }
}
