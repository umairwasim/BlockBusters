using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    public static BlocksManager Instance;

    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private GameObject[] mapsArray;
    [SerializeField] private LayerMask layerMask;

    private int levelNo;
    private int spawnLimit = 20;
    private float spawnInterval;
    private float minRange;
    private float maxRange;
    private Vector3 minBounds; // The minimum bounds
    private Vector3 maxBounds; // The maximum bounds
    private Collider mapCollider;

    private List<GameObject> items = new();

    private const string LEVEL = "Level";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameManager.Instance.GameState == GameState.Starting)
        {
            StartCoroutine(SpawnMap());
            StartCoroutine(SpawnItemsRoutine());
        }
        levelNo = PlayerPrefs.GetInt(LEVEL, 0);
    }

    //Intialize Level Data
    public void InitializeLevelData(LevelData level)
    {
        levelNo = GameManager.Instance.GetLevel();

        spawnInterval = level.spawnInterval;
        minRange = level.minRange;
        maxRange = level.maxRange;

        //set FOV for levels
        if (levelNo > 1)
            CameraShake.Instance.SetCameraFOV(20f);
        else
            CameraShake.Instance.SetCameraFOV(15f);
    }

    private IEnumerator SpawnMap()
    {
        yield return new WaitForSeconds(0.25f);

        //First disable all maps 
        for (int i = 0; i < mapsArray.Length; i++)
        {
            mapsArray[i].SetActive(false);
        }
        //Check if level can be activated, activte it and assign collider
        if (levelNo < mapsArray.Length)
        {
            mapsArray[levelNo].SetActive(true);
            mapsArray[levelNo].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InElastic);

            //Assign map collider for bounds calculation
            mapCollider = mapsArray[levelNo].GetComponentInChildren<Collider>();
        }
    }

    private IEnumerator SpawnItemsRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            // Get the bounds of the spawn map area
            minBounds = mapCollider.bounds.min;
            maxBounds = mapCollider.bounds.max;

            // Generate a random position within the defined bounds
            float x = Random.Range(minBounds.x, maxBounds.x);
            float y = 3f;
            float z = Random.Range(minBounds.z, maxBounds.z);
            Vector3 randomPosition = new(x, y, z);

            //wai for random time interval
            yield return new WaitForSeconds(Random.Range(minRange, maxRange));

            //if there is platform beneath the spawn manager , then spawn object
            if (Physics.Raycast(randomPosition, Vector3.down, float.MaxValue, layerMask))
            {
                // Spawn the object at the random position
                GameObject spawnedItem = Instantiate(itemPrefabs[i], randomPosition, Quaternion.identity);
                AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSfx);
                items.Add(spawnedItem);
            }
        }
        yield return new WaitForSeconds(spawnInterval);

        //Call spawning recursively if it is within the spawn limit
        yield return new WaitUntil(() => items.Count <= spawnLimit);
        StartCoroutine(SpawnItemsRoutine());
    }

    public void StopItemsSpawning()
    {
        StopAllCoroutines();
        items.Clear();
    }

    public void RemoveItem(GameObject itemToRemove)
    {
        if (items.Count > 0)
            items.Remove(itemToRemove);
    }
}
