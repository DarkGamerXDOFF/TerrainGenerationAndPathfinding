using UnityEngine;

public class MouseToMovePosition : MonoBehaviour
{
    private IMovement movement;

    bool isHarvesting;

    private void Awake()
    {
        movement = GetComponent<IMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Utils.GetMouseWorldPosition();
            
            if (!isHarvesting)
            movement.MoveTo(mouseWorldPos, () =>
            {
                isHarvesting = true;

                GetComponentInChildren<IUnitGFX>().PlaySpecialAnimation(() =>
                {
                    isHarvesting = false;
                    Debug.Log($"Harvested positon: {mouseWorldPos}");
                });
            });
        }
    }
}
