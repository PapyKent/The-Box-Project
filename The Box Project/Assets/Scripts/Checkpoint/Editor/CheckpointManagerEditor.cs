using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckpointManager), true)]
public class CheckpointManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying)
		{
			SerializedProperty checkpointsProp = serializedObject.FindProperty(CHECKPOINTS_NAME);
			for (int i = 0; i < checkpointsProp.arraySize; i++)
			{
				Checkpoint checkpoint = checkpointsProp.GetArrayElementAtIndex(i).objectReferenceValue as Checkpoint;
				if (GUILayout.Button("Load Checkpoint " + checkpoint.name))
				{
					CheckpointManager.Instance.LoadCheckpoint(checkpoint);
				}
			}
		}
	}

	private const string CHECKPOINTS_NAME = "m_checkPoints";
}