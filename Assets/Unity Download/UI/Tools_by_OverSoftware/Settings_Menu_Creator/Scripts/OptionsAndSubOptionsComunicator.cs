using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAndSubOptionsComunicator : MonoBehaviour
{
    [SerializeField]
    OptionsCreator myOptions;
    [SerializeField]
    SubOptionsCreator mySubOptions;

    public void BackToOptions(int optionFocusedIndex)
    {
        myOptions.SetIndexOfOptionFocused(optionFocusedIndex);
        myOptions.ResetOptions();
        myOptions.EnableClickInButtons(true);
    }
    
    public void ActivateAllSuboptionsOfAnOption(List<GameObject> listOptions, string sName){
        Debug.Log("Mostrando sub opciones");
        myOptions.EnableClickInButtons(false);
        mySubOptions.ActivateAllSuboptionsOfAnOption(listOptions, sName);
    }

    public GameObject GetObjectFromListWithIndex(string sValueInDictionary){
        return myOptions.GetObjectFromListWithIndex(sValueInDictionary);
    }

    public int GetListCount(){
        return myOptions.GetListCount(); 
    }

    public GameObject GetOptionPrefab(){
        return(myOptions.GetOptionPrefab());
    }

    public GameObject GetObjectFromListWithIndex(int iIndex){
        return myOptions.GetObjectFromListWithIndex(iIndex);
    }

    public string GetBackButtonText(){
        return myOptions.GetBackButtonText();
    }

}
