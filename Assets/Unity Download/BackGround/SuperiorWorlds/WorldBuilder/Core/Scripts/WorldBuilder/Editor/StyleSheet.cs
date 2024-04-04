using UnityEngine;
using UnityEditor;

namespace SuperiorWorlds {
    public static class StyleSheet {
        public static GUIStyle GetSubTitleStyle() {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.label) {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(5, 5, 3, 3),
            };

            Texture2D backgroundTexture = MakeTex(1, 1, new Color(0f, 0f, 0f, 0.5f));
            titleStyle.normal.background = backgroundTexture;
            titleStyle.normal.textColor = Color.white;
            titleStyle.alignment = TextAnchor.MiddleCenter;

            return titleStyle;
        }

        public static GUIStyle GetDictionaryTitleStyle() {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.label) {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(5, 5, 8, 8),
            };

            Texture2D backgroundTexture = MakeTex(1, 1, new Color(0f, 0f, 0f, 0.5f));
            titleStyle.normal.background = backgroundTexture;
            titleStyle.normal.textColor = Color.white;
            titleStyle.alignment = TextAnchor.MiddleCenter;

            return titleStyle;
        }

        public static Texture2D MakeTex(int width, int height, Color color) {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}
