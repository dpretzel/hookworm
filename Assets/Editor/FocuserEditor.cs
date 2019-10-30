using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Focuser)), CanEditMultipleObjects]
public class FocuserEditor : Editor
{

    [SerializeField]
    public Focuser focuser;
    public GameObject camGO;
    public Camera cam;

    void OnEnable()
    {
        Camera mainCam = Camera.main; // get a reference now so even if the new Camera gets elected to be main, we have a reference the actual Camera we want to be main
        focuser = (Focuser)target;

        camGO = EditorUtility.CreateGameObjectWithHideFlags("z!!!!!!!!!!!!!!SHOULDN'T STILL BE IN SCENE!!!!!!!!!!!!!!!!!", HideFlags.HideAndDontSave);
        cam = camGO.AddComponent<Camera>();
        cam.CopyFrom(mainCam);
        cam.tag = "Untagged";
    }

    private void OnDisable()
    {
        DestroyImmediate(camGO);
    }


    public override void OnInspectorGUI()
    {
        Rect previewRect = EditorGUILayout.GetControlRect(new GUILayoutOption[] { GUILayout.Height(Camera.main.pixelRect.height * EditorGUIUtility.currentViewWidth / Camera.main.pixelRect.width) });

        focuser.zoom = EditorGUILayout.FloatField("Zoom", focuser.zoom);
        focuser.playerFocus = EditorGUILayout.Slider("Player Focus", focuser.playerFocus, 0, 1);
        focuser.snappiness = EditorGUILayout.Slider("Snappiness", focuser.snappiness, 0, 1);

        cam.transform.position = focuser.transform.position + Vector3.back * 16;
        cam.orthographicSize = focuser.zoom;

        Handles.DrawCamera(previewRect, cam);
        EditorGUIUtility.AddCursorRect(previewRect, MouseCursor.Pan);

        Undo.RecordObject(focuser, "Modified Focuser settings");

        //Handles.ArrowHandleCap(0, cam.transform.position, cam.transform.rotation, 3, EventType.Repaint);

        //EditorGUIUtility.GUIToScreenPoint
        //EditorGUIUtility.GetFlowLayoutedRects
        //EditorGUIUtility.PingObject
        //EditorGUIUtility.ShowObjectPicker

        // update the preview whenever variable in RowMaker is changed
        //rowMaker.Preview();

    }
}
