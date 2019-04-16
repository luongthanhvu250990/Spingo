using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner: MonoBehaviour
{
    [SerializeField]
    Vector2Int gridSize;

    [SerializeField]
    Vector2 offSet;
    [SerializeField]
    float spacing = 20f;

    [SerializeField]
    float delayTime = 1;
    [SerializeField]
    List<GameObject> spawnPrefabs = new List<GameObject>();    

    Vector2 cellSize;
    float areaWidth;
    float areaHeight;
    private void Start()
    {
        areaWidth = Screen.width - offSet.x * 2;
        areaHeight = (Screen.height - offSet.y * 2);
        cellSize = new Vector2(areaWidth / gridSize.x, areaHeight/ gridSize.y);

        StartCoroutine(ISpawn());      
    }
    
   public IEnumerator ISpawn() {
        var wait = new WaitForSeconds(delayTime);
        while (true)
        {
            if (!GameManager.Instance.IsPause)
            {
                int i = Random.Range(0, gridSize.x);
                int j = Random.Range(0, gridSize.y);

                if (PaddleController.Instance != null)
                {
                    Vector3 pos = new Vector3(Random.Range(i * cellSize.x + spacing, (i + 1) * cellSize.x - spacing) - areaWidth / 2,
                               Random.Range(j * cellSize.y + spacing, (j + 1) * cellSize.y - spacing) - areaHeight / 2) / 100;

                    if (Vector3.Distance(pos, PaddleController.Instance.LeftPoint.position) > 1 &&
                        Vector3.Distance(pos, PaddleController.Instance.RightPoint.position) > 1)
                    {
                        GameObject go = GameObjectUtils.LoadGameObject(transform, spawnPrefabs[Random.Range(0, spawnPrefabs.Count)]);
                        go.transform.position = pos;
                    }
                }
            }
            yield return wait;
        }
    }
       
    private struct GridPos
    {
        public int x;
        public int y;

        public bool isAvailable;       
    }
}
