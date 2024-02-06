using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableTextAnimation : MonoBehaviour
{
    [SerializeField]
    string sAnimParam = "bShow_Text";
    public Animator animText;
    int iShowTextHash;

    public void DisableAnimation(){
        animText.SetBool(iShowTextHash, false);
    }

    public void EnableAnimation(){
        animText.SetBool(iShowTextHash, true);
    }

    public void SetText(string sText){
        this.GetComponent<Text>().text = sText;
    }

    // Start is called before the first frame update
    void Awake()
    {
        animText = this.gameObject.GetComponent<Animator>();
        iShowTextHash = Animator.StringToHash(sAnimParam);
    }
}
