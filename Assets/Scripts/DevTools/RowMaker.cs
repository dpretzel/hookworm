using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

// this class duplicates a prefab in a row, useful for rows of hooks that need to be evenly spaced
[ExecuteAlways]
[System.Serializable]
public class RowMaker : MonoBehaviour
{
    public GameObject prefab;
    public int copies = 2;
    public float spacing = 5;
    public float angle = 0;
    public bool rotateObjects = false;
    public float twist = 0;

    [SerializeField]
    private List<GameObject> copyList;

    // effectively locks copies in place, restricting the level designer to only modify their positions by adjusting the RowMaker script
    private void Update()
    {
        if (!Application.isPlaying)
            Preview();
    }

    // add/delete a single copy per frame (approaching copies specified in RowMaker) and update all copies' positions
    public void Preview()
    {
        if (copyList == null)
            Clean();
        UpdateCopies();
        UpdatePositions();
    }

    // delete or add copies 1 per frame until there we have have the right amount of copies
    public void UpdateCopies()
    {
        int oldCopies = copyList.Count;
        if (copies > oldCopies)
            AddCopy();
        else if (copies < oldCopies)
            RemoveCopy();
    }

    public void UpdatePositions()
    {
        for (int i = 0; i < copyList.Count; i++)
        {
            float deg = angle * Mathf.Deg2Rad;
            float x = (spacing * i) * Mathf.Cos(deg);
            float y = (spacing * i) * Mathf.Sin(deg);

            //twist
            //x *= Mathf.Cos(twist * i);
            //y *= Mathf.Sin(twist * i);

            copyList[i].transform.position = transform.position + new Vector3(x, y);
            if(rotateObjects)
                copyList[i].transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // destroy the copies created during the last edit session (copyList's ref to them was lost, so copyList doesn't know these copies already exist and will create more than desired)
    private void WipeStrayCopies()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject copy = transform.GetChild(i).gameObject;
            if (!copyList.Contains(copy))
            {
                print("wipe");
                DestroyImmediate(copy);
            }
        }
    }

    // if there are copies leftover from last time that copyList lost reference to, add them to copyList instead of deleting them and making more
    private void RecoverStrayCopies()
    {
        for (int i = 0; i < copies && i < transform.childCount; i++)
        {
            GameObject copy = transform.GetChild(i).gameObject;
            if (!copyList.Contains(copy))
            {
                print("recover!");
                AddCopy(copy);
            }
        }

    }

    // destroy the preview created during the last edit session and add fresh copies in
    public void Clean()
    {
        print("clean!");
        copyList = new List<GameObject>();

        RecoverStrayCopies();
        WipeStrayCopies();

        for (int i = transform.childCount; i < copies; i++)
            AddCopy();
    }

    // remove known copy from copyList and delete it
    private void RemoveCopy()
    {
        GameObject toDestroy = copyList[copyList.Count - 1];
        copyList.Remove(toDestroy);
        DestroyImmediate(toDestroy);
    }

    // create copy and add to copyList
    private void AddCopy()
    {
        /*
        string path = AssetDatabase.GetAssetPath(prefab);
        //remove "Assets/Resources/" and ".prefab" from the path
        path = path.Replace("Assets/Resources/", "");
        path = path.Replace(".prefab", "");
        print(path);

        GameObject copy = Instantiate(Resources.Load(path, typeof(GameObject)), transform) as GameObject;
        //GameObject copy = GameObject.Instantiate(Resources.Load("Hook", typeof(GameObject)), transform) as GameObject;
        */
        GameObject copy = GameObject.Instantiate(prefab, transform);
        AddCopy(copy);
    }

    private void AddCopy(GameObject copy)
    {
        copyList.Add(copy);
    }
}
