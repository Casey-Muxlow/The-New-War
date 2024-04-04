using UnityEditor;

namespace SuperiorWorlds
{
    public static class SuperiorWorldsMenu
    {
        [MenuItem("Window/World Builder/World Builder Editor", priority = 1)]
        public static void OpenWorldBuilderEditor()
        {
            WorldBuilderWindow.OpenWorldBuilderEditor(); // Call the method in WorldBuilderWindow
        }
    }
}
