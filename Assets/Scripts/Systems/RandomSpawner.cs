using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class RandomGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public int spawnPerPrefab = 5;

    public Transform spawnPlane;

    public int candidateTries = 20; // quantas posições testar pra escolher a melhor
    public float checkRadius = 0.5f;

    void OnValidate()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        if (spawnPlane == null || prefabs.Length == 0)
            return;

        // 🧹 limpar
        List<GameObject> toRemove = new List<GameObject>();
        foreach (Transform child in spawnPlane)
            toRemove.Add(child.gameObject);

        foreach (var obj in toRemove)
            DestroyImmediate(obj);

        Renderer renderer = spawnPlane.GetComponent<Renderer>();
        if (renderer == null)
            return;

        Bounds bounds = renderer.bounds;

        List<Vector3> usedPositions = new List<Vector3>();

        // para cada prefab
        foreach (var prefab in prefabs)
        {
            for (int i = 0; i < spawnPerPrefab; i++)
            {
                Vector3 bestPosition = Vector3.zero;
                float bestDistance = -1f;

                // testar várias posições candidatas
                for (int j = 0; j < candidateTries; j++)
                {
                    float x = Random.Range(bounds.min.x, bounds.max.x);
                    float z = Random.Range(bounds.min.z, bounds.max.z);

                    Vector3 rayOrigin = new Vector3(x, bounds.max.y + 50f, z);
                    RaycastHit hit;

                    if (!Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
                        continue;

                    if (!hit.collider.CompareTag("GroundPlane"))
                        continue;

                    Vector3 position = hit.point;

                    // colisão
                    Collider[] hits = Physics.OverlapSphere(position, checkRadius);

                    bool blocked = false;
                    foreach (var h in hits)
                    {
                        if (!h.CompareTag("GroundPlane") && !h.CompareTag("Ice"))
                        {
                            blocked = true;
                            break;
                        }
                    }

                    if (blocked)
                        continue;

                    // calcular distância mínima até outros pontos
                    float minDist = float.MaxValue;

                    foreach (var pos in usedPositions)
                    {
                        float dist = Vector3.Distance(pos, position);
                        if (dist < minDist)
                            minDist = dist;
                    }

                    // se for o primeiro objeto
                    if (usedPositions.Count == 0)
                        minDist = 999f;

                    // escolher o melhor candidato
                    if (minDist > bestDistance)
                    {
                        bestDistance = minDist;
                        bestPosition = position;
                    }
                }

                // spawn final
                if (bestDistance > 0f)
                {
                    GameObject obj = Instantiate(prefab, bestPosition, Quaternion.identity);
                    usedPositions.Add(bestPosition);
                }
            }
        }
    }
}