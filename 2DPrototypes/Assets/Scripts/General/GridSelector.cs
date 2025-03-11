using UnityEngine;
using static PlacedObjectTypeSO;
using static GridBuildingSystem;

public class GridSelector : MonoBehaviour
{
    public static GridSelector I;

    [SerializeField] private SpriteRenderer GFX;
    [SerializeField] private SpriteRenderer objRend;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color invalidColor;

    private void Awake()
    {
        if (I == null)
            I = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        Vector2 currentMousePos = Utils.GetMouseWorldPosition();
        Vector2 gridPos = Utils.WorldToGrid(currentMousePos);
        transform.position = gridPos;

        switch (GridBuildingSystem.I.wm)
        {
            case WorkMode.Default:
                GFX.color = defaultColor;
                break;
            case WorkMode.Build:

                if (CanBuild())
                    GFX.color = defaultColor;
                else
                    GFX.color = invalidColor;
                break;
            case WorkMode.Destroy:
                GFX.color = invalidColor;
                break;
            default:
                GFX.color = defaultColor;
                break;
        }
    }

    private bool CanBuild() => GridBuildingSystem.I.canBuild;

    public void SetSelector(PlacedObjectTypeSO SO, Dir dir)
    {
        Vector2 rotationOffset = SO.GetRotationOffset(dir);
        objRend.transform.rotation = Quaternion.Euler(0, 0, SO.GetRotationAngle(dir));
        objRend.transform.localPosition = rotationOffset;

        objRend.sprite = SO.visual;
        int x = 0, y = 0;

        switch (dir)
        {
            case Dir.Down:
            case Dir.Up:
                x = SO.width;
                y = SO.height;
                break;
            case Dir.Left:
            case Dir.Right:
                x = SO.height;
                y = SO.width;
                break;
            default:
                break;
        }

        GFX.size = new Vector2(x, y);
    }
    public void ResetSelector()
    {
        objRend.sprite = null;
        GFX.size = Vector2.one;
    }
}
