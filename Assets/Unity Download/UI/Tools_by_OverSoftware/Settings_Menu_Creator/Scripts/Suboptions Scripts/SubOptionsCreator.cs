using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubOptionsCreator : MonoBehaviour
{
    [SerializeField]
    ColorChangerToFocusedAndUnfocused myColorChanger;
    [SerializeField]
    DictionaryCreator myDictionary;
    [SerializeField]
    OptionsAndSubOptionsComunicator myOptSuboptComunicator;
    [SerializeField]
    PlayerPrefsAndSubOptionsComunicator myPpAndSubOptComunicator;
    [SerializeField]
    SubOptionsPositionConfigurator mySuboptionsConfigurator;

    [SerializeField]
    GameObject goSubOptionPrefab;

    [SerializeField]
    string sSettingsMessage = "Setting Saved...";
    [SerializeField]
    EnableTextAnimation textBackAnination;


    [Tooltip("Watch out! sIsSonOf must match any of the aMenuOptions strings")]
    [SerializeField]
    classSubOption[] aSubOptions;

    [System.Serializable]
    public class classSubOption{
        [SerializeField]
        string sName;
    
        [Tooltip("These string must match any of the aMenuOptions strings")]
        [SerializeField]
        string sIsSonOf; //String with the name of the father Option

        [SerializeField]
        string[] aSValues;
        [SerializeField]
        int iIndexDefault;


        int iID;

        int iLastIndexUsed = 0;

        public string GetName(){
            return sName;
        }

        public string[] GetValues(){
            return aSValues;
        }

        public string GetValueFromPosition(int i){
            return aSValues[i];
        }

        public string GetFather(){
            return sIsSonOf;
        }

        public int GetID(){
            return iID;
        }

        public int GetLastIndexUsed(){
            return iLastIndexUsed;
        }

        public int GetIndexDefault(){
            return iIndexDefault;
        }

        public int GetValuesLength(){
            return aSValues.Length;
        }

        public void SetID(int iNewValue){
            iID = iNewValue;
        }

        public void SetLastIndexUsed(int iNewValue){
            iLastIndexUsed = iNewValue;
        }
    }

    int iSubOptionFocused;

    public void LoadSubOptionsValuesFromPlayerPrefs(){
        myPpAndSubOptComunicator.LoadValuesFromPlayerPrefs(aSubOptions);
    }

    public int GetSubOptionsLength(){
        return aSubOptions.Length;
    }

    public classSubOption[] GetAllSubOptions(){
        return aSubOptions;
    }

    public void BuildSubOptions(){
        textBackAnination.SetText(sSettingsMessage);

        AddAllSubOptionsOfAnOption();

        AddABackButtonForEachSubOptionMenu();
    }

    private void AddAllSubOptionsOfAnOption(){
        string sFather = "";

        for (int i = 0; i < aSubOptions.Length; i++){
            sFather = aSubOptions[i].GetFather();
            
            GameObject goNewSubOption = Instantiate(goSubOptionPrefab, myOptSuboptComunicator.GetObjectFromListWithIndex(sFather).transform.GetChild(0).transform); //El objeto se hace hijo de la opción del menú que se define en "sIsSonOf"

            goNewSubOption.transform.localPosition = mySuboptionsConfigurator.GetPositionForIndex(goNewSubOption.transform.GetSiblingIndex());

            goNewSubOption.GetComponent<Text>().text = aSubOptions[i].GetName();
            UnfocusSubOption(goNewSubOption);
            goNewSubOption.GetComponent<Button>().onClick.AddListener(delegate{SubOptionFocusedEvent(goNewSubOption);});

            aSubOptions[i].SetID(i);

            int iSubOptionIndexTemp = aSubOptions[i].GetID();

            goNewSubOption.name = iSubOptionIndexTemp.ToString();
            //Left Arrow
            AddListenerToLeftArrow(goNewSubOption, iSubOptionIndexTemp);
            //Suboption Value
            AddListenerToSubOptionText(i,goNewSubOption, iSubOptionIndexTemp);
            //Right Arrow
            AddListenerToRightArrow(goNewSubOption, iSubOptionIndexTemp);
        }
    }

    private void AddListenerToLeftArrow(GameObject goNewSubOption, int iSubOptionIndexTemp){
        goNewSubOption.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{SubOptionEventsLeft(goNewSubOption, iSubOptionIndexTemp);});
    }

    private void AddListenerToSubOptionText(int i, GameObject goNewSubOption, int iSubOptionIndexTemp){
        goNewSubOption.transform.GetChild(1).gameObject.GetComponent<Text>().text = aSubOptions[i].GetValueFromPosition(aSubOptions[i].GetLastIndexUsed());
    }

    private void AddListenerToRightArrow(GameObject goNewSubOption, int iSubOptionIndexTemp){
            goNewSubOption.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate{SubOptionEventsRight(goNewSubOption, iSubOptionIndexTemp);});
    }

    private void AddABackButtonForEachSubOptionMenu(){
        int iListLength = myOptSuboptComunicator.GetListCount(); 
  
        //The two last buttons of Options are Back and Quit.
        //These buttons doesn't to be checked in the "for" because they don't have suboptions.
        for (int i = 0; i < iListLength - 2; i++){
            GameObject goBackButton = Instantiate(myOptSuboptComunicator.GetOptionPrefab(), myOptSuboptComunicator.GetObjectFromListWithIndex(i).transform.GetChild(0).transform);

            goBackButton.transform.localPosition = mySuboptionsConfigurator.GetPositionForIndex(goBackButton.transform.GetSiblingIndex());

            int iOptionIndex = i;
            goBackButton.GetComponent<Text>().text = myOptSuboptComunicator.GetBackButtonText();
            ChangeBackButtonTextColorToUnfocused(goBackButton.GetComponent<Text>());
            goBackButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                AddListenerToBackButtonInSubOptionMenu(goBackButton.transform.parent.gameObject, true, iOptionIndex);
            });
        }
    }

    public void AddListenerToBackButtonInSubOptionMenu(GameObject goParent, bool bSaveSubOptions, int iFather){
        if(bSaveSubOptions)
        {
            ResetThisSubOptionAndBack(goParent, iFather);
        }

//        iSubOptionFocused = 0;
        goParent.SetActive(false);
        myOptSuboptComunicator.BackToOptions(iFather);
    }

    public void ResetThisSubOptionAndBack(GameObject parent, int iFather)
    {
        iSubOptionFocused = 0;
        ChangeOptionTextColorToUnfocused(parent.transform.parent.GetComponent<Text>());
        myPpAndSubOptComunicator.SaveValuesInPlayerPrefs(aSubOptions, iFather);
        textBackAnination.EnableAnimation();
    }
    
    public int GetCurrentSubOptionFocused(){
        return iSubOptionFocused;
    }

    public void IncreaseSubOptionFocused(int iLastIndexOfSubOptionList){
        iSubOptionFocused++;

        if(iSubOptionFocused > iLastIndexOfSubOptionList){
            iSubOptionFocused = 0;
        }
    }

    public void DecreaseSubOptionFocused(int iLastIndexOfSubOptionList){
        iSubOptionFocused--;

        if(iSubOptionFocused < 0){
            iSubOptionFocused = iLastIndexOfSubOptionList;
        }
    }

    public void ActivateAllSuboptionsOfAnOption(List<GameObject> listOptions, string sName){
            int iLength = listOptions.Count;     
            int iOptionFocused = myDictionary.GetIndexFromValue(sName);
            iSubOptionFocused = 0;

            ResetSubOptions(listOptions[iOptionFocused].transform.GetChild(0).gameObject);

            EnableSubOptionsFocusedAndDisableSubOptionsUnfocused(listOptions, iLength, iOptionFocused);

            ChangeBackButtonTextColorToUnfocused(listOptions[iLength - 2].GetComponent<Text>()); 
            ChangeQuitButtonTextColorToUnfocused(listOptions[iLength - 1].GetComponent<Text>()); 

            FocusSubOption(listOptions[iOptionFocused].transform.GetChild(0).transform.GetChild(0).gameObject);
    }

    private void EnableSubOptionsFocusedAndDisableSubOptionsUnfocused(List<GameObject> listOptions, int iLength, int iOptionFocused){
            for(int i = 0; i < iLength - 2; i++){ //Last 2 buttons are Back and Quit and they don't have suboptions
                listOptions[i].transform.GetChild(0).gameObject.SetActive(i == iOptionFocused);
                ChangeOptionTextColorToUnfocused(listOptions[i].GetComponent<Text>());
            }

            ChangeOptionTextColorToFocused(listOptions[iOptionFocused].GetComponent<Text>());
    }

    private void FocusSubOption(GameObject goSubOptionFocused){
        //Text Suboption
        ChangeSubOptionTextColorToFocused(goSubOptionFocused.GetComponent<Text>());
        if(goSubOptionFocused.transform.childCount > 0){
            //Left arrow
            ChangeLeftArrowTextColorToFocused(goSubOptionFocused.transform.GetChild(0).GetComponent<Text>());
            //Suboption selected
            ChangeSubOptionTextColorToFocused(goSubOptionFocused.transform.GetChild(1).GetComponent<Text>());
            //Right arrow
            ChangeRightArrowTextColorToFocused(goSubOptionFocused.transform.GetChild(2).GetComponent<Text>());
        }
    }

    private void UnfocusSubOption(GameObject goSubOptionFocused){
        //Text Suboption
        ChangeSubOptionTextColorToUnfocused(goSubOptionFocused.GetComponent<Text>());
        if(goSubOptionFocused.transform.childCount > 0){
            //Left arrow
            ChangeLeftArrowTextColorToUnfocused(goSubOptionFocused.transform.GetChild(0).GetComponent<Text>());
            //Suboption selected
            ChangeSubOptionTextColorToUnfocused(goSubOptionFocused.transform.GetChild(1).GetComponent<Text>());
            //Right arrow
            ChangeRightArrowTextColorToUnfocused(goSubOptionFocused.transform.GetChild(2).GetComponent<Text>());
        }
    }


