using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
    [CustomGridBrush(false, false, true, "Frame Correction Brush")]
    public class FrameCorrectionBrush : GridBrush
    {
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            base.Paint(grid, brushTarget, position);

            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;
            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            if (tilemap == null) return;

            //Find suitable Frame
            Vector3Int suitablePosition;
            Tilemap suitableTilemap = FindSuitableFrame(tilemap, position, out suitablePosition);
            if (suitableTilemap == null) return;

            //Get tile and set to suitableTilemap
            TileBase tile = tilemap.GetTile(position);
            tilemap.SetTile(position, null);
            suitableTilemap.SetTile(suitablePosition, tile);
        }

        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;
            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            if (tilemap == null) return;

            //Find suitable Frame
            Vector3Int suitablePosition;
            Tilemap suitableTilemap = FindSuitableFrame(tilemap, position, out suitablePosition);
            if (suitableTilemap == null || suitableTilemap.GetTile(suitablePosition) == null)
            {
                base.Erase(gridLayout, brushTarget, position);
                return;
            }
            suitableTilemap.SetTile(suitablePosition, null);
        }

        private static Tilemap FindSuitableFrame(Tilemap baseTileMap, Vector3Int position, out Vector3Int newPosition)
        {
            Frame[] frames = FindObjectsOfType<Frame>();
            for (int i = 0; i < frames.Length; i++)
            {
                Rect frameRect = frames[i].GetComponent<RectTransform>().rect;
                frameRect.center = frames[i].transform.position;
                if (frameRect.Contains(baseTileMap.transform.position + new Vector3(position.x, position.y, position.z)))
                {
                    //Find suitable position
                    Tilemap platformTilemap = frames[i].GetComponentsInChildren<Tilemap>().Where(x => x.GetComponent<Rigidbody2D>() != null).ToArray()[0]; //Platform
                    Vector3 offset = platformTilemap.transform.position - baseTileMap.transform.position;
                    //Set tile map 
                    newPosition = position - new Vector3Int((int)offset.x, (int)offset.y, (int)offset.z);

                    return platformTilemap;
                }
            }

            newPosition = Vector3Int.zero;              
            return null;
        }
    }


    [CustomEditor(typeof(FrameCorrectionBrush))]
    public class FrameCorrectionBrushEditor: GridBrushEditorBase
    {
        public override GameObject[] validTargets
        {
            get
            {
                return GameObject.FindObjectsOfType<Tilemap>().Select(x => x.gameObject).ToArray();
            }
        }
    }
}
