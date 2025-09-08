#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _Projects.Utils.Editor
{
    public class SceneOpenWindow : EditorWindow
    {
        [MenuItem("Window/Scene Open Window")]
        public static void OpenWindow()
        {
            GetWindow<SceneOpenWindow>("Scene Open Window");
        }

        private string[] _allScenePaths; // 全シーンのパス
        private string _filterText = "Assets/_Projects/Scenes/"; // 入力されたフィルター文字列
        private GUIStyle _buttonStyle;
        private GUIContent _sceneIconContent;

        private void OnEnable() => RefreshSceneList();

        private void OnGUI()
        {
            _buttonStyle ??= CreateButtonStyle();

            // ボタンスタイルがnullの時がある
            if (_buttonStyle == null) return;

            // フィルター入力フィールド
            EditorGUILayout.LabelField("Filter by path:", EditorStyles.boldLabel);
            _filterText = EditorGUILayout.TextField(_filterText);
            EditorGUILayout.Space();

            // 更新ボタン
            if (GUILayout.Button("Refresh")) RefreshSceneList();
            EditorGUILayout.Space();

            var displayedScenes = string.IsNullOrEmpty(_filterText)
                ? _allScenePaths
                : System.Array.FindAll(_allScenePaths, path => path.ToLower().Contains(_filterText.ToLower()));

            var sceneIconContent = EditorGUIUtility.IconContent("SceneAsset Icon");

            foreach (var scenePath in displayedScenes)
            {
                var label = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                var content = new GUIContent(label, sceneIconContent.image);

                if (GUILayout.Button(content, _buttonStyle))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
            }
        }

        private void RefreshSceneList()
        {
            var guids = AssetDatabase.FindAssets("t:Scene");
            _allScenePaths = new string[guids.Length];
            for (var i = 0; i < guids.Length; i++)
            {
                _allScenePaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            Repaint();
        }

        private static GUIStyle CreateButtonStyle()
            => new(EditorStyles.miniButton)
            {
                fixedHeight = 20f,
                alignment = TextAnchor.MiddleLeft,
                imagePosition = ImagePosition.ImageLeft
            };
    }
}
#endif