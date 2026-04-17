using UnityEngine;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour
{
    private List<PickupItem> pickups = new List<PickupItem>();
    private List<PickupItem> selectedPickups = new List<PickupItem>();

    public TrailGuide trailGuide;

    public System.Action OnAllPickupsCollected;

    public int itemLimit = 5;

    void Start()
    {
        // Pega todos os pickups filhos
        foreach (Transform child in transform)
        {
            PickupItem item = child.GetComponent<PickupItem>();

            if (item != null)
            {
                pickups.Add(item);
            }
        }

        // Desativa todos
        foreach (var p in pickups)
        {
            p.gameObject.SetActive(false);
        }

        // Embaralha a lista
        ShuffleList(pickups);

        // Define limite (máximo 5)
        int limit = Mathf.Min(itemLimit, pickups.Count);

        // Seleciona apenas os 5 primeiros embaralhados
        for (int i = 0; i < limit; i++)
        {
            selectedPickups.Add(pickups[i]);
        }

        // Ativa o primeiro aleatório
        if (selectedPickups.Count > 0)
        {
            ActivatePickup(Random.Range(0, selectedPickups.Count));
        }
    }

    void ActivatePickup(int index)
    {
        PickupItem pickup = selectedPickups[index];

        pickup.gameObject.SetActive(true);

        if (trailGuide != null)
        {
            trailGuide.SetNewTarget(pickup.transform);
        }
    }

    public void OnPickupCollected(PickupItem collected)
    {
        collected.gameObject.SetActive(false);
        selectedPickups.Remove(collected);

        if (selectedPickups.Count > 0)
        {
            ActivatePickup(Random.Range(0, selectedPickups.Count));
        }
        else
        {
            if (trailGuide != null)
            {
                trailGuide.OnTargetCollected();
            }

            OnAllPickupsCollected?.Invoke();
        }
    }

    void ShuffleList(List<PickupItem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}