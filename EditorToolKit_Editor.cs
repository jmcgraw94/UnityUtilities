#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//Shortcut Commands: https://docs.unity3d.com/ScriptReference/MenuItem.html

/// <summary>
/// A collection of shortcut complete editor tools
/// </summary>
public class EditorToolKit_Editor : ScriptableObject {

    [MenuItem ("Utilities/Deselect All &#d")]
    static void DoDeselect () {
        Selection.objects = new UnityEngine.Object [0];
    }

    [MenuItem ("Utilities/Toggle Active %e")]
    static void ToggleActive () {
        foreach (GameObject CurSel in Selection.objects) {
            CurSel.SetActive (!CurSel.activeSelf);
        }
    }

    #region Transforms

    [MenuItem ("Utilities/Snap Down &#s")]
    static void SnapToGround () {
        RaycastHit Hit;

        foreach (GameObject CurSel in Selection.objects)
            if (Physics.Raycast (CurSel.transform.position, Vector3.down, out Hit)) {
                if (Hit.collider != null) {
                    CurSel.transform.position = Hit.point;
                }
            }
    }

    [MenuItem ("Utilities/Reset Transfrom &#r")]
    static void ResetTransform () {
        foreach (GameObject CurSel in Selection.objects) {
            CurSel.transform.localEulerAngles = Vector3.zero;
            CurSel.transform.localPosition = Vector3.zero;
            CurSel.transform.localScale = Vector3.one;
        }
    }

    #region KeyboardMovement

    

    static void Move (Vector3 Displacement) {
        float magnitude = Displacement.magnitude;
        if (Tools.pivotRotation == PivotRotation.Global) {
            foreach (GameObject CurSel in Selection.objects) {
                CurSel.transform.position += magnitude * NearestWorldAxis (Displacement);
            }
        }
        else {
            foreach (GameObject CurSel in Selection.objects) {
                CurSel.transform.position += magnitude * NearestLocalAxis (Displacement, CurSel.transform);
            }
        }
    }

    static Vector3 NearestWorldAxis (Vector3 input) {
        return NearestAxis (input, new Vector3 [] { Vector3.right, Vector3.up, Vector3.forward });
    }
    static Vector3 NearestLocalAxis (Vector3 input, Transform trans) {
        return NearestAxis (input, new Vector3 [] { trans.right, trans.up, trans.forward });
    }

    static Vector3 NearestAxis (Vector3 input, Vector3[] basis) {
        Vector3 nearest = Vector3.right;
        float dotX = Vector3.Dot (input, basis[0]);
        float dotY = Vector3.Dot (input, basis[1]);
        float dotZ = Vector3.Dot (input, basis[2]);
        float absX = Mathf.Abs (dotX);
        float absY = Mathf.Abs (dotY);
        float absZ = Mathf.Abs (dotZ);

        if (absX > absY && absX > absZ) {
            float sign = dotX / absX;
            nearest = sign * basis[0];
        }
        else if (absY > absX && absY > absZ) {
            float sign = dotY / absY;
            nearest = sign * basis[1];
        }
        else {
            float sign = dotZ / absZ;
            nearest = sign * basis[2];
        }
        return nearest;
    }

    static float nudgeDistance = 0.1f;

    [MenuItem ("Utilities/Move/Nudge Left &LEFT")]
    static void NudgeLeft () {
        if (Camera.current) {
            Move (-nudgeDistance * Camera.current.transform.right);
        }
    }

    [MenuItem ("Utilities/Move/Nudge Right &RIGHT")]
    static void NudgeRight () {
        if (Camera.current) {
            if (Camera.current) {
                Move (nudgeDistance * Camera.current.transform.right);
            }
        }
    }

    [MenuItem ("Utilities/Move/Nudge Forward &UP")]
    static void NudgeForward () {
        if (Camera.current) {
            Move (nudgeDistance * Camera.current.transform.forward);
        }
    }

    [MenuItem ("Utilities/Move/Nudge Backward &DOWN")]
    static void NudgeBackward () {
        if (Camera.current) {
            Move (-nudgeDistance * Camera.current.transform.forward);
        }
    }

    static float shoveDistance = 1f;

    [MenuItem ("Utilities/Move/Shove Left &#LEFT")]
    static void ShoveLeft () {
        if (Camera.current) {
            Move (-shoveDistance * Camera.current.transform.right);
        }
    }

