using UnityEngine;
using UnityEngine.UI;

public class PlaceableButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image placeableImage;

    [SerializeField] private PlacedObjectTypeSO pObject;

    public void Setup(PlacedObjectTypeSO SO, GridBuildingSystem GBS)
    {
        pObject = SO;
        placeableImage.sprite = SO.visual;

        button.onClick.AddListener(() => { 
            GBS.SetPlaceObjectTypeSO(pObject);
            GridSelector.I.SetSelector(SO, GBS.dir);
            GridBuildingSystem.I.SetWorkMode("b");
        });
    }
}
