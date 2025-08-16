
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mechanics.CourseGeneration
{
    public class TileSetter
    {

        static TilemapParameters _tilemapParameters;
        static TilePaletteReferences _paletteReferences;
        static CourseParameters _courseParameters;
        static List<Vector2Int> _defaultRelevantCeilingCells;
        static bool _staticsSet;

        TilemapController _attachedTilemap;

        public TileSetter(TilemapController attachedTilemap, GenerationParameters parameters)
        {
            if (_staticsSet == false)
            {
                _tilemapParameters = parameters.tilemapParameters;
                _paletteReferences = parameters.tilePaletteReferences;
                _courseParameters = parameters.courseParameters;
                SetDefaultCeilingRelevantCells();
                _staticsSet = true;
            }

            _attachedTilemap = attachedTilemap;
        }

        static void SetDefaultCeilingRelevantCells()
        {
            _defaultRelevantCeilingCells = new List<Vector2Int>();
            for (int j = _courseParameters.minHorizontalDistance; j < _tilemapParameters.tilemapWidth; j++)
            {
                _defaultRelevantCeilingCells.Add(new Vector2Int(j, 0));
            }
        }

        public void SetTiles()
        {
            ClearAllTiles();
            GenerateCeiling();
        }

        public void ClearAllTiles()
        {
            _attachedTilemap.tilemap.ClearAllTiles();
        }

        void GenerateCeiling()
        {
            var origin = new Vector2Int(0, _tilemapParameters.tilemapHeight - 1);
            var gaps = CeilingGapIndexes();
            FillBox(origin, _tilemapParameters.tilemapWidth, 1, _paletteReferences.whiteTile, gaps);
        }

        List<int> CeilingGapIndexes()
        {
            var output = new List<int>();
            var numberOfGaps = Random.Range(_courseParameters.minCeilingGapNumberPerMap, _courseParameters.maxCeilingGapNumberPerMap + 1);
            var relevantCells = new List<Vector2Int>(_defaultRelevantCeilingCells);
            for (var i = 0; i < numberOfGaps; i++)
            {
                var gapLength = Random.Range(_courseParameters.minCeilingGapLength, _courseParameters.maxCeilingGapLength + 1);
                var fittingIndexes = ColumnIndexesWhereThisGapFits(relevantCells, gapLength);
                if (fittingIndexes == null || fittingIndexes.Count == 0) continue;
                var randomX = fittingIndexes[Random.Range(0, fittingIndexes.Count)];
                for (var offset = 0; offset < gapLength; offset++)
                {
                    output.Add(randomX + offset);
                    relevantCells = relevantCells.Where(cell => output.Contains(cell.x) == false).ToList();
                }
            }
            return output;
        }

        void FillBox(Vector2Int origin, int width, int height, TileBase fillTile, List<int> exceptionCells = null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (exceptionCells != null && exceptionCells.Contains(x)) continue;

                    Vector3Int cellPosition = new Vector3Int(origin.x + x, origin.y + y, 0);
                    _attachedTilemap.tilemap.SetTile(cellPosition, fillTile);
                }
            }
        }

        bool IsBlockAvailable(Vector2Int origin, int width, int height)
        {
            width = width + _courseParameters.minHorizontalDistance;
            height = height + _courseParameters.minVerticalDistance;
            origin = new Vector2Int(origin.x - _courseParameters.minHorizontalDistance, origin.y - _courseParameters.minVerticalDistance);
            for (int i = origin.y; i < height; i++)
            {
                for (int j = origin.x; j < width; j++)
                {
                    if (IsCellEmpty(i, j) == false) return false;
                }
            }
            return true;
        }

        List<Vector2Int> EmptyCells()
        {
            var output = new List<Vector2Int>();
            for (int i = 0; i < _attachedTilemap.occupancyMatrix.Length; i++)
            {
                for (int j = 0; j < _attachedTilemap.occupancyMatrix[0].Length; j++)
                {
                    if (IsCellEmpty(i, j)) { output.Add(new Vector2Int(i, j)); }
                }
            }
            return output;
        }

        bool IsCellEmpty(int x, int y)
        {
            if (_attachedTilemap.occupancyMatrix[x][y] != 0) return false;
            return true;
        }

        List<int> ColumnIndexesWhereThisGapFits(List<Vector2Int> relevantCells, int gapLength)
        {
            return relevantCells.
                Where(cell => cell.x < _tilemapParameters.tilemapWidth - (gapLength - 1)).
                Select(cell => cell.x).
                ToList();
        }
    }
}
