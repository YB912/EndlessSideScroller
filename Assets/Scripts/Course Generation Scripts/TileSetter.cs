
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mechanics.CourseGeneration
{
    public class TileSetter
    {
        static TilemapParameters _tilemapParameters;
        static TilePaletteReferences _paletteReferences;
        static CourseParameters _courseParameters;
        static bool _staticsSet;

        TilemapController _attachedTilemap;
        byte[,] _occupancyMatrix;

        public TileSetter(TilemapController attachedTilemap, GenerationParameters parameters)
        {
            if (_staticsSet == false)
            {
                _tilemapParameters = parameters.tilemapParameters;
                _paletteReferences = parameters.tilePaletteReferences;
                _courseParameters = parameters.courseParameters;
                _staticsSet = true;
            }

            _attachedTilemap = attachedTilemap;
            _occupancyMatrix = _attachedTilemap.occupancyMatrix;
        }

        public void SetTiles()
        {
            ClearAllTiles();
            GenerateCeiling();
            GenerateAllBlocks();
        }

        public void SetCeilingTilesOnly()
        {
            ClearAllTiles();
            GenerateCeiling(false);
        }

        public void ClearAllTiles()
        {
            _attachedTilemap.tilemap.ClearAllTiles();
            System.Array.Clear(_occupancyMatrix, 0, _occupancyMatrix.Length);
        }

        void GenerateCeiling(bool withGaps = true)
        {
            var origin = new Vector2Int(0, _tilemapParameters.tilemapHeight - 1);
            if (withGaps)
            {
                GenerateCeilingGaps();
            }
            FillBlock(origin, _tilemapParameters.tilemapWidth, 1, _paletteReferences.whiteTile);
        }

        void GenerateCeilingGaps()
        {
            var numberOfGaps = _courseParameters.RandomCeilingGapNumber();
            for (var i = 0; i < numberOfGaps; i++)
            {
                var gapWidth = _courseParameters.RandomCeilingGapWidth();
                var fittingIndexes = OriginXsWhereThisGapFits(gapWidth);
                if (fittingIndexes == null || fittingIndexes.Count == 0) continue;
                var randomX = fittingIndexes[Random.Range(0, fittingIndexes.Count)];
                SetOccupancyMatrixBlock(new Vector2Int(randomX, _tilemapParameters.tilemapHeight - 1), new Vector2Int(gapWidth, 1), 2);
            }
        }

        void GenerateAllBlocks()
        {
            GeneratePlatforms();
            GenerateWalls();
        }

        void GeneratePlatforms()
        {
            var number = _courseParameters.RandomPlatformNumber();
            System.Func<int> randomWidthMethod = _courseParameters.RandomPlatformWidth;
            System.Func<int> randomHeightMethod = _courseParameters.RandomPlatformHeight;
            GenerateBlocks(number, randomWidthMethod, randomHeightMethod);
        }

        void GenerateWalls()
        {
            var number = _courseParameters.RandomWallNumber();
            System.Func<int> randomWidthMethod = _courseParameters.RandomWallWidth;
            System.Func<int> randomHeightMethod = _courseParameters.RandomWallHeight;
            GenerateBlocks(number, randomWidthMethod, randomHeightMethod);
        }

        void GenerateBlocks(int numberOfBlocks, System.Func<int> randomWidthMethod, System.Func<int> randomHeightMethod)
        {
            for (var i = 0; i < numberOfBlocks; i++)
            {
                var blockWidth = randomWidthMethod();
                var blockHeight = randomHeightMethod();
                for (var trial = 0; trial < _courseParameters.maxElementOriginRandomTrials; trial++)
                {
                    var randomOriginX = Random.Range(_courseParameters.minHorizontalDistance, _tilemapParameters.tilemapWidth - blockWidth);
                    var randomOriginY = Random.Range(_courseParameters.minVerticalDistance, _tilemapParameters.tilemapHeight - blockHeight);
                    var randomOrigin = new Vector2Int(randomOriginX, randomOriginY);
                    if (IsSpaceAvailableForBlock(randomOrigin, blockWidth, blockHeight))
                    {
                        FillBlock(randomOrigin, blockWidth, blockHeight, _paletteReferences.whiteTile);
                        break;
                    }
                }
            }
        }

        bool IsSpaceAvailableForBlock(Vector2Int blockOrigin, int blockWidth, int blockHeight)
        {
            var xStart = Mathf.Max(0, blockOrigin.x - _courseParameters.minHorizontalDistance);
            var xEnd = Mathf.Min(blockOrigin.x + blockWidth + _courseParameters.minHorizontalDistance, _tilemapParameters.tilemapWidth);
            var yStart = Mathf.Max(0, blockOrigin.y - _courseParameters.minVerticalDistance);
            var yEnd = Mathf.Min(blockOrigin.y + blockHeight + _courseParameters.minVerticalDistance, _tilemapParameters.tilemapHeight);

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    if (IsCellEmpty(x, y) == false) return false;
                }
            }
            return true;
        }

        void FillBlock(Vector2Int blockOrigin, int width, int height, TileBase fillTile)
        {
            
            for (int y = blockOrigin.y; y < blockOrigin.y + height; y++)
            {
                for (int x = blockOrigin.x; x < blockOrigin.x + width; x++)
                {
                    if (_occupancyMatrix[x, y] == 2)
                    {
                        continue;
                    }
                    _attachedTilemap.tilemap.SetTile(new Vector3Int(x, y, 0), fillTile);
                    SetOccupancyMatrixCell(new Vector2Int(x, y), 1);
                }
            }
        }

        bool IsCellEmpty(int x, int y)
        {
            if (x < 0 || _occupancyMatrix.GetLength(0) <= x) return false;
            if (y < 0 || _occupancyMatrix.GetLength(1) <= y) return false;
            if (_occupancyMatrix[x, y] != 0) return false;
            return true;
        }

        List<int> OriginXsWhereThisGapFits(int gapWidth)
        {
            var output = new List<int>();
            for (var originX = _courseParameters.minHorizontalDistance; originX < _tilemapParameters.tilemapWidth - (gapWidth - 1); originX++)
            {
                var allCellsEmpty = true;
                for (var gapCellX = originX; gapCellX < originX + gapWidth; gapCellX++)
                {
                    if (IsCellEmpty(gapCellX, 0) == false)
                    {
                        originX = gapCellX;
                        allCellsEmpty = false;
                        break;
                    }
                }
                if (allCellsEmpty) output.Add(originX);
            }
            return output;
        }

        void SetOccupancyMatrixBlock(Vector2Int origin, Vector2Int size, byte code)
        {
            for (int x = origin.x; x < origin.x + size.x; x++)
            {
                for (int y = origin.y; y < origin.y + size.y; y++)
                {
                    SetOccupancyMatrixCell(new Vector2Int(x, y), code);
                }
            }
        }

        void SetOccupancyMatrixCell(Vector2Int cell, byte code)
        {
            if (IsCellOutOfBounds(cell)) return;
            _occupancyMatrix[cell.x, cell.y] = code;
        }

        bool IsCellOutOfBounds(Vector2Int cell)
        {
            var output = 
                (cell.x < 0 || cell.x >= _occupancyMatrix.GetLength(0)) ||
                (cell.y < 0 || cell.y >= _occupancyMatrix.GetLength(1));
            return output;
        }
    }
}
