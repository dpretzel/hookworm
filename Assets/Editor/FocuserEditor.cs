using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Focuser)), CanEditMultipleObjects]
public class FocuserEditor : Editor
{

    [SerializeField]
    public GameObject previewPlayerPrefab;
    public Sprite previewPlayerSprite;
    private Vector3 startPos;
    public Focuser focuser;
    public GameObject camGO;
    public Camera previewCam;
    public GameObject previewPlayer;


    void OnEnable()
    {
        Camera mainCam = Camera.main; // get a reference now so even if the new Camera gets elected to be main, we have a reference the actual Camera we want to be main
        focuser = (Focuser)target;

        camGO = EditorUtility.CreateGameObjectWithHideFlags("z!!!!!!!!!!!!!!SHOULDN'T STILL BE IN SCENE!!!!!!!!!!!!!!!!!", HideFlags.HideAndDontSave);
        previewCam = camGO.AddComponent<Camera>();
        previewCam.CopyFrom(mainCam);
        previewCam.tag = "Untagged";

        previewPlayer = EditorUtility.CreateGameObjectWithHideFlags("z!!!!!!!!!!!!!!SHOULDN'T STILL BE IN SCENE!!!!!!!!!!!!!!!!!", HideFlags.HideAndDontSave);
        SpriteRenderer s = previewPlayer.AddComponent<SpriteRenderer>();
        s.sprite = previewPlayerSprite;
        startPos = focuser.transform.position - Vector3.down * 10;
    }

    private void OnDisable()
    {
        DestroyImmediate(camGO);
        DestroyImmediate(previewPlayer);
    }


    public override void OnInspectorGUI()
    {
        Rect previewRect = EditorGUILayout.GetControlRect(new GUILayoutOption[] { GUILayout.Height(Camera.main.pixelRect.height * EditorGUIUtility.currentViewWidth / Camera.main.pixelRect.width) });

        focuser.attachedTo = (Transform)EditorGUILayout.ObjectField("Attached To", focuser.attachedTo, typeof(Transform), true);
        EditorUtility.SetDirty(focuser);
        Undo.RecordObject(focuser, "Set AttachedTo");
        focuser.zoom = EditorGUILayout.FloatField("Zoom", focuser.zoom);
        focuser.playerFocus = EditorGUILayout.Slider("Player Focus", focuser.playerFocus, 0, 1);
        focuser.snappiness = EditorGUILayout.Slider("Snappiness", focuser.snappiness, 0, 1);

        previewCam.transform.position = focuser.CalculateResultingPosition(previewPlayer.transform.position);
        previewCam.orthographicSize = focuser.zoom;

        Handles.DrawCamera(previewRect, previewCam);
        EditorGUIUtility.AddCursorRect(previewRect, MouseCursor.Pan);

        Undo.RecordObject(focuser, "Modified Focuser settings");

        float m = 5;
        previewPlayer.transform.position = new Vector3(startPos.x + Mathf.Sin(Time.realtimeSinceStartup) * 5, startPos.y + Mathf.Cos(Time.realtimeSinceStartup) * 5, startPos.z);

        //Handles.ArrowHandleCap(0, previewCam.transform.position, previewCam.transform.rotation, 3, EventType.Repaint);

        //EditorGUIUtility.GUIToScreenPoint
        //EditorGUIUtility.GetFlowLayoutedRects
        //EditorGUIUtility.PingObject
        //EditorGUIUtility.ShowObjectPicker

        // update the preview whenever variable in RowMaker is changed
        //rowMaker.Preview();

    }
}
