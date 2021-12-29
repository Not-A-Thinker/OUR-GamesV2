using UnityEditor;
using UnityEngine;

namespace BgTools.CastVisualizer
{
    public class CastVisualizerEditorWindow : EditorWindow
    {
        private ActicationState actvationState = ActicationState.Off;
        private enum ActicationState
        {
            Off = 0,
            On = 1
        }

        private CastVisualizerManager castVisualizer;

        private Color physicsRayColor;
        private Color physics2dRayColor;
        private Color hitMarkerColor;

        [MenuItem("Tools/BG Tools/CastVisualizer", false, 1)]
        static void ShowWindow()
        {
            CastVisualizerEditorWindow window = EditorWindow.GetWindow<CastVisualizerEditorWindow>(false, "CastVisualizer");
            window.minSize = new Vector2(270.0f, 300.0f);
            window.name = "CastVisualizer";

            window.Show();
        }

        private void OnEnable()
        {
            castVisualizer = CastVisualizerManager.Instance;

            actvationState = (ActicationState) EditorPrefs.GetInt("BGTools.CastVisualizer.ActiveState", 0);

            castVisualizer.ShowPhysicsCasts = EditorPrefs.GetBool("BGTools.CastVisualizer.ShowPhysicsCasts", castVisualizer.ShowPhysicsCasts);
            castVisualizer.ShowPhysics2DCasts = EditorPrefs.GetBool("BGTools.CastVisualizer.ShowPhysics2DCasts", castVisualizer.ShowPhysics2DCasts);
            castVisualizer.ShowHits = EditorPrefs.GetBool("BGTools.CastVisualizer.ShowHits", castVisualizer.ShowHits);
            castVisualizer.CastBodyVisualization = (CastVisualizerManager.CastBodyVisuType) EditorPrefs.GetInt("BGTools.CastVisualizer.CastBodyVisuType", 0);

            string htmlColor = $"#{EditorPrefs.GetString("BGTools.CastVisualizer.PhysicsCastColor", ColorUtility.ToHtmlStringRGBA(castVisualizer.PhysicsRayColor))}";
            ColorUtility.TryParseHtmlString(htmlColor, out physicsRayColor);
            htmlColor = $"#{EditorPrefs.GetString("BGTools.CastVisualizer.Physics2DCastColor", ColorUtility.ToHtmlStringRGBA(castVisualizer.Physics2dRayColor))}";
            ColorUtility.TryParseHtmlString(htmlColor, out physics2dRayColor);
            htmlColor = $"#{EditorPrefs.GetString("BGTools.CastVisualizer.HitColor", ColorUtility.ToHtmlStringRGBA(castVisualizer.HitMarkerColor))}";
            ColorUtility.TryParseHtmlString(htmlColor, out hitMarkerColor);
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();

            GUIStyle gs = new GUIStyle();
            gs.margin.left = 100;
            gs.margin.right = 2;

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Toggle(actvationState == ActicationState.Off, "Off", EditorStyles.toolbarButton))
                actvationState = ActicationState.Off;

            if (GUILayout.Toggle(actvationState == ActicationState.On, "On", EditorStyles.toolbarButton))
                actvationState = ActicationState.On;

            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt("BGTools.CastVisualizer.ActiveState", (int) actvationState);
                switch(actvationState)
                {
                    case ActicationState.On:
                        castVisualizer.StartVisualizer();
                        break;
                    case ActicationState.Off:
                        castVisualizer.StopVisualizer();
                        break;
                }
            }
            GUILayout.EndHorizontal();

            GUI.enabled = (actvationState == ActicationState.On);

            GUILayout.Label("Show");
            EditorGUI.indentLevel++;

            EditorGUI.BeginChangeCheck();
            castVisualizer.ShowPhysicsCasts = EditorGUILayout.Toggle("Physics", castVisualizer.ShowPhysicsCasts);
            castVisualizer.ShowPhysics2DCasts = EditorGUILayout.Toggle("Physics2D", castVisualizer.ShowPhysics2DCasts);
            castVisualizer.ShowHits = EditorGUILayout.Toggle("Hits", castVisualizer.ShowHits);
            if(EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("BGTools.CastVisualizer.ShowPhysicsCasts", castVisualizer.ShowPhysicsCasts);
                EditorPrefs.SetBool("BGTools.CastVisualizer.ShowPhysics2DCasts", castVisualizer.ShowPhysics2DCasts);
                EditorPrefs.SetBool("BGTools.CastVisualizer.ShowHits", castVisualizer.ShowHits);
            }
            EditorGUI.indentLevel--;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUILayout.Label("Settings");
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            physicsRayColor = EditorGUILayout.ColorField("Physics cast color", physicsRayColor);
            physics2dRayColor = EditorGUILayout.ColorField("Physics2D cast color", physics2dRayColor);
            hitMarkerColor = EditorGUILayout.ColorField("Hit marker color", hitMarkerColor);
            if (EditorGUI.EndChangeCheck())
            {
                castVisualizer.PhysicsRayColor = physicsRayColor;
                castVisualizer.Physics2dRayColor = physics2dRayColor;
                castVisualizer.HitMarkerColor = hitMarkerColor;

                EditorPrefs.SetString("BGTools.CastVisualizer.PhysicsCastColor", ColorUtility.ToHtmlStringRGBA(castVisualizer.PhysicsRayColor));
                EditorPrefs.SetString("BGTools.CastVisualizer.Physics2DCastColor", ColorUtility.ToHtmlStringRGBA(castVisualizer.Physics2dRayColor));
                EditorPrefs.SetString("BGTools.CastVisualizer.HitColor", ColorUtility.ToHtmlStringRGBA(castVisualizer.HitMarkerColor));
            }

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            castVisualizer.CastBodyVisualization = (CastVisualizerManager.CastBodyVisuType) EditorGUILayout.EnumPopup("Body visualization", castVisualizer.CastBodyVisualization);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt("BGTools.CastVisualizer.CastBodyVisuType", (int) castVisualizer.CastBodyVisualization);
            }

            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }
    }
}