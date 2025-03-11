using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem I;
    public enum WorkMode
    {
        Default,
        Build,
        Destroy
    }

    public WorkMode wm { get; private set; }

    private NodeGrid grid;

    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;

    public PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;

    [SerializeField] private bool debug = false;

    public bool canBuild { get; private set; }

    private void Awake()
    {
        if (I == null)
            I = this;
        else 
            Destroy(this);
    }

    private void Start()
    {
        grid = GridManager.I.grid;

        Init();
    }

    private void Init()
    {
        foreach (var SO in placedObjectTypeSOList)
        {
            PlaceableButton button = Instantiate(buttonPrefab, buttonParent).GetComponent<PlaceableButton>();
            button.Setup(SO, this);
        }
    }

    private void Update()
    {
        if (wm == WorkMode.Build)
        {
            canBuild = true;

            if (placedObjectTypeSO == null)
                return;

            grid.WorldToGridPos(Utils.GetMouseWorldPosition(), out int x, out int y);
            Vector2Int gridPos = new Vector2Int(x, y);
            List<Vector2Int> gridPositions = placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, y), dir);
            
            //Test can build
            foreach (Vector2Int gridPosition in gridPositions)
            {
                Node node = grid.GetNode(gridPosition.x, gridPosition.y);
                if (node == null || !grid.GetNode(gridPosition.x, gridPosition.y).canBuild())
                {
                    canBuild = false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!IsPointerOverUIElement() && placedObjectTypeSO != null)
                    PlaceBuilding(gridPos, gridPositions, canBuild);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                dir = PlacedObjectTypeSO.GetNextDir(dir);
                GridSelector.I.SetSelector(placedObjectTypeSO, dir);
            }
        }
        else if(wm == WorkMode.Destroy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsPointerOverUIElement())
                    DestroyBuilding();
            }
        }
    }

    private void DestroyBuilding()
    {
        grid.WorldToGridPos(Utils.GetMouseWorldPosition(), out int x, out int y);
        Node node = grid.GetNode(x, y);

        PlacedObject placedObject = node.GetPlacedObject();

        if (placedObject != null)
        {
            placedObject.DestroySelf();

            List<Vector2Int> gridPositions = placedObject.GetGridPositionList();

            foreach (Vector2Int gridPosition in gridPositions)
            {
                grid.GetNode(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }

    private void PlaceBuilding(Vector2Int gridPos, List<Vector2Int> gridPositions, bool canBuild)
    {
        if (Utils.IsValidGridPosition(gridPos, grid))
        {
            Node node = grid.GetNode(gridPos.x, gridPos.y);
            if (canBuild)
            {
                Vector2 rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector2 placedObjectWorldPosition = gridPos + rotationOffset;

                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, gridPos, dir, placedObjectTypeSO);

                foreach (Vector2Int gridPosition in gridPositions)
                {
                    grid.GetNode(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
            }
            else
            {
                Console.I.LogError("Cannot build here, position occupied!");

                if (debug)
                    Debug.LogError("Cannot build here, position occupied!");
            }
        }
        else
        {
            Console.I.LogError("Invalid position!");

            if (debug)
                Debug.LogError("Invalid position!");
        }
    }

    public void SetWorkMode(string workMode)
    {
        switch (workMode)
        {
            case "b":
                wm = WorkMode.Build;
                break;
            case "d":
                wm = WorkMode.Destroy;
                break;
            default:
                wm = WorkMode.Default;
                dir = PlacedObjectTypeSO.Dir.Down;
                placedObjectTypeSO = null;
                break;
        }
    }

    public void SetPlaceObjectTypeSO(PlacedObjectTypeSO SO) => placedObjectTypeSO = SO;

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
