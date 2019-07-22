using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;
using Mg.Wy;

namespace Mg.Wy
{
    public class CreateNetSyncObj : EditorWindow
    {

        #region Private Field
        private static string Path = "Assets/Resources/Prefabs/NetSync/";
        #endregion

        #region Editor Extends
        [MenuItem("Window/NetSync")]
        static void CreateNetSync()
        {
            Rect wr = new Rect(0, 0, 300, 80);
            CreateNetSyncObj window = EditorWindow.GetWindowWithRect(typeof(CreateNetSyncObj), wr, true, "Create NetSync Obj") as CreateNetSyncObj;
            window.Show();
        }

        [MenuItem("Assets/TransFormNetSync")]
        static void TransformNetSync()
        {
            GameObject obj = Selection.activeObject as GameObject;
            if (obj == null) return;
            CreateTransformSyncObj(ref obj);
            SaveTheSyncObj(ref obj);
        }

        [MenuItem("Assets/AnimatorNetSync")]
        static void AnimatorNetSync()
        {
            GameObject obj = Selection.activeObject as GameObject;
            if (obj == null) return;
            CreateAnimatorSyncObj(ref obj);
            SaveTheSyncObj(ref obj);
        }


        #endregion

        #region Mono Callbacks

        private GameObject sourceObj;
        private Animator sourceAnimator;
        private void OnGUI()
        {
            sourceObj = EditorGUILayout.ObjectField("Select the source obj", sourceObj, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Make It as a Transform NetSync Object", GUILayout.Width(300)))
            {
                CreateTransformSyncObj(ref sourceObj);
                SaveTheSyncObj(ref sourceObj);
            }

            if (GUILayout.Button("Make It as a Animator NetSync Object", GUILayout.Width(300)))
            {
                CreateAnimatorSyncObj(ref sourceObj);
                SaveTheSyncObj(ref sourceObj);
            }
        }

        #endregion

        #region Private Functions
        private static void CreateTransformSyncObj(ref GameObject sourceObj)
        {
            var pv = sourceObj.AddComponent<PhotonView>();
            pv.Synchronization = ViewSynchronization.UnreliableOnChange;
            var ptv = sourceObj.AddComponent<PhotonTransformView>();
            ptv.m_SynchronizePosition = true;
            ptv.m_SynchronizeRotation = true;
            ptv.m_SynchronizeScale = true;
            pv.ObservedComponents = new List<Component>();
            pv.ObservedComponents.Add(ptv);
        }

        private static void CreateAnimatorSyncObj(ref GameObject sourceObj)
        {
            CreateTransformSyncObj(ref sourceObj);
            var pv = sourceObj.GetComponent<PhotonView>();

            var pav = sourceObj.AddComponent<PhotonAnimatorView>();
            var paramList = pav.GetSynchronizedParameters();
            foreach (var obj in paramList)
            {
                obj.SynchronizeType = PhotonAnimatorView.SynchronizeType.Discrete;
            }
            pv.ObservedComponents.Add(pav);
        }

        private static void SaveTheSyncObj(ref GameObject sourceObj)
        {
            GameObject obj = PrefabUtility.SaveAsPrefabAsset(sourceObj, Path + sourceObj.name + ".prefab");
            AddToScriptableObj(obj);
            DestroyImmediate(sourceObj);
        }

        private static void AddToScriptableObj(GameObject obj)
        {
            StaticObj data = Resources.Load<StaticObj>("Scriptable/StaticObjData");
            data.staticObjs.Add(obj);
        }
        #endregion
    }

}