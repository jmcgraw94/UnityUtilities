#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//Shortcut Commands: https://docs.unity3d.com/ScriptReference/MenuItem.html

public class EditorToolKit_Editor : ScriptableObject {
//A collection of shortcut complete editor tools
	
    [MenuItem("Utilities/Deselect All &#d")]
    static void DoDeselect() {
        Selection.objects = new UnityEngine.Object[0];
    }

    [MenuItem("Utilities/Snap Down &#s")]
    static void SnapToGround() {
        RaycastHit Hit;

        foreach (GameObject CurSel in Selection.objects)
            if (Physics.Raycast(CurSel.transform.position, Vector3.down, out Hit)) {
                if (Hit.collider != null) {
					CurSel.transform.position = Hit.point;
                }
            }
    }
	
	[MenuItem("Utilities/Toggle Active %e")]
    static void ToggleActive() {
        foreach (GameObject CurSel in Selection.objects) {
            CurSel.SetActive(!CurSel.activeSelf);
        }
    }
}

[ExecuteInEditMode]
public class RecursiveMaterialApply_Editor : MonoBehaviour {
//Apply a material to all children of this script's gameobject

    public Material Material;
    public bool Apply = false;

    void Update() {
        if (Apply) {
            Apply = false;
            foreach (MeshRenderer MR in GetComponentsInChildren<MeshRenderer>()) {
                MR.material = Material;
            }
        }
    }
}

[ExecuteInEditMode]
public class FindAndReplace_Editor : MonoBehaviour {
//Perform a naming find and replace operation on the hierarchy

    public string Find = "";
    public string ReplaceWith = "";

    string LastFind = "";
    string LastReplace = "";

    [Header("Find and Replace")]
    public bool Replace = false;
    public bool Undo = false;

    void Start() {

    }

    void FindAndReplace(string f, string r, bool UpdateLast = true) {
        Replace = false;

        if (Find.ToLower().Equals(ReplaceWith.ToLower())) {
            Debug.LogWarning("Rename: No Change");
            return;
        }

        if (UpdateLast) {
            LastFind = f;
            LastReplace = r;
        }

        foreach (GameObject G in FindObjectsOfType<GameObject>()) {
            if (G.name.Contains(f)) {
                G.name = G.name.Replace(f, r);
            }
        }
    }


    void Update() {

        if (Undo) {
            Undo = false;

            FindAndReplace(LastReplace, LastFind, false);
        }

        if (Replace)
            FindAndReplace(Find, ReplaceWith);
    }
}
#endif