using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
    [CreateAssetMenu(fileName = "Gem Prefab brush", menuName = "Brushes/Gem Prefab brush")]
    [CustomGridBrush(false, true, false, "Gem Prefab Brush")]
    public class GemFrameCorrectionBrush : GridBrush
    {
        private const float k_PerlinOffset = 100000f;
        public GameObject[] m_Prefabs;
        public float m_PerlinScale = 0.5f;
        public int m_Z;
        private GameObject prev_brushTarget;
        private Vector3Int prev_position;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            if (position == prev_position)
            {
                return;
            }
            prev_position = position;
            if (brushTarget)
            {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset) * m_Prefabs.Length), 0, m_Prefabs.Length - 1);
            GameObject prefab = m_Prefabs[index];
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (instance != null)
            {
                Tilemap newTileMap = FindSuitableFrame(brushTarget.GetComponent<Tilemap>(), position);
                if (newTileMap == null) return;

                Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
                Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
                instance.transform.SetParent(newTileMap.transform);
                instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, 0) + new Vector3(.5f, .5f, .5f))) + Vector3.forward * m_Z;
            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            if (brushTarget)
            {
                prev_brushTarget = brushTarget;
            }
            brushTarget = prev_brushTarget;
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            if (tilemap == null) return;

            //Find suitable Frame
            Transform erased = null;
            Tilemap suitableTilemap = FindSuitableFrame(tilemap, position);

            erased = GetObjectInCell(grid, suitableTilemap.transform, new Vector3Int(position.x, position.y, 0), m_Z);
            if (erased != null)
                Undo.DestroyObjectImmediate(erased.gameObject);
        }

        private static Tilemap FindSuitableFrame(Tilemap baseTileMap, Vector3Int position)
        {
            Frame[] frames = FindObjectsOfType<Frame>();
            for (int i = 0; i < frames.Length; i++)
            {
                Rect frameRect = frames[i].GetComponent<RectTransform>().rect;
                frameRect.center = frames[i].transform.position;
                if (frameRect.Contains(baseTileMap.transform.position + new Vector3(position.x, position.y, position.z)))
                {
                    //Find suitable position
                    Tilemap platformTilemap = frames[i].GetComponentsInChildren<Tilemap>().Where(x => x.GetComponent<Rigidbody2D>() != null).ToArray()[0];
                    Vector3 offset = platformTilemap.transform.position - baseTileMap.transform.position;


                    return platformTilemap;
                }
            }

            return null;
        }

        private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position, int z)
        {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position)) + Vector3.forward * z;
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one)) + Vector3.forward * z;
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position))
                    return child;
            }
            return null;
        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset)
        {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }
    }

    [CustomEditor(typeof(GemFrameCorrectionBrush))]
    public class GemFrameCorrectionBrushEditor : GridBrushEditor
    {
        private GemFrameCorrectionBrush prefabBrush { get { return target as GemFrameCorrectionBrush; } }

        private SerializedProperty m_Prefabs;
        private SerializedObject m_SerializedObject;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SerializedObject = new SerializedObject(target);
            m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
        }

        public override void OnPaintInspectorGUI()
        {
            m_SerializedObject.UpdateIfRequiredOrScript();
            prefabBrush.m_PerlinScale = EditorGUILayout.Slider("Perlin Scale", prefabBrush.m_PerlinScale, 0.001f, 0.999f);
            prefabBrush.m_Z = EditorGUILayout.IntField("Position Z", prefabBrush.m_Z);

            EditorGUILayout.PropertyField(m_Prefabs, true);
            m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