//Para The Power ups: lo he hecho así para favorecer la lectura pero  no sé si esto es duplicar código y sería mejor directamente llamar a la función 
    public void ChangeBackButtonTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }

    public void ChangeBackButtonTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeQuitButtonTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }

    public void ChangeOptionTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }

    public void ChangeQuitButtonTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeOptionTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

    private void ChangeLeftArrowTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);      
    }

    private void ChangeLeftArrowTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);      
    }
    private void ChangeRightArrowTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);      
    }

    private void ChangeRightArrowTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);      
    }

    public void ChangeSubOptionTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeSubOptionTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }
///////////////////////////////////////

    void ResetSubOptions(GameObject goParent){
        for (int i = 0; i < goParent.transform.childCount; i++){
            UnfocusSubOption(goParent.transform.GetChild(i).gameObject);
        }
    }


    private void ApplyArrowEvents(GameObject goSubOptionSelected, int iSubOptionID, int iIndex){
        
        FocusSubOption(goSubOptionSelected);

        iSubOptionFocused = goSubOptionSelected.transform.GetSiblingIndex();

        aSubOptions[iSubOptionID].SetLastIndexUsed(iIndex);

        SetTextToSubOptionSelected(goSubOptionSelected, iSubOptionID, iIndex);

        UnfocusUnselectedSubOptions(goSubOptionSelected);
    }

    private void UnfocusUnselectedSubOptions(GameObject goSubOptionSelected){
        for (int i = 0; i < goSubOptionSelected.transform.parent.childCount; i++){
            if(goSubOptionSelected.transform.parent.GetChild(i).GetSiblingIndex() != goSubOptionSelected.transform.GetSiblingIndex()){
                UnfocusSubOption(goSubOptionSelected.transform.parent.GetChild(i).gameObject);
            }
        }        
    }

    public void UnfocusCurrentSubOption(GameObject goParentOption){
        UnfocusSubOption(goParentOption.transform.GetChild(0).GetChild(iSubOptionFocused).gameObject);
    }

    public void FocusCurrentSubOption(GameObject goParentOption){
        FocusSubOption(goParentOption.transform.GetChild(0).GetChild(iSubOptionFocused).gameObject);
    }

    private void SetTextToSubOptionSelected(GameObject goSubOptionSelected, int iSubOptionID, int iIndex){
        goSubOptionSelected.transform.GetChild(1).gameObject.GetComponent<Text>().text = aSubOptions[iSubOptionID].GetValues()[iIndex];
    }

    //This event is assigned to the left arrow to go through the list of options
    private void SubOptionEventsLeft(GameObject goSubOptionSelected, int iSubOptionID){       
        int iIndex = aSubOptions[iSubOptionID].GetLastIndexUsed();

        iIndex--;
        if (iIndex < 0){
            iIndex = aSubOptions[iSubOptionID].GetValues().Length - 1;
        }

        ApplyArrowEvents(goSubOptionSelected, iSubOptionID, iIndex);
    }

    //This event is assigned to the right arrow to go through the list of options
    private void SubOptionEventsRight(GameObject goSubOptionSelected, int iSubOptionID){
        int iIndex = aSubOptions[iSubOptionID].GetLastIndexUsed();

        iIndex++;
        if (iIndex > aSubOptions[iSubOptionID].GetValues().Length-1){
            iIndex = 0;
        }

        ApplyArrowEvents(goSubOptionSelected, iSubOptionID, iIndex);
    }

    public void CurrentSubOptionEventLeft(GameObject goParent){
        SubOptionEventsLeft(goParent.transform.GetChild(0).GetChild(iSubOptionFocused).gameObject, int.Parse(goParent.transform.GetChild(0).GetChild(iSubOptionFocused).name));
    }

    public void CurrentSubOptionEventRight(GameObject goParent){
        SubOptionEventsRight(goParent.transform.GetChild(0).GetChild(iSubOptionFocused).gameObject, int.Parse(goParent.transform.GetChild(0).GetChild(iSubOptionFocused).name));
    }


    //This event focus the suboption selected by mouse click
    private void SubOptionFocusedEvent(GameObject goSubOptionFocused){
        ResetSubOptions(goSubOptionFocused.transform.parent.gameObject);
        FocusSubOption(goSubOptionFocused);
        iSubOptionFocused = goSubOptionFocused.transform.GetSiblingIndex();
    }
}