    [MenuItem ("Utilities/Move/Shove Right &#RIGHT")]
    static void ShoveRight () {
        if (Camera.current) {
            if (Camera.current) {
                Move (shoveDistance * Camera.current.transform.right);
            }
        }
    }

    [MenuItem ("Utilities/Move/Shove Forward &#UP")]
    static void ShoveForward () {
        if (Camera.current) {
            Move (shoveDistance * Camera.current.transform.forward);
        }
    }

    [MenuItem ("Utilities/Move/Shove Backward &#DOWN")]
    static void ShoveBackward () {
        if (Camera.current) {
            Move (-shoveDistance * Camera.current.transform.forward);
        }
    }

    #endregion KeyboardMovement

    #endregion Transforms

    #region CustomCreation

    static GameObject CreateEmptyGameObject (MenuCommand menuCommand) {
        GameObject go = new GameObject ("New GameObject");
        GameObjectUtility.SetParentAndAlign (go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
        Selection.activeObject = go;
        return go;
    }

    [MenuItem ("GameObject/Triggers/Trigger Zone (Box)", false, 10)]
    static void CreateTriggerBox (MenuCommand menuCommand) {
        GameObject trigger = CreateEmptyGameObject (menuCommand);
        BoxCollider col = trigger.AddComponent<BoxCollider> ();
        col.isTrigger = true;
    }

    [MenuItem ("GameObject/Triggers/Trigger Zone (Capsule)", false, 10)]
    static void CreateTriggerCapsule (MenuCommand menuCommand) {
        GameObject trigger = CreateEmptyGameObject (menuCommand);
        CapsuleCollider col = trigger.AddComponent<CapsuleCollider> ();
        col.isTrigger = true;
    }

    #endregion CustomCreation

}

/// <summary>
/// Apply a material to all children of this script's gameobject
/// </summary>
[ExecuteInEditMode]
public class RecursiveMaterialApply_Editor : MonoBehaviour {

    public Material Material;
    public bool Apply = false;

    void Update () {
        if (Apply) {
            Apply = false;
            foreach (MeshRenderer MR in GetComponentsInChildren<MeshRenderer> ()) {
                MR.material = Material;
            }
        }
    }
}

[ExecuteInEditMode]
public class AutoSnap_Script : MonoBehaviour {

    public float snapValueX = 1f;
    public float snapValueY = 1f;
    public float snapValueZ = 1f;


    void Update () {
        if (Application.isPlaying)
            return;

        if (snapValueX != 0)
            transform.position = new Vector3 (Mathf.Round (transform.position.x * (1 / snapValueX)) / (1 / snapValueX), transform.position.y, transform.position.z);

        if (snapValueY != 0)
            transform.position = new Vector3 (transform.position.x, Mathf.Round (transform.position.y * (1 / snapValueY)) / (1 / snapValueY), transform.position.z);

        if (snapValueZ != 0)
            transform.position = new Vector3 (transform.position.x, transform.position.y, Mathf.Round (transform.position.z * (1 / snapValueZ)) / (1 / snapValueZ));
    }
}

/// <summary>
/// Perform a naming find and replace operation on the hierarchy
/// </summary>
[ExecuteInEditMode]
public class FindAndReplace_Editor : MonoBehaviour {

    public string Find = "";
    public string ReplaceWith = "";

    string LastFind = "";
    string LastReplace = "";

    [Header ("Find and Replace")]
    public bool Replace = false;
    public bool Undo = false;

    void Start () {

    }

    void FindAndReplace (string f, string r, bool UpdateLast = true) {
        Replace = false;

        if (Find.ToLower ().Equals (ReplaceWith.ToLower ())) {
            Debug.LogWarning ("Rename: No Change");
            return;
        }

        if (UpdateLast) {
            LastFind = f;
            LastReplace = r;
        }

        foreach (GameObject G in FindObjectsOfType<GameObject> ()) {
            if (G.name.Contains (f)) {
                G.name = G.name.Replace (f, r);
            }
        }
    }


    void Update () {

        if (Undo) {
            Undo = false;

            FindAndReplace (LastReplace, LastFind, false);
        }

        if (Replace)
            FindAndReplace (Find, ReplaceWith);
    }
}
#endif