using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GathererAI : MonoBehaviour
{

    public static event EventHandler OnGathererClicked;
    
    [SerializeField] private State state;

    private UnitRTS unit;

    private ResourceNode resourceNode;
    private Transform storageTransform;

    [SerializeField] private GameObject selectedSprite;
    [SerializeField] private TMP_Text inventoryText;

    [SerializeField] private int maxInventorySize = 3;

    private Dictionary<ResourceType, int> inventoryAmountDictionary;

    private void Awake()
    {

        unit = GetComponent<UnitRTS>();
        state = State.Idle;

        inventoryAmountDictionary = new Dictionary<ResourceType, int>();
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            inventoryAmountDictionary[resourceType] = 0;
        }

        UpdateInventory();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (OnGathererClicked != null) OnGathererClicked(this, EventArgs.Empty);
            }
        }

        switch (state)
        {
            case State.Idle:
                //resourceNode = GameHandler.I.GetResourceNode();
                if (resourceNode != null)
                    state = State.MovingToResource;
                break;
            case State.MovingToResource:
                if (unit.IsIdle())
                {
                    unit.MoveTo(resourceNode.GetPosition(), () =>
                    {
                        state = State.Gathering;
                    });
                }
                break;
            case State.Gathering:
                if (unit.IsIdle())
                {
                    if (IsInventoryFull())
                    {
                        //Move To Storage
                        storageTransform = GameHandler.I.GetStorageTransform();
                        resourceNode = GameHandler.I.GetResourceNodeNearPosition(resourceNode.GetPosition());
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        //Gather resources
                        unit.PlayHarvestAnimation(GrabResourceFromNode);

                        //---- In case of different animations ----
                        //switch (resourceNode.GetResourceType())
                        //{
                        //    case ResourceType.Wood:
                        //        break;
                        //    case ResourceType.Stone:
                        //        break;
                        //    default:
                        //        break;
                        //}
                    }
                }
                break;
            case State.MovingToStorage:
                if (unit.IsIdle())
                {
                    unit.MoveTo(storageTransform.position, () =>
                    {
                        DropInventoryAmountIntoGameResources();
                        UpdateInventory();
                        state = State.Idle;
                    });
                }
                break;
            default:
                break;
        }
    }

    private void GrabResourceFromNode()
    {
        ResourceType resourceType = resourceNode.GrabResource();
        inventoryAmountDictionary[resourceType]++;
        UpdateInventory();
    }

    private bool IsInventoryFull()
    {
        return GetTotalInventoryAmount() >= maxInventorySize;
    }

    private int GetTotalInventoryAmount()
    {
        int total = 0;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            total += inventoryAmountDictionary[resourceType];
        }

        return total;
    }
    
    private void UpdateInventory()
    {
        int inventoryAmount = GetTotalInventoryAmount();

        if (inventoryAmount > 0)
            inventoryText.text = inventoryAmount.ToString();
        else
            inventoryText.text = "";
    }

    private void DropInventoryAmountIntoGameResources()
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            GameResources.AddResourceAmount(resourceType, inventoryAmountDictionary[resourceType]);
            inventoryAmountDictionary[resourceType] = 0;
        }
    }


    public void SetResourceNode(ResourceNode resourceNode)
    {
        this.resourceNode = resourceNode;
    }

    public void SetSelectedSprite(bool visible)
    {
        selectedSprite.SetActive(visible);
    }
}

public enum State
{
    Idle,
    MovingToResource,
    Gathering,
    MovingToStorage
}