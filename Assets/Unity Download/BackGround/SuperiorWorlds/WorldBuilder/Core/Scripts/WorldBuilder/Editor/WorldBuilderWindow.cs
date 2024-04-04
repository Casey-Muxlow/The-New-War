using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace SuperiorWorlds
{
    [HideInInspector]
	public class WorldBuilderWindow : EditorWindow
    {
	    private WorldBuilder worldBuilder;
	    private Vector2 leftScrollPosition;
	    private Vector2 rightScrollPosition;
	    private int selectedLayerIndex = -1; // Add a variable to store the selected layer index
        private SerializedObject serializedObject; // To serialize fields
		
		// tab drawers for layer element types
		private ITabDrawer unityTreesTabDrawer;
        private ITabDrawer unityDetailsTabDrawer;
        // private ITabDrawer fooTabDrawer;
		
		public int tabs = 2; // 3;
		string[] tabOptions = new string[] { "Unity Trees", "Unity Details"}; //, "Foo" };
		
        public static void OpenWorldBuilderEditor()
        {
			WorldBuilderWindow window = GetWindow<WorldBuilderWindow>();
            window.titleContent = new GUIContent("World Builder Editor");
            window.minSize = new Vector2(1000f, 600f); // Set the minimum size of the window
            window.Show();
        }
		
		private void OnFocus()
        {
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
            	// Find the WorldBuilder script in the scene when the window receives focus
            	worldBuilder = FindObjectOfType<WorldBuilder>();
            }

            // Repaint the window to update the content
            Repaint();
        }
		
		private void OnEnable()
		{
		            unityTreesTabDrawer = new UnityTreesTabDrawer(worldBuilder, selectedLayerIndex);
            unityDetailsTabDrawer = new UnityDetailsTabDrawer(worldBuilder, selectedLayerIndex);

		    // Find the WorldBuilder script in the scene
		    worldBuilder = FindObjectOfType<WorldBuilder>();
		
		    if (worldBuilder == null)
		    {
		        // If the WorldBuilder component is not found, find an active terrain and attach the WorldBuilder component
		        Terrain activeTerrain = Terrain.activeTerrain;
		        if (activeTerrain != null)
		        {
		            // Create a new GameObject as a child of the terrain
		            GameObject worldBuilderObject = new GameObject("World Builder");
		
		            // Attach the WorldBuilder component to the new GameObject
		            worldBuilder = worldBuilderObject.AddComponent<WorldBuilder>();
		
		            // Assign the terrain to the WorldBuilder component
		            worldBuilder.terrain = activeTerrain;
		
		            // Set the Detail Scatter Mode to Instance Count Mode (for Unity 2022 and later)
		            #if UNITY_2022_1_OR_NEWER
		            activeTerrain.terrainData.SetDetailScatterMode(DetailScatterMode.InstanceCountMode);
					Debug.Log("Detail Scatter Mode set to 'Instance Count Mode'");
		            #endif
					
		            // Make the new GameObject a child of the terrain
		            worldBuilderObject.transform.parent = activeTerrain.transform;
		        }
		    }
		
		    // Repaint the window to update the content
		    Repaint();
		}
		
		private void OnGUI()
		{
			if (worldBuilder == null && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorGUILayout.LabelField("Cannot Change Settings in Play Mode.", EditorStyles.boldLabel);
                return;
            }
			if (worldBuilder == null)
            {
                EditorGUILayout.LabelField("Error: WorldBuilder component not found on the terrain in the scene. Add a terrain to proceed.", EditorStyles.boldLabel);
                return;
            }
		    // Load the background image
		    Texture2D backgroundTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/SuperiorWorlds/WorldBuilder/Core/Scripts/WorldBuilder/Editor/Background2.png");
		
		    // Stretch the background image to fill the window
		    GUI.DrawTexture(new Rect(0, 0, position.width, position.height), backgroundTexture, ScaleMode.StretchToFill);
		
		    // Display the header with the shadow and outline effects
		    DrawHeader();
		
		    // GUILayout.Space(2f); // Add some space below the header
		    EditorGUILayout.BeginHorizontal(GUILayout.Height(position.height - 95f)); // Adjust the height to accommodate header, footer, and terrain field
		
		    // Left Column (200px wide)
		    GUIStyle leftColumnStyle = new GUIStyle(GUI.skin.box)
		    {
		        normal = new GUIStyleState() { background = StyleSheet.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.75f)) },
		        stretchHeight = true
		    };

		    leftScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition, leftColumnStyle, GUILayout.Width(210), GUILayout.Height(position.height - 100f));
		    EditorGUILayout.BeginVertical(GUILayout.Width(200));
		    DrawLayerList();
		    EditorGUILayout.EndVertical();
		    EditorGUILayout.EndScrollView();
		
		    // Right Column (600px wide)
		    GUIStyle rightColumnStyle = new GUIStyle(GUI.skin.box)
		    {
		        normal = new GUIStyleState() { background = StyleSheet.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.75f)) },
		        stretchHeight = true
		    };

		    rightScrollPosition = EditorGUILayout.BeginScrollView(rightScrollPosition, rightColumnStyle, GUILayout.ExpandWidth(true), GUILayout.Height(position.height - 100f));
		    EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));


    		DrawLayerDetails();
			
		    EditorGUILayout.EndVertical();
		    EditorGUILayout.EndScrollView();
		
		    EditorGUILayout.EndHorizontal();
		
		    // Footer (100px high)
		    GUILayout.Space(10f); // Add some space above the footer
		    EditorGUILayout.LabelField("Footer Area", GUILayout.Height(100f)); // Add any additional UI elements in the footer
		}
		
		////
		//// Draw Header
		////
		private void DrawHeader()
		{
		    // Set the RGB values for the header color (e.g., red)
		    Color headerColor = new Color(.78f, .82f, .773f); // Red
		
		    // Create the SerializedObject in the OnGUI method
		    serializedObject = new SerializedObject(worldBuilder);
			
		    // Increase the font size of the header title to 30px
		    GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
		    {
		        fontSize = 30,
		        alignment = TextAnchor.MiddleCenter,
		        normal = new GUIStyleState() { textColor = headerColor },
		        fontStyle = FontStyle.Normal, // Change the font style (Bold and Italic)
		        wordWrap = true // Allow the text to wrap if it's too long
		    };
		
		    // Display the header without the outline effect
		    EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true)); // Expand the width to fit the window
		    EditorGUILayout.LabelField("World Builder", headerStyle, GUILayout.Height(40f));
		
		    // Display the terrain object selector field in the header
		    worldBuilder.terrain = EditorGUILayout.ObjectField("Terrain Object", worldBuilder.terrain, typeof(Terrain), true) as Terrain;
			GUILayout.Space(10f); // Add some space above the footer

		    EditorGUILayout.EndVertical();
		}

		////
		//// Draw Layers List
		////
		private void DrawLayerList()
		{
		    EditorGUILayout.BeginVertical(GUI.skin.box);
		    GUILayout.Label("Layers", StyleSheet.GetSubTitleStyle(), GUILayout.Height(24f));
		
		    for (int i = 0; i < worldBuilder.worldLayers.Count; i++)
		    {
		        // Add extra padding around the layer name
		        GUIStyle layerButtonStyle = new GUIStyle(GUI.skin.button)
		        {
		            padding = new RectOffset(5, 5, 5, 5)
		        };
		
		        // Check if the current layer index matches the selected layer index
		        if (selectedLayerIndex == i)
		        {
		            // If they match, set a different background color to highlight the selected layer
		            Color selectedColor = new Color(0.2f, 0.2f, 0.2f);
		            Texture2D selectedBackground = StyleSheet.MakeTex(1, 1, selectedColor);
		            layerButtonStyle.normal.background = selectedBackground;
		        }
		        else
		        {
		            // For unselected layers, use the default button style (no change in color)
		            Color unselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		            Texture2D unselectedBackground = StyleSheet.MakeTex(1, 1, unselectedColor);
		            layerButtonStyle.normal.background = unselectedBackground;
		        }
		        // Render the layer button using the appropriate style
		        if (GUILayout.Button(worldBuilder.worldLayers[i].name, layerButtonStyle))
		        {
		            selectedLayerIndex = i;
		        }
		    }
		
		    if (GUILayout.Button("+ Add", GUILayout.Width(80f)))
		    {
		        var newLayer = new WorldLayer();
		        worldBuilder.worldLayers.Add(newLayer);
		        EditorGUI.FocusTextInControl("LayerName" + (worldBuilder.worldLayers.Count - 1));
		    }
		    EditorGUILayout.EndVertical();
		}

		////
		//// Draw Layer Details
		////
	    public void DrawLayerDetails()
	    {
			unityTreesTabDrawer = new UnityTreesTabDrawer(worldBuilder, selectedLayerIndex);
            unityDetailsTabDrawer = new UnityDetailsTabDrawer(worldBuilder, selectedLayerIndex);
			// Check if the worldLayers list is empty
		    if (worldBuilder.worldLayers.Count == 0 || selectedLayerIndex < 0 || selectedLayerIndex >= worldBuilder.worldLayers.Count)
		    {
		        GUILayout.Space(10f);
		        EditorGUILayout.LabelField("Select a Layer");
		        return; // Return from the method since there are no layers to display
		    }
			
			// Get the layer name based on the selectedLayerIndex
	        string layerName = worldBuilder.worldLayers[selectedLayerIndex].name;
	
	        // Display the layer name in the title
	        GUILayout.Label(layerName + " Layer Details", StyleSheet.GetSubTitleStyle(), GUILayout.Height(24f));

	
	        if (worldBuilder.worldLayers.Count > 0 && selectedLayerIndex >= 0 && selectedLayerIndex < worldBuilder.worldLayers.Count)
	        {
	            EditorGUILayout.BeginVertical(GUI.skin.box);
	
	            DrawLayerConfiguration();
	
	            EditorGUILayout.EndVertical();
	
	            EditorGUILayout.BeginVertical(GUI.skin.box);
	
	            DrawButtonsSection();
	
	            EditorGUILayout.EndVertical();
	
	            EditorGUILayout.BeginVertical(GUI.skin.box);
				
				// Tab Options
				tabs = GUILayout.Toolbar(tabs, tabOptions);
				switch (tabs)
				{
					case 0:
						unityTreesTab();
						break;
					case 1:
						unityDetailsTab();
						break;
				// 	case 2:
				// 		fooTab();
				// 		break;
					default:
	        			unityTreesTab();
	        			break;
				}
				
	            EditorGUILayout.EndVertical();
	
	            EditorGUILayout.BeginVertical(GUI.skin.box);
	
	            EditorGUILayout.EndVertical();
	        }
	        else
	        {
	            GUILayout.Space(10f);
	            EditorGUILayout.LabelField("Select a Layer");
	        }
	    }
		
		// Method to display Layer Configuration (Layer Name, Heatmap Texture, Threshold)
		private void DrawLayerConfiguration()
		{
		    EditorGUI.BeginChangeCheck();
		    string newLayerName = EditorGUILayout.TextField("Layer Name", worldBuilder.worldLayers[selectedLayerIndex].name);
		    if (EditorGUI.EndChangeCheck())
		    {
		        worldBuilder.worldLayers[selectedLayerIndex].name = newLayerName;
		    }
		
		    // Display the PNG heatmap selector
		    worldBuilder.worldLayers[selectedLayerIndex].heatmapTexture = EditorGUILayout.ObjectField("Heatmap Texture", worldBuilder.worldLayers[selectedLayerIndex].heatmapTexture, typeof(Texture2D), false) as Texture2D;
		
		    // Display the threshold slider
		    worldBuilder.worldLayers[selectedLayerIndex].minimumThreshold = EditorGUILayout.Slider("Minimum Threshold", worldBuilder.worldLayers[selectedLayerIndex].minimumThreshold, 0f, 1f);
			worldBuilder.worldLayers[selectedLayerIndex].maximumThreshold = EditorGUILayout.Slider("Maximum Threshold", worldBuilder.worldLayers[selectedLayerIndex].maximumThreshold, 0f, 1f);
		}
		
		// Method to display the buttons (Populate Layer, Delete Layer)
		private void DrawButtonsSection()
		{
		    EditorGUILayout.BeginHorizontal();
		
		    // Create a custom GUIStyle for the 'Delete Layer' button (dark red color)
		    GUIStyle deleteLayerButtonStyle = new GUIStyle(GUI.skin.button)
		    {
		        normal = new GUIStyleState() { textColor = Color.red },
		        fontStyle = FontStyle.Bold,
		        padding = new RectOffset(10, 10, 5, 5), // Add vertical padding here (5px top and bottom)
		    };
		
		    // Create a custom GUIStyle for the 'Populate Layer' button (dark green color)
		    GUIStyle populateLayerButtonStyle = new GUIStyle(GUI.skin.button)
		    {
		        normal = new GUIStyleState() { textColor = Color.green },
		        fontStyle = FontStyle.Bold,
		        padding = new RectOffset(10, 10, 5, 5), // Add vertical padding here (5px top and bottom)
		    };
		
		    // Background textures for buttons
		    Texture2D darkRedBackground = StyleSheet.MakeTex(1, 1, new Color(0.8f, 0f, 0f, 0.7f)); // Dark red background
		    Texture2D lightRedBackground = StyleSheet.MakeTex(1, 1, new Color(0.9f, 0f, 0f, 0.7f)); // Light red background
		    Texture2D darkGreenBackground = StyleSheet.MakeTex(1, 1, new Color(0f, 0.8f, 0f, 0.7f)); // Dark green background
		    Texture2D lightGreenBackground = StyleSheet.MakeTex(1, 1, new Color(0f, 0.9f, 0f, 0.7f)); // Light green background
		
		    // 'Populate Layer' button
			if (GUILayout.Button("Populate Layer", populateLayerButtonStyle, GUILayout.ExpandWidth(true), GUILayout.MinWidth(250f)))
		    {
		        if (selectedLayerIndex >= 0 && selectedLayerIndex < worldBuilder.worldLayers.Count)
		        {
		            WorldLayer selectedLayer = worldBuilder.worldLayers[selectedLayerIndex];
		
		            // Check if the heatmapTexture is assigned and if it is not readable
		            if (selectedLayer.heatmapTexture != null && !selectedLayer.heatmapTexture.isReadable)
		            {
		                // Set the texture to readable using AssetImporter
		                string assetPath = AssetDatabase.GetAssetPath(selectedLayer.heatmapTexture);
		                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
		                textureImporter.isReadable = true;
		                AssetDatabase.ImportAsset(assetPath);
		            }
		
		            selectedLayer.PopulateLayer(worldBuilder);
		        }
		    }
			
		    // Set custom background colors for the buttons based on hover state
		    if (GUILayout.Button("Delete Layer", deleteLayerButtonStyle, GUILayout.Width(150f)))
		    {
		        // Prompt for confirmation before removing the layer
		        bool confirmed = EditorUtility.DisplayDialog("Confirm Layer Removal", "Are you sure you want to remove this layer?", "Remove", "Cancel");
		        if (confirmed)
		        {
		            worldBuilder.worldLayers.RemoveAt(selectedLayerIndex);
		            selectedLayerIndex = -1;
		        }
		    }
			
		    EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.HelpBox("Changing prefabs after population will orphan those populations. To avoid, remove the element first then add a new element with the desired prefab selected and populate again.", MessageType.Warning);
			
			EditorGUILayout.EndHorizontal();

		}
		
		private void unityTreesTab() {
			unityTreesTabDrawer.DrawTab();
			GUILayout.FlexibleSpace();
		}
		
		private void unityDetailsTab() {
			unityDetailsTabDrawer.DrawTab();
			GUILayout.FlexibleSpace();
		}
		
		// private void fooTab() {
		// 	// fooTabDrawer.DrawTab();
		// 	GUILayout.FlexibleSpace();
		// }
    }
}
