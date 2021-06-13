using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScrubBar))]
public class ScrubBarEditor : Editor {
	public override void OnInspectorGUI() {
		//The target variable is the selected MyBehaviour.

		ScrubBar scrubBar = (ScrubBar) target;
		SerializedProperty videoPlayer = serializedObject.FindProperty("videoPlayer");
		SerializedProperty audioSource = serializedObject.FindProperty("audioSource");
		SerializedProperty scrubCursor = serializedObject.FindProperty("scrubCursor");

		EditorGUIUtility.LookLikeInspector();
		EditorGUI.BeginChangeCheck();

		scrubBar.isProgressionBar = EditorGUILayout.Toggle("Is Progression Bar", scrubBar.isProgressionBar);

		if (scrubBar.isProgressionBar) {
			EditorGUILayout.PropertyField(videoPlayer, true);
		} else {
			EditorGUILayout.PropertyField(audioSource, true);
		}

		//EditorGUILayout.PropertyField(scrubCursor, true);

		if (EditorGUI.EndChangeCheck())
		 serializedObject.ApplyModifiedProperties();

		EditorGUIUtility.LookLikeControls();

	}
}
