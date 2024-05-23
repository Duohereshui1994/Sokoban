using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManagerScript : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject playerPrefab;       //玩家预制体
    [SerializeField] GameObject boxPrefab;          //箱子预制体
    [SerializeField] GameObject goalPrefab;         //目标预制体
    [SerializeField] GameObject wallPrefab;         //墙预制体
    [SerializeField] GameObject particlePrefab;     //粒子预制体
    [SerializeField] GameObject panel;              //面板

    [Header("AudioData")]
    [SerializeField] AudioSource moveSFX;


    MapManager mapManager;                          //地图管理器
    SceneController sceneController;                //场景控制器

    int[,] map;                                     //地图数组
    GameObject[,] field;                            //游戏对象数组
    GameObject particleInstance;                    //粒子实例



    void Start()
    {
        //设置屏幕分辨率
        Screen.SetResolution(1280, 720, false);

        //获取场景控制器
        sceneController = FindObjectOfType<SceneController>();


        //获取地图管理器
        mapManager = FindObjectOfType<MapManager>();

        //加载地图
        map = mapManager.LoadMap();

        //实例化游戏对象
        InstantiateGameObjects();
    }

    void Update()
    {
        if (IsCleard())
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);

            PlayerMove();

            if (Input.GetKeyDown(KeyCode.R))
            {
                sceneController.RestartLevel();
            }

        }
    }

    /// <summary>
    /// 实例化游戏对象
    /// </summary>
    private void InstantiateGameObjects()
    {
        //初始化游戏对象数组
        field = new GameObject[map.GetLength(0), map.GetLength(1)];


        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                switch (map[y, x])
                {
                    case 1:
                        field[y, x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                        break;
                    case 2:
                        field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                        break;
                    case 3:
                        field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0.01f), Quaternion.identity);
                        break;
                    case 4:
                        field[y, x] = Instantiate(wallPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 玩家移动
    /// </summary>
    private void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            if (IsValidMove(playerIndex + new Vector2Int(1, 0)))
            {
                MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
                EmitParticleEffect();
                moveSFX.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            if (IsValidMove(playerIndex + new Vector2Int(-1, 0)))
            {
                MoveNumber(playerIndex, playerIndex + new Vector2Int(-1, 0));
                EmitParticleEffect();
                moveSFX.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            if (IsValidMove(playerIndex + new Vector2Int(0, -1)))
            {
                MoveNumber(playerIndex, playerIndex + new Vector2Int(0, -1));
                EmitParticleEffect();
                moveSFX.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            if (IsValidMove(playerIndex + new Vector2Int(0, 1)))
            {
                MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));
                EmitParticleEffect();
                moveSFX.Play();
            }
        }
    }

    /// <summary>
    /// 判断是否可以移动
    /// </summary>
    /// <param name="targetIndex">目标位置的索引</param>
    /// <returns>是否可以移动</returns>
    private bool IsValidMove(Vector2Int targetIndex)
    {
        if (targetIndex.y < 0 || targetIndex.y >= field.GetLength(0)) { return false; }
        if (targetIndex.x < 0 || targetIndex.x >= field.GetLength(1)) { return false; }

        return map[targetIndex.y, targetIndex.x] != 4;
    }

    /// <summary>
    /// 获取玩家索引
    /// </summary>
    /// <returns>玩家索引</returns>
    private Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].CompareTag("Player"))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    /// <summary>
    /// 移动数字
    /// </summary>
    /// <param name="moveFrom">起始点</param>
    /// <param name="moveTo">目标点</param>
    /// <returns>是否能移动</returns>
    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        //边界
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        //墙
        if (map[moveTo.y, moveTo.x] == 4) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            Vector2Int nextMoveTo = moveTo + velocity;

            // 检查下一个为止是否在边界内且不是墙
            if (nextMoveTo.y < 0 || nextMoveTo.y >= field.GetLength(0)) { return false; }
            if (nextMoveTo.x < 0 || nextMoveTo.x >= field.GetLength(1)) { return false; }
            if (map[nextMoveTo.y, nextMoveTo.x] == 4) { return false; }

            // 如果下一个为止是箱子，则不能移动箱子
            if (field[nextMoveTo.y, nextMoveTo.x] != null && field[nextMoveTo.y, nextMoveTo.x].tag == "Box") { return false; }

            // 尝试移动箱子
            bool success = MoveNumber(moveTo, nextMoveTo);
            if (!success) { return false; }
        }



        //field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        Vector3 moveToPosition = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;

        return true;
    }

    /// <summary>
    /// 判断是否已经清除所有目标
    /// </summary>
    bool IsCleard()
    {
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || !f.CompareTag("Box")) { return false; }
        }
        return true;
    }

    /// <summary>
    /// 发射粒子效果
    /// </summary>
    void EmitParticleEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            particleInstance = Instantiate(particlePrefab, field[GetPlayerIndex().y, GetPlayerIndex().x].transform.position, Quaternion.identity);
        }
    }

}