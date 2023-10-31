using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] GameObject Platform;
    [SerializeField] List<Platform> removedPlatforms = new();
    public List<Platform> platforms = new(), activePlatforms = new();
    
    public void GenerateMap(int rowsNumber,int colsNumber, int numberOfActivePlatforms, bool canPlatformBeUsedManyTimes)
    {
        activePlatforms.Clear();
        platforms.Clear();
        removedPlatforms.Clear();
        foreach(Platform p in gameObject.GetComponentsInChildren<Platform>())
        {
            Destroy(p.gameObject);
        }
        int rowsR = rowsNumber % 2;
        
        for(int i = -(int)rowsNumber / 2; i < (int)rowsNumber/2+rowsR; i++)
        {
            for(int j = 0; j < colsNumber; j++)
            {
                Vector3 spawnPos = new();
                if(rowsR == 0)
                {
                    spawnPos = new Vector3(i * 2.5f+1.25f, 0, j * 2.5f) + this.transform.position;
                }
                else
                {
                    spawnPos = new Vector3(i * 2.5f, 0, j * 2.5f) + this.transform.position;
                }

                Vector3 spawnRot = new Vector3(-90, 0, 0);
                GameObject _platform = GameObject.Instantiate(Platform, spawnPos,Quaternion.Euler(spawnRot),this.transform);
                platforms.Add(_platform.GetComponent<Platform>());
            }
        }
        
        for(int i = 0; i < numberOfActivePlatforms; i++)
        {
            
            int r = Random.Range(0, platforms.Count);
            activePlatforms.Add(platforms[r]);
            
            if(canPlatformBeUsedManyTimes) removedPlatforms.Add(platforms[r]);
            
            platforms.RemoveAt(r);

            if(removedPlatforms.Count > 1)
            {
                platforms.Add(removedPlatforms[0]);
                removedPlatforms.RemoveAt(0);
            }
        }

    }
}
