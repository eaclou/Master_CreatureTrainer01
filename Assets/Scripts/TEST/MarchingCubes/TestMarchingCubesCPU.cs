﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestMarchingCubesCPU : MonoBehaviour {

    public int resolution = 8;
    public float cellSize = 0.5f;

    // For each Voxel of the chunk grid:
    private float cap = 0.2f;   // density threshold to determine if a vert of the cube is on a density boundary

    #region tables
    
    //string[,] strArr = new string[2, 3];
    //new int[,] {{0, 0, 1}, {1, 0, 0}},
    private int[,] edge_to_verts = {
        { 0,1 }, //0
        { 1,2 }, //1
        { 2,3 }, //2
        { 3,0 }, //3
        { 4,5 }, //4
        { 5,6 }, //5
        { 6,7 }, //6
        { 7,4 }, //7
        { 4,0 }, //8
        { 5,1 }, //9
        { 6,2 }, //10
        { 7,3 } //11
	};
    private int[] case_to_numpolys = {
        0,1,1,2,1,2,2,3,1,2,2,3,2,3,3,2,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,3,
        1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,3,2,3,3,2,3,4,4,3,3,4,4,3,4,5,5,2,
        1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,3,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,4,
        2,3,3,4,3,4,2,3,3,4,4,5,4,5,3,2,3,4,4,3,4,5,3,2,4,5,5,4,5,2,4,1,
        1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,3,2,3,3,4,3,4,4,5,3,2,4,3,4,3,5,2,
        2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,4,3,4,4,3,4,5,5,4,4,3,5,2,5,4,2,1,
        2,3,3,4,3,4,4,5,3,4,4,5,2,3,3,2,3,4,4,5,4,5,5,2,4,3,5,4,3,2,4,1,
        3,4,4,5,4,5,3,4,4,5,5,2,3,4,2,1,2,3,3,2,3,4,2,1,3,2,4,1,2,1,1,0
    };
    //   256*5 = 1280 entries
    private int[,] edge_connect_list = {
        {-1, -1, -1, -1}, { -1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  1,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  8,  3, -1},  {9,  8,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  3, -1},  {1,  2, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  2, 10, -1},  {0,  2,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  8,  3, -1},  {2, 10,  8, -1}, {10,  9,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3, 11,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0, 11,  2, -1},  {8, 11,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  9,  0, -1},  {2,  3, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1, 11,  2, -1},  {1,  9, 11, -1},  {9,  8, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3, 10,  1, -1}, {11, 10,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0, 10,  1, -1},  {0,  8, 10, -1},  {8, 11, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  9,  0, -1},  {3, 11,  9, -1}, {11, 10,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  8, 10, -1}, {10,  8, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  7,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  3,  0, -1},  {7,  3,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  1,  9, -1},  {8,  4,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  1,  9, -1},  {4,  7,  1, -1},  {7,  3,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 10, -1},  {8,  4,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  4,  7, -1},  {3,  0,  4, -1},  {1,  2, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  2, 10, -1},  {9,  0,  2, -1},  {8,  4,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2, 10,  9, -1},  {2,  9,  7, -1},  {2,  7,  3, -1},  {7,  9,  4, -1}, {-1, -1, -1, -1},
        {8,  4,  7, -1},  {3, 11,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {11,  4,  7, -1}, {11,  2,  4, -1},  {2,  0,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  0,  1, -1},  {8,  4,  7, -1},  {2,  3, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  7, 11, -1},  {9,  4, 11, -1},  {9, 11,  2, -1},  {9,  2,  1, -1}, {-1, -1, -1, -1},
        {3, 10,  1, -1},  {3, 11, 10, -1},  {7,  8,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1, 11, 10, -1},  {1,  4, 11, -1},  {1,  0,  4, -1},  {7, 11,  4, -1}, {-1, -1, -1, -1},
        {4,  7,  8, -1},  {9,  0, 11, -1},  {9, 11, 10, -1}, {11,  0,  3, -1}, {-1, -1, -1, -1},
        {4,  7, 11, -1},  {4, 11,  9, -1},  {9, 11, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  5,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  5,  4, -1},  {0,  8,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  5,  4, -1},  {1,  5,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  5,  4, -1},  {8,  3,  5, -1},  {3,  1,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 10, -1},  {9,  5,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  0,  8, -1},  {1,  2, 10, -1},  {4,  9,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5,  2, 10, -1},  {5,  4,  2, -1},  {4,  0,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2, 10,  5, -1},  {3,  2,  5, -1},  {3,  5,  4, -1},  {3,  4,  8, -1}, {-1, -1, -1, -1},
        {9,  5,  4, -1},  {2,  3, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0, 11,  2, -1},  {0,  8, 11, -1},  {4,  9,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  5,  4, -1},  {0,  1,  5, -1},  {2,  3, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  1,  5, -1},  {2,  5,  8, -1},  {2,  8, 11, -1},  {4,  8,  5, -1}, {-1, -1, -1, -1},
        {10,  3, 11, -1}, {10,  1,  3, -1},  {9,  5,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  9,  5, -1},  {0,  8,  1, -1},  {8, 10,  1, -1},  {8, 11, 10, -1}, {-1, -1, -1, -1},
        {5,  4,  0, -1},  {5,  0, 11, -1},  {5, 11, 10, -1}, {11,  0,  3, -1}, {-1, -1, -1, -1},
        {5,  4,  8, -1},  {5,  8, 10, -1}, {10,  8, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  7,  8, -1},  {5,  7,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  3,  0, -1},  {9,  5,  3, -1},  {5,  7,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  7,  8, -1},  {0,  1,  7, -1},  {1,  5,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  5,  3, -1},  {3,  5,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  7,  8, -1},  {9,  5,  7, -1}, {10,  1,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  1,  2, -1},  {9,  5,  0, -1},  {5,  3,  0, -1},  {5,  7,  3, -1}, {-1, -1, -1, -1},
        {8,  0,  2, -1},  {8,  2,  5, -1},  {8,  5,  7, -1}, {10,  5,  2, -1}, {-1, -1, -1, -1},
        {2, 10,  5, -1},  {2,  5,  3, -1},  {3,  5,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {7,  9,  5, -1},  {7,  8,  9, -1},  {3, 11,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  5,  7, -1},  {9,  7,  2, -1},  {9,  2,  0, -1},  {2,  7, 11, -1}, {-1, -1, -1, -1},
        {2,  3, 11, -1},  {0,  1,  8, -1},  {1,  7,  8, -1},  {1,  5,  7, -1}, {-1, -1, -1, -1},
        {11,  2,  1, -1}, {11,  1,  7, -1},  {7,  1,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  5,  8, -1},  {8,  5,  7, -1}, {10,  1,  3, -1}, {10,  3, 11, -1}, {-1, -1, -1, -1},
        {5,  7,  0, -1},  {5,  0,  9, -1},  {7, 11,  0, -1},  {1,  0, 10, -1}, {11, 10,  0, -1},
        {11, 10,  0, -1}, {11,  0,  3, -1}, {10,  5,  0, -1},  {8,  0,  7, -1},  {5,  7,  0, -1},
        {11, 10,  5, -1},  {7, 11,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  6,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  3, -1},  {5, 10,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  0,  1, -1},  {5, 10,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  8,  3, -1},  {1,  9,  8, -1},  {5, 10,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  6,  5, -1},  {2,  6,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  6,  5, -1},  {1,  2,  6, -1},  {3,  0,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  6,  5, -1},  {9,  0,  6, -1},  {0,  2,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5,  9,  8, -1},  {5,  8,  2, -1},  {5,  2,  6, -1},  {3,  2,  8, -1}, {-1, -1, -1, -1},
        {2,  3, 11, -1}, {10,  6,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {11,  0,  8, -1}, {11,  2,  0, -1}, {10,  6,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  1,  9, -1},  {2,  3, 11, -1},  {5, 10,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5, 10,  6, -1},  {1,  9,  2, -1},  {9, 11,  2, -1},  {9,  8, 11, -1}, {-1, -1, -1, -1},
        {6,  3, 11, -1},  {6,  5,  3, -1},  {5,  1,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8, 11, -1},  {0, 11,  5, -1},  {0,  5,  1, -1},  {5, 11,  6, -1}, {-1, -1, -1, -1},
        {3, 11,  6, -1},  {0,  3,  6, -1},  {0,  6,  5, -1},  {0,  5,  9, -1}, {-1, -1, -1, -1},
        {6,  5,  9, -1},  {6,  9, 11, -1}, {11,  9,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5, 10,  6, -1},  {4,  7,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  3,  0, -1},  {4,  7,  3, -1},  {6,  5, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  9,  0, -1},  {5, 10,  6, -1},  {8,  4,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  6,  5, -1},  {1,  9,  7, -1},  {1,  7,  3, -1},  {7,  9,  4, -1}, {-1, -1, -1, -1},
        {6,  1,  2, -1},  {6,  5,  1, -1},  {4,  7,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2,  5, -1},  {5,  2,  6, -1},  {3,  0,  4, -1},  {3,  4,  7, -1}, {-1, -1, -1, -1},
        {8,  4,  7, -1},  {9,  0,  5, -1},  {0,  6,  5, -1},  {0,  2,  6, -1}, {-1, -1, -1, -1},
        {7,  3,  9, -1},  {7,  9,  4, -1},  {3,  2,  9, -1},  {5,  9,  6, -1},  {2,  6,  9, -1},
        {3, 11,  2, -1},  {7,  8,  4, -1}, {10,  6,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5, 10,  6, -1},  {4,  7,  2, -1},  {4,  2,  0, -1},  {2,  7, 11, -1}, {-1, -1, -1, -1},
        {0,  1,  9, -1},  {4,  7,  8, -1},  {2,  3, 11, -1},  {5, 10,  6, -1}, {-1, -1, -1, -1},
        {9,  2,  1, -1},  {9, 11,  2, -1},  {9,  4, 11, -1},  {7, 11,  4, -1},  {5, 10,  6, -1},
        {8,  4,  7, -1},  {3, 11,  5, -1},  {3,  5,  1, -1},  {5, 11,  6, -1}, {-1, -1, -1, -1},
        {5,  1, 11, -1},  {5, 11,  6, -1},  {1,  0, 11, -1},  {7, 11,  4, -1},  {0,  4, 11, -1},
        {0,  5,  9, -1},  {0,  6,  5, -1},  {0,  3,  6, -1}, {11,  6,  3, -1},  {8,  4,  7, -1},
        {6,  5,  9, -1},  {6,  9, 11, -1},  {4,  7,  9, -1},  {7, 11,  9, -1}, {-1, -1, -1, -1},
        {10,  4,  9, -1},  {6,  4, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4, 10,  6, -1},  {4,  9, 10, -1},  {0,  8,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  0,  1, -1}, {10,  6,  0, -1},  {6,  4,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  3,  1, -1},  {8,  1,  6, -1},  {8,  6,  4, -1},  {6,  1, 10, -1}, {-1, -1, -1, -1},
        {1,  4,  9, -1},  {1,  2,  4, -1},  {2,  6,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  0,  8, -1},  {1,  2,  9, -1},  {2,  4,  9, -1},  {2,  6,  4, -1}, {-1, -1, -1, -1},
        {0,  2,  4, -1},  {4,  2,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  3,  2, -1},  {8,  2,  4, -1},  {4,  2,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  4,  9, -1}, {10,  6,  4, -1}, {11,  2,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  2, -1},  {2,  8, 11, -1},  {4,  9, 10, -1},  {4, 10,  6, -1}, {-1, -1, -1, -1},
        {3, 11,  2, -1},  {0,  1,  6, -1},  {0,  6,  4, -1},  {6,  1, 10, -1}, {-1, -1, -1, -1},
        {6,  4,  1, -1},  {6,  1, 10, -1},  {4,  8,  1, -1},  {2,  1, 11, -1},  {8, 11,  1, -1},
        {9,  6,  4, -1},  {9,  3,  6, -1},  {9,  1,  3, -1}, {11,  6,  3, -1}, {-1, -1, -1, -1},
        {8, 11,  1, -1},  {8,  1,  0, -1}, {11,  6,  1, -1},  {9,  1,  4, -1},  {6,  4,  1, -1},
        {3, 11,  6, -1},  {3,  6,  0, -1},  {0,  6,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {6,  4,  8, -1}, {11,  6,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {7, 10,  6, -1},  {7,  8, 10, -1},  {8,  9, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  7,  3, -1},  {0, 10,  7, -1},  {0,  9, 10, -1},  {6,  7, 10, -1}, {-1, -1, -1, -1},
        {10,  6,  7, -1},  {1, 10,  7, -1},  {1,  7,  8, -1},  {1,  8,  0, -1}, {-1, -1, -1, -1},
        {10,  6,  7, -1}, {10,  7,  1, -1},  {1,  7,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2,  6, -1},  {1,  6,  8, -1},  {1,  8,  9, -1},  {8,  6,  7, -1}, {-1, -1, -1, -1},
        {2,  6,  9, -1},  {2,  9,  1, -1},  {6,  7,  9, -1},  {0,  9,  3, -1},  {7,  3,  9, -1},
        {7,  8,  0, -1},  {7,  0,  6, -1},  {6,  0,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {7,  3,  2, -1},  {6,  7,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  3, 11, -1}, {10,  6,  8, -1}, {10,  8,  9, -1},  {8,  6,  7, -1}, {-1, -1, -1, -1},
        {2,  0,  7, -1},  {2,  7, 11, -1},  {0,  9,  7, -1},  {6,  7, 10, -1},  {9, 10,  7, -1},
        {1,  8,  0, -1},  {1,  7,  8, -1},  {1, 10,  7, -1},  {6,  7, 10, -1},  {2,  3, 11, -1},
        {11,  2,  1, -1}, {11,  1,  7, -1}, {10,  6,  1, -1},  {6,  7,  1, -1}, {-1, -1, -1, -1},
        {8,  9,  6, -1},  {8,  6,  7, -1},  {9,  1,  6, -1}, {11,  6,  3, -1},  {1,  3,  6, -1},
        {0,  9,  1, -1}, {11,  6,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {7,  8,  0, -1},  {7,  0,  6, -1},  {3, 11,  0, -1}, {11,  6,  0, -1}, {-1, -1, -1, -1},
        {7, 11,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {7,  6, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  0,  8, -1}, {11,  7,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  1,  9, -1}, {11,  7,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  1,  9, -1},  {8,  3,  1, -1}, {11,  7,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  1,  2, -1},  {6, 11,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 10, -1},  {3,  0,  8, -1},  {6, 11,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  9,  0, -1},  {2, 10,  9, -1},  {6, 11,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {6, 11,  7, -1},  {2, 10,  3, -1}, {10,  8,  3, -1}, {10,  9,  8, -1}, {-1, -1, -1, -1},
        {7,  2,  3, -1},  {6,  2,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {7,  0,  8, -1},  {7,  6,  0, -1},  {6,  2,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  7,  6, -1},  {2,  3,  7, -1},  {0,  1,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  6,  2, -1},  {1,  8,  6, -1},  {1,  9,  8, -1},  {8,  7,  6, -1}, {-1, -1, -1, -1},
        {10,  7,  6, -1}, {10,  1,  7, -1},  {1,  3,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  7,  6, -1},  {1,  7, 10, -1},  {1,  8,  7, -1},  {1,  0,  8, -1}, {-1, -1, -1, -1},
        {0,  3,  7, -1},  {0,  7, 10, -1},  {0, 10,  9, -1},  {6, 10,  7, -1}, {-1, -1, -1, -1},
        {7,  6, 10, -1},  {7, 10,  8, -1},  {8, 10,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {6,  8,  4, -1}, {11,  8,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  6, 11, -1},  {3,  0,  6, -1},  {0,  4,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  6, 11, -1},  {8,  4,  6, -1},  {9,  0,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  4,  6, -1},  {9,  6,  3, -1},  {9,  3,  1, -1}, {11,  3,  6, -1}, {-1, -1, -1, -1},
        {6,  8,  4, -1},  {6, 11,  8, -1},  {2, 10,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 10, -1},  {3,  0, 11, -1},  {0,  6, 11, -1},  {0,  4,  6, -1}, {-1, -1, -1, -1},
        {4, 11,  8, -1},  {4,  6, 11, -1},  {0,  2,  9, -1},  {2, 10,  9, -1}, {-1, -1, -1, -1},
        {10,  9,  3, -1}, {10,  3,  2, -1},  {9,  4,  3, -1}, {11,  3,  6, -1},  {4,  6,  3, -1},
        {8,  2,  3, -1},  {8,  4,  2, -1},  {4,  6,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  4,  2, -1},  {4,  6,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  9,  0, -1},  {2,  3,  4, -1},  {2,  4,  6, -1},  {4,  3,  8, -1}, {-1, -1, -1, -1},
        {1,  9,  4, -1},  {1,  4,  2, -1},  {2,  4,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  1,  3, -1},  {8,  6,  1, -1},  {8,  4,  6, -1},  {6, 10,  1, -1}, {-1, -1, -1, -1},
        {10,  1,  0, -1}, {10,  0,  6, -1},  {6,  0,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  6,  3, -1},  {4,  3,  8, -1},  {6, 10,  3, -1},  {0,  3,  9, -1}, {10,  9,  3, -1},
        {10,  9,  4, -1},  {6, 10,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  9,  5, -1},  {7,  6, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  3, -1},  {4,  9,  5, -1}, {11,  7,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5,  0,  1, -1},  {5,  4,  0, -1},  {7,  6, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {11,  7,  6, -1},  {8,  3,  4, -1},  {3,  5,  4, -1},  {3,  1,  5, -1}, {-1, -1, -1, -1},
        {9,  5,  4, -1}, {10,  1,  2, -1},  {7,  6, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {6, 11,  7, -1},  {1,  2, 10, -1},  {0,  8,  3, -1},  {4,  9,  5, -1}, {-1, -1, -1, -1},
        {7,  6, 11, -1},  {5,  4, 10, -1},  {4,  2, 10, -1},  {4,  0,  2, -1}, {-1, -1, -1, -1},
        {3,  4,  8, -1},  {3,  5,  4, -1},  {3,  2,  5, -1}, {10,  5,  2, -1}, {11,  7,  6, -1},
        {7,  2,  3, -1},  {7,  6,  2, -1},  {5,  4,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  5,  4, -1},  {0,  8,  6, -1},  {0,  6,  2, -1},  {6,  8,  7, -1}, {-1, -1, -1, -1},
        {3,  6,  2, -1},  {3,  7,  6, -1},  {1,  5,  0, -1},  {5,  4,  0, -1}, {-1, -1, -1, -1},
        {6,  2,  8, -1},  {6,  8,  7, -1},  {2,  1,  8, -1},  {4,  8,  5, -1},  {1,  5,  8, -1},
        {9,  5,  4, -1}, {10,  1,  6, -1},  {1,  7,  6, -1},  {1,  3,  7, -1}, {-1, -1, -1, -1},
        {1,  6, 10, -1},  {1,  7,  6, -1},  {1,  0,  7, -1},  {8,  7,  0, -1},  {9,  5,  4, -1},
        {4,  0, 10, -1},  {4, 10,  5, -1},  {0,  3, 10, -1},  {6, 10,  7, -1},  {3,  7, 10, -1},
        {7,  6, 10, -1},  {7, 10,  8, -1},  {5,  4, 10, -1},  {4,  8, 10, -1}, {-1, -1, -1, -1},
        {6,  9,  5, -1},  {6, 11,  9, -1}, {11,  8,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  6, 11, -1},  {0,  6,  3, -1},  {0,  5,  6, -1},  {0,  9,  5, -1}, {-1, -1, -1, -1},
        {0, 11,  8, -1},  {0,  5, 11, -1},  {0,  1,  5, -1},  {5,  6, 11, -1}, {-1, -1, -1, -1},
        {6, 11,  3, -1},  {6,  3,  5, -1},  {5,  3,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 10, -1},  {9,  5, 11, -1},  {9, 11,  8, -1}, {11,  5,  6, -1}, {-1, -1, -1, -1},
        {0, 11,  3, -1},  {0,  6, 11, -1},  {0,  9,  6, -1},  {5,  6,  9, -1},  {1,  2, 10, -1},
        {11,  8,  5, -1}, {11,  5,  6, -1},  {8,  0,  5, -1}, {10,  5,  2, -1},  {0,  2,  5, -1},
        {6, 11,  3, -1},  {6,  3,  5, -1},  {2, 10,  3, -1}, {10,  5,  3, -1}, {-1, -1, -1, -1},
        {5,  8,  9, -1},  {5,  2,  8, -1},  {5,  6,  2, -1},  {3,  8,  2, -1}, {-1, -1, -1, -1},
        {9,  5,  6, -1},  {9,  6,  0, -1},  {0,  6,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  5,  8, -1},  {1,  8,  0, -1},  {5,  6,  8, -1},  {3,  8,  2, -1},  {6,  2,  8, -1},
        {1,  5,  6, -1},  {2,  1,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  3,  6, -1},  {1,  6, 10, -1},  {3,  8,  6, -1},  {5,  6,  9, -1},  {8,  9,  6, -1},
        {10,  1,  0, -1}, {10,  0,  6, -1},  {9,  5,  0, -1},  {5,  6,  0, -1}, {-1, -1, -1, -1},
        {0,  3,  8, -1},  {5,  6, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  5,  6, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {11,  5, 10, -1},  {7,  5, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {11,  5, 10, -1}, {11,  7,  5, -1},  {8,  3,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5, 11,  7, -1},  {5, 10, 11, -1},  {1,  9,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {10,  7,  5, -1}, {10, 11,  7, -1},  {9,  8,  1, -1},  {8,  3,  1, -1}, {-1, -1, -1, -1},
        {11,  1,  2, -1}, {11,  7,  1, -1},  {7,  5,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  3, -1},  {1,  2,  7, -1},  {1,  7,  5, -1},  {7,  2, 11, -1}, {-1, -1, -1, -1},
        {9,  7,  5, -1},  {9,  2,  7, -1},  {9,  0,  2, -1},  {2, 11,  7, -1}, {-1, -1, -1, -1},
        {7,  5,  2, -1},  {7,  2, 11, -1},  {5,  9,  2, -1},  {3,  2,  8, -1},  {9,  8,  2, -1},
        {2,  5, 10, -1},  {2,  3,  5, -1},  {3,  7,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  2,  0, -1},  {8,  5,  2, -1},  {8,  7,  5, -1}, {10,  2,  5, -1}, {-1, -1, -1, -1},
        {9,  0,  1, -1},  {5, 10,  3, -1},  {5,  3,  7, -1},  {3, 10,  2, -1}, {-1, -1, -1, -1},
        {9,  8,  2, -1},  {9,  2,  1, -1},  {8,  7,  2, -1}, {10,  2,  5, -1},  {7,  5,  2, -1},
        {1,  3,  5, -1},  {3,  7,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  7, -1},  {0,  7,  1, -1},  {1,  7,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  0,  3, -1},  {9,  3,  5, -1},  {5,  3,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9,  8,  7, -1},  {5,  9,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5,  8,  4, -1},  {5, 10,  8, -1}, {10, 11,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {5,  0,  4, -1},  {5, 11,  0, -1},  {5, 10, 11, -1}, {11,  3,  0, -1}, {-1, -1, -1, -1},
        {0,  1,  9, -1},  {8,  4, 10, -1},  {8, 10, 11, -1}, {10,  4,  5, -1}, {-1, -1, -1, -1},
        {10, 11,  4, -1}, {10,  4,  5, -1}, {11,  3,  4, -1},  {9,  4,  1, -1},  {3,  1,  4, -1},
        {2,  5,  1, -1},  {2,  8,  5, -1},  {2, 11,  8, -1},  {4,  5,  8, -1}, {-1, -1, -1, -1},
        {0,  4, 11, -1},  {0, 11,  3, -1},  {4,  5, 11, -1},  {2, 11,  1, -1},  {5,  1, 11, -1},
        {0,  2,  5, -1},  {0,  5,  9, -1},  {2, 11,  5, -1},  {4,  5,  8, -1}, {11,  8,  5, -1},
        {9,  4,  5, -1},  {2, 11,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  5, 10, -1},  {3,  5,  2, -1},  {3,  4,  5, -1},  {3,  8,  4, -1}, {-1, -1, -1, -1},
        {5, 10,  2, -1},  {5,  2,  4, -1},  {4,  2,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3, 10,  2, -1},  {3,  5, 10, -1},  {3,  8,  5, -1},  {4,  5,  8, -1},  {0,  1,  9, -1},
        {5, 10,  2, -1},  {5,  2,  4, -1},  {1,  9,  2, -1},  {9,  4,  2, -1}, {-1, -1, -1, -1},
        {8,  4,  5, -1},  {8,  5,  3, -1},  {3,  5,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  4,  5, -1},  {1,  0,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {8,  4,  5, -1},  {8,  5,  3, -1},  {9,  0,  5, -1},  {0,  3,  5, -1}, {-1, -1, -1, -1},
        {9,  4,  5, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4, 11,  7, -1},  {4,  9, 11, -1},  {9, 10, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  8,  3, -1},  {4,  9,  7, -1},  {9, 11,  7, -1},  {9, 10, 11, -1}, {-1, -1, -1, -1},
        {1, 10, 11, -1},  {1, 11,  4, -1},  {1,  4,  0, -1},  {7,  4, 11, -1}, {-1, -1, -1, -1},
        {3,  1,  4, -1},  {3,  4,  8, -1},  {1, 10,  4, -1},  {7,  4, 11, -1}, {10, 11,  4, -1},
        {4, 11,  7, -1},  {9, 11,  4, -1},  {9,  2, 11, -1},  {9,  1,  2, -1}, {-1, -1, -1, -1},
        {9,  7,  4, -1},  {9, 11,  7, -1},  {9,  1, 11, -1},  {2, 11,  1, -1},  {0,  8,  3, -1},
        {11,  7,  4, -1}, {11,  4,  2, -1},  {2,  4,  0, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {11,  7,  4, -1}, {11,  4,  2, -1},  {8,  3,  4, -1},  {3,  2,  4, -1}, {-1, -1, -1, -1},
        {2,  9, 10, -1},  {2,  7,  9, -1},  {2,  3,  7, -1},  {7,  4,  9, -1}, {-1, -1, -1, -1},
        {9, 10,  7, -1},  {9,  7,  4, -1}, {10,  2,  7, -1},  {8,  7,  0, -1},  {2,  0,  7, -1},
        {3,  7, 10, -1},  {3, 10,  2, -1},  {7,  4, 10, -1},  {1, 10,  0, -1},  {4,  0, 10, -1},
        {1, 10,  2, -1},  {8,  7,  4, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  9,  1, -1},  {4,  1,  7, -1},  {7,  1,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  9,  1, -1},  {4,  1,  7, -1},  {0,  8,  1, -1},  {8,  7,  1, -1}, {-1, -1, -1, -1},
        {4,  0,  3, -1},  {7,  4,  3, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {4,  8,  7, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9, 10,  8, -1}, {10, 11,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  0,  9, -1},  {3,  9, 11, -1}, {11,  9, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  1, 10, -1},  {0, 10,  8, -1},  {8, 10, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  1, 10, -1}, {11,  3, 10, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  2, 11, -1},  {1, 11,  9, -1},  {9, 11,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  0,  9, -1},  {3,  9, 11, -1},  {1,  2,  9, -1},  {2, 11,  9, -1}, {-1, -1, -1, -1},
        {0,  2, 11, -1},  {8,  0, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {3,  2, 11, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  3,  8, -1},  {2,  8, 10, -1}, {10,  8,  9, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {9, 10,  2, -1},  {0,  9,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {2,  3,  8, -1},  {2,  8, 10, -1},  {0,  1,  8, -1},  {1, 10,  8, -1}, {-1, -1, -1, -1},
        {1, 10,  2, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {1,  3,  8, -1},  {9,  1,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  9,  1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {0,  3,  8, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1},
        {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}
    };

    #endregion

    // Use this for initialization
    void Start () {
        //float dist = DistancePointToBox(new Vector3(4f, 4f, 4f), Vector3.zero, new Vector3(1f, 2f, 5f), Quaternion.identity);
        //MarchCubes();
    }
	
	// Update is called once per frame
	void Update () {
        //MarchCubes();
        DrawVoxel(Color.red, transform.position);
    }

    private float SampleDensity(Vector3 pos) {
        float density = -(pos.magnitude * 1.2f - 5) - pos.x * 0.2f - Mathf.Sin(pos.z + Time.fixedTime * 0.1f) * 0.9f - Mathf.Cos(pos.x * 2.6f * Time.fixedTime * 0.047f) + pos.z * 0.4f;
        //density += pos.x * 0.2f + (pos.z * pos.y * 0.4f + 0.5f);
        //density *= 0.1f;
        //float targetDist = 2f;
        //float distToSegment = DistancePointToSegment3D(pos, new Vector3(-4f, -8f, -10f)*0.5f, new Vector3(8f, 9f, 6f) * 0.5f);
        //float densitySharpness = 2f;
        //density += Mathf.Clamp(-densitySharpness * (distToSegment - targetDist), 0f, 10f);
        //distToSegment = DistancePointToSegment3D(pos, new Vector3(-4f, 8f, -10f) * 0.8f, new Vector3(-8f, 9f, -6f) * 0.5f);
        //density += Mathf.Clamp(-densitySharpness * (distToSegment - targetDist), 0f, 10f);
        //distToSegment = DistancePointToSegment3D(pos, new Vector3(-8f, 8f, -3f) * 0.5f, new Vector3(5f, 2f, -4f) * 1f);
        //density += Mathf.Clamp(-densitySharpness * (distToSegment - targetDist), 0f, 10f);
        density = -0.5f;
        float dist = DistancePointToBox(pos, Vector3.zero, new Vector3(1f, 3f, 6f), Quaternion.Euler(70f, -138f, 9f));
        density += 1f / (dist * dist);
        dist = DistancePointToBox(pos, new Vector3(-3f, 3f, -1f), new Vector3(2f, 1f, 6f), Quaternion.Euler(70f, -138f, 9f));
        density += 1f / (dist * dist);
        dist = DistancePointToBox(pos, new Vector3(1f, -2f, 4f), new Vector3(4f, 3f, 1f), Quaternion.Euler(7f, -13f, 9f));
        density += 1f / (dist * dist);
        return density;
    }

    private void MarchCubes() {
        float halfSize = cellSize * 0.5f;
        Vector3[] cubeVerts = {
            //front face
            new Vector3(-halfSize, -halfSize, -halfSize),		//LB   0
			new Vector3(-halfSize,  halfSize, -halfSize),		//LT   1
            new Vector3(halfSize,  halfSize, -halfSize),		//RT   2
			new Vector3(halfSize, -halfSize, -halfSize),		//RB   3
			//back
			new Vector3(-halfSize, -halfSize,  halfSize),		// LB  4
			new Vector3(-halfSize,  halfSize,  halfSize),		// LT  5
			new Vector3(halfSize,  halfSize,  halfSize),		// RT  6
			new Vector3(halfSize, -halfSize,  halfSize)		    // RB  7
		};

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        // Traverse the Cell Grid!
        for(int x = 0; x < resolution; x++) {
            for(int y = 0; y < resolution; y++) {
                for(int z = 0; z < resolution; z++) {

                    Vector3 pos = new Vector3((x - resolution / 2) * cellSize, (y - resolution / 2) * cellSize, (z - resolution / 2) * cellSize);

                    string debugWeights = "SampleDensity at: " + pos.ToString();
                    float[] weights = new float[8];
                    for(int w = 0; w < weights.Length; w++) {                        
                        weights[w] = SampleDensity(pos + cubeVerts[w]);
                        debugWeights += ", vert[" + w.ToString() + "] = " + weights[w].ToString();                        
                    }
                    //Debug.Log(debugWeights);

                    int marchingCase =
                        (weights[7] > cap ? 1 : 0) * 128 +
                        (weights[6] > cap ? 1 : 0) * 64 +
                        (weights[5] > cap ? 1 : 0) * 32 +
                        (weights[4] > cap ? 1 : 0) * 16 +
                        (weights[3] > cap ? 1 : 0) * 8 +
                        (weights[2] > cap ? 1 : 0) * 4 +
                        (weights[1] > cap ? 1 : 0) * 2 +
                        (weights[0] > cap ? 1 : 0) * 1;

                    int numpolys = case_to_numpolys[marchingCase];
                    //Debug.Log("numPolys = " + numpolys.ToString() + ", marchingcase: " + marchingCase.ToString());
                    for(int p = 0; p < numpolys; p++) {
                        int[] polyEdges = { edge_connect_list[marchingCase * 5 + p, 0],
                                            edge_connect_list[marchingCase * 5 + p, 1],
                                            edge_connect_list[marchingCase * 5 + p, 2],
                                            edge_connect_list[marchingCase * 5 + p, 3],
                        };

                        int va = edge_to_verts[polyEdges[0],0];
                        int vb = edge_to_verts[polyEdges[0],1];
                        float amount = (cap - weights[va]) / (weights[vb] - weights[va]);
                        vertices.Add(Vector3.Lerp(pos + cubeVerts[va], pos + cubeVerts[vb], amount));
                        triangles.Add(vertices.Count - 1);
                        //NewPoly.A = lerp(p + cubeVerts[va], p + cubeVerts[vb], amount);
                        //NewPoly.NA = -TriLinearInterpNormal(NewPoly.A);

                        va = edge_to_verts[polyEdges[1],0];
                        vb = edge_to_verts[polyEdges[1],1];
                        amount = (cap - weights[va]) / (weights[vb] - weights[va]);
                        vertices.Add(Vector3.Lerp(pos + cubeVerts[va], pos + cubeVerts[vb], amount));
                        triangles.Add(vertices.Count - 1);
                        //NewPoly.NB = -TriLinearInterpNormal(NewPoly.B);

                        va = edge_to_verts[polyEdges[2],0];
                        vb = edge_to_verts[polyEdges[2],1];
                        amount = (cap - weights[va]) / (weights[vb] - weights[va]);
                        vertices.Add(Vector3.Lerp(pos + cubeVerts[va], pos + cubeVerts[vb], amount));
                        triangles.Add(vertices.Count - 1);
                        //NewPoly.NC = -TriLinearInterpNormal(NewPoly.C);
                        
                        //Increase buffer index counter and get current one
                        //InterlockedAdd(index_counter, 1, Index);

                        //Append new poly to buffer
                        //buffer[Index] = NewPoly;
                    }
                }
            }
        }

        Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) {
            mesh = new Mesh();
        }
        else {
            mesh.Clear();
        }
        
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        this.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private float DistancePointToSegment3D(Vector3 point, Vector3 segmentStart, Vector3 segmentEnd) {
        Vector3 v = segmentEnd - segmentStart;
        Vector3 w = point - segmentStart;

        float c1 = Vector3.Dot(w, v);
        if (c1 <= 0)
            return (point - segmentStart).magnitude;

        float c2 = Vector3.Dot(v, v);
        if(c2 <= c1)
            return (point - segmentEnd).magnitude;

        float b = c1 / c2;
        Vector3 pb = segmentStart + b * v;
        return (point - pb).magnitude;
    }

    private float DistancePointToBox(Vector3 point, Vector3 boxPos, Vector3 boxScale, Quaternion boxRotation) {

        Vector3 boxCenterToPoint = point - boxPos + new Vector3(0.0001f, 0.0001f, 0.0001f);  // prevent divide by 0??
        Vector3 right = boxRotation * new Vector3(1f, 0f, 0f);
        Vector3 up = boxRotation * new Vector3(0f, 1f, 0f);
        Vector3 forward = boxRotation * new Vector3(0f, 0f, 1f);

        float dotRight = Vector3.Dot(right, boxCenterToPoint);
        float dotUp = Vector3.Dot(up, boxCenterToPoint);
        float dotForward = Vector3.Dot(forward, boxCenterToPoint);

        float distRight = Mathf.Abs(dotRight) - boxScale.x * 0.5f;
        float distUp = Mathf.Abs(dotUp) - boxScale.y * 0.5f;
        float distForward = Mathf.Abs(dotForward) - boxScale.z * 0.5f;

        //Debug.Log("distRight: " + distRight.ToString() + ", distUp: " + distUp.ToString() + ", distForward: " + distForward.ToString());

        float minDistance = 0.001f;
        float pointRight = Mathf.Min(Mathf.Abs(dotRight), boxScale.x * 0.5f) * dotRight / Mathf.Abs(dotRight); // amount to move in box's local X direction
        float pointUp = Mathf.Min(Mathf.Abs(dotUp), boxScale.y * 0.5f) * dotUp / Mathf.Abs(dotUp);
        float pointForward = Mathf.Min(Mathf.Abs(dotForward), boxScale.z * 0.5f) * dotForward / Mathf.Abs(dotForward);
        Vector3 samplePoint = right * pointRight + up * pointUp + forward * pointForward + boxPos;
        float distance = Mathf.Max(minDistance, (point - samplePoint).magnitude);

        return distance;
        //return Mathf.Max(Mathf.Max(distRight, distUp), distForward);
    }

    private void OnDrawGizmos() {
        // Traverse the Cell Grid!
        for (int x = 0; x < resolution; x++) {
            for (int y = 0; y < resolution; y++) {
                for (int z = 0; z < resolution; z++) {

                    Vector3 pos = new Vector3((x - resolution / 2) * cellSize, (y - resolution / 2) * cellSize, (z - resolution / 2) * cellSize);
                    
                    float density = SampleDensity(pos);
                    Color sdColor = new Color(density * 0.5f + 0.5f, density * 0.5f + 0.5f, density * 0.5f + 0.5f, 1f);
                    if (density > 0f)
                        sdColor.g += 0.5f;
                    if (density < 0f)
                        sdColor.r += 0.4f;
                    Gizmos.color = sdColor;
                    Gizmos.DrawSphere(pos, 0.1f);
                }
            }
        }
                    //for (int i = 0; i < vertices.Length; i++) {
            //Gizmos.color = Color.black;
            //Gizmos.DrawSphere(vertices[i], 0.1f);
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawRay(vertices[i], normals[i]);
        
    }

    // dist_Point_to_Segment(): get the distance of a point to a segment
    //     Input:  a Point P and a Segment S (in any dimension)
    //     Return: the shortest distance from P to S
    /*float
    dist_Point_to_Segment(Point P, Segment S) {
        Vector v = S.P1 - S.P0;
        Vector w = P - S.P0;

        double c1 = dot(w, v);
        if (c1 <= 0)
            return d(P, S.P0);

        double c2 = dot(v, v);
        if (c2 <= c1)
            return d(P, S.P1);

        double b = c1 / c2;
        Point Pb = S.P0 + b * v;
        return d(P, Pb);
    }*/

    public void DrawVoxel(Color Col, Vector3 Pos, float Dur = 0.0f) {
        Vector3 A, B, C, D, E, F, G, H;

        A = Pos - new Vector3(resolution / 2f, resolution / 2f, resolution / 2f) * cellSize;

        B = A + transform.right * resolution * cellSize;
        C = A + transform.up * resolution * cellSize;
        D = A + transform.forward * resolution * cellSize;

        E = A + transform.right * resolution * cellSize + transform.forward * resolution * cellSize;
        F = A + transform.right * resolution * cellSize + transform.up * resolution * cellSize;

        G = A + transform.right * resolution * cellSize + transform.up * resolution * cellSize + transform.forward * resolution * cellSize;
        H = A + transform.up * resolution * cellSize + transform.forward * resolution * cellSize;

        Debug.DrawLine(A, B, Col, Dur);
        Debug.DrawLine(B, E, Col, Dur);
        Debug.DrawLine(E, D, Col, Dur);
        Debug.DrawLine(D, A, Col, Dur);

        Debug.DrawLine(C, F, Col, Dur);
        Debug.DrawLine(F, G, Col, Dur);
        Debug.DrawLine(G, H, Col, Dur);
        Debug.DrawLine(H, C, Col, Dur);

        Debug.DrawLine(A, C, Col, Dur);
        Debug.DrawLine(D, H, Col, Dur);
        Debug.DrawLine(E, G, Col, Dur);
        Debug.DrawLine(B, F, Col, Dur);
    }
}
