using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsAndSubOptionsComunicator : MonoBehaviour
{
    [SerializeField]
    MenuPlayerPrefsCoordinator myPlayerPrefs;
    [SerializeField]
    SubOptionsCreator mySubOptions;


    public int GetSubOptionsLength(){
        return mySubOptions.GetSubOptionsLength();
    }

    public SubOptionsCreator.classSubOption[] GetSubOptions(){
        return mySubOptions.GetAllSubOptions();
    }

    public void LoadValuesFromPlayerPrefs(SubOptionsCreator.classSubOption[] aSubOptions)
    {
        myPlayerPrefs.LoadValuesFromPlayerPrefs(aSubOptions);
    }

    public void SaveValuesInPlayerPrefs(SubOptionsCreator.classSubOption[] aSubOptions, int iFather){
        myPlayerPrefs.SaveValuesInPlayerPrefs(aSubOptions,iFather);
    }

}
