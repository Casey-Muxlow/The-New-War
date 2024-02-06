using UnityEditor;
namespace DIG.Tools
{
    public static class UIExtensionMenus
    {
        [MenuItem("GameObject/UI/Carrousel(TMP)", priority = -50)]
        private static void CreateArrowSelector()
        {
            CreateUtility.CreateUIObject("Carrousel(TMP)");
        }

        [MenuItem("GameObject/UI/SlideToggle(TMP)", priority = -50)]
        private static void CreateSlideToggle()
        {
            CreateUtility.CreateUIObject("SlideToggle");
        }

        [MenuItem("GameObject/UI/RadioBtnContainer(TMP)", priority = -50)]
        private static void CreateRadioBtnContainer()
        {
            CreateUtility.CreateUIObject("RadioBtns_Container");
        }

        [MenuItem("GameObject/UI/ProgresBar", priority = -50)]
        private static void CreateLinearBar()
        {
            CreateUtility.CreateUIObject("ProgresBar");
        }

        [MenuItem("GameObject/UI/CirclularProgresBar", priority = -50)]
        private static void CreateCircularBar()
        {
            CreateUtility.CreateUIObject("CirclularProgresBar");
        }
        [MenuItem("GameObject/UI/PopUp(TMP)", priority = -50)]
        private static void CreatePopUp()
        {
            CreateUtility.CreateUIObject("PopUp(TMP)");
        }

    }

}