﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace CrowsBedroom.OneStroke
{
    public class MapDict
    {
        Tilemap _tilemap = null;
        Dictionary<Vector3Int, Cell> _mapDict = null;

        public MapDict(Tilemap tilemap)
        {
            _tilemap = tilemap;
            _mapDict = new Dictionary<Vector3Int, Cell>();
        }

        public void CreateDict()
        {
            // Optimize bounds to use it easily.
            _tilemap.CompressBounds();

            foreach (var pos in _tilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = _tilemap.GetTile(pos);

                if (tile == null)
                {
                    continue;
                }
                // Try to get type from the tile.
                // If succeeded, add Cell to the dict.
                if (TryParse(tile.name, out CellType type))
                {
                    _mapDict.Add(pos, CellFactory.GetInstance(type));
                }
            }
        }

        /// <summary>
        /// Try to get CellType from name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns>Whether it succeeded.</returns>
        bool TryParse(string name, out CellType type)
        {
            return Enum.TryParse(name, out type) && Enum.IsDefined(typeof(CellType), type);
        }

        public Cell GetCell(Vector3Int pos)
        {
            return _mapDict.ContainsKey(pos) ? _mapDict[pos] : null;
        }

        public bool SetCell(Vector3Int pos, Cell cell)
        {
            if(!_mapDict.ContainsKey(pos))
            {
                return false;
            }
             _mapDict[pos] = cell;
            return true;
        }

        public bool IsStageClear()
        {
            foreach (var cell in _mapDict)
            {
                if (cell.Value.Type == CellType.Road)
                {
                    return false;
                }
            }
            return true;
        }

        public void Log()
        {
            foreach (var cell in _mapDict)
            {
                Debug.Log(cell.Key + cell.Value.Type.ToString());
            }
        }
    }
}
