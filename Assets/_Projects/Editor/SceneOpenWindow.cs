#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _Projects.Editor
{
    public class SceneOpenWindow : EditorWindow
    {
        [MenuItem("Window/Scene Open Window")]
        public static void OpenWindow()
        {
            GetWindow<SceneOpenWindow>("Scene Open Window");
        }

        private string[] _allScenePaths;
        private string _filterText = "Assets/_Projects/Scenes/";
        private GUIStyle _buttonStyle;
        private Vector2 _scrollPosition; // スクロール位置を管理する変数

        private void OnEnable() => RefreshSceneList();

        private void OnGUI()
        {
            _buttonStyle ??= CreateButtonStyle();

            if (_buttonStyle == null) return;

            // フィルター入力フィールド
            EditorGUILayout.LabelField("Filter by path:", EditorStyles.boldLabel);
            _filterText = EditorGUILayout.TextField(_filterText);
            EditorGUILayout.Space();

            // 更新ボタン
            if (GUILayout.Button("Refresh")) RefreshSceneList();
            EditorGUILayout.Space();

            // ここからスクロールビューを開始
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            string[] displayedScenes = string.IsNullOrEmpty(_filterText)
                ? _allScenePaths
                : System.Array.FindAll(_allScenePaths, path => path.ToLower().Contains(_filterText.ToLower()));

            var sceneIconContent = EditorGUIUtility.IconContent("SceneAsset Icon");

            foreach (string scenePath in displayedScenes)
            {
                string label = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                var content = new GUIContent(label, sceneIconContent.image);

                if (GUILayout.Button(content, _buttonStyle))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
            }

            // ここでスクロールビューを終了
            EditorGUILayout.EndScrollView();
        }

        private void RefreshSceneList()
        {
            string[] guids = AssetDatabase.FindAssets("t:Scene");
            _allScenePaths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
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