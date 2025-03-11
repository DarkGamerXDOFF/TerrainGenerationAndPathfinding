using UnityEngine;
using System.Collections.Generic;

public class UnitHandler : MonoBehaviour
{
    
    private Vector2 startPosition;

    [SerializeField] private Transform selectionAreaTransform;
    [SerializeField] private List<UnitRTS> selectedUnitRTSList;

    [SerializeField] private int baseUnitCount = 6;
    [SerializeField] private float baseRingDistance = 1.5f;


    private void Awake()
    {
        selectedUnitRTSList = new List<UnitRTS>();
        selectionAreaTransform.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Left mouse button pressed
            startPosition = Utils.GetMouseWorldPosition();
            selectionAreaTransform.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            //Left mouse button held down
            Vector2 currentPosition = Utils.GetMouseWorldPosition();
            Vector2 lowerLeft = new Vector2(
                Mathf.Min(startPosition.x, currentPosition.x),
                Mathf.Min(startPosition.y, currentPosition.y)
                );
            Vector2 upperRight = new Vector2(
                Mathf.Max(startPosition.x, currentPosition.x),
                Mathf.Max(startPosition.y, currentPosition.y)
                );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Left mouse button released
            selectionAreaTransform.gameObject.SetActive(false);

            Vector2 endPosition = Utils.GetMouseWorldPosition(); 
            Collider2D[] colliders = Physics2D.OverlapAreaAll(startPosition, endPosition);

            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetSelectedVisible(false);
            }
            selectedUnitRTSList.Clear();
            
            foreach (Collider2D collider in colliders)
            {
                UnitRTS unitRTS = collider.GetComponent<UnitRTS>();

                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                }
            }

            //Debug.Log(selectedUnitRTSList.Count);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 moveToPosition = Utils.GetMouseWorldPosition();

            CalcRingSizeAndAmount(
                CalculateRingAmount(selectedUnitRTSList.Count), 
                out float[] ringDistances, 
                out int[] ringUnitAmounts
                );

            List<Vector2> targetPositionList = GetPositionListAround(moveToPosition, ringDistances, ringUnitAmounts);

            int targetPositionIndex = 0;
            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.MoveTo(targetPositionList[targetPositionIndex]);
                targetPositionIndex = (targetPositionIndex + 1) % targetPositionList.Count;
            }
        }
    }
    private List<Vector2> GetPositionListAround(Vector2 startPosition, float[] ringDistanceArray, int[] ringPostionCountArray)
    {
        List<Vector2> positionList = new List<Vector2>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPostionCountArray[i]));
        }

        return positionList;
    }
    private List<Vector2> GetPositionListAround(Vector2 startPosition, float distance, int positionsCount)
    {
        List<Vector2> positionsList = new List<Vector2>();
        
        for (int i = 0; i < positionsCount; i++)
        {
            float angle = i * (360f / positionsCount);
            Vector2 dir = ApplyRotationToVector(new Vector2(1, 0), angle);
            Vector2 position = startPosition + dir * distance;
            positionsList.Add(position);
        }

        return positionsList;
    }

    private Vector2 ApplyRotationToVector(Vector2 vec, float angle) => Quaternion.Euler(0, 0, angle) * vec;

    private int CalculateRingAmount(int unitCount)
    {
        if (unitCount <= 1)
            return 0; // No rings needed if there's only one unit

        int ringCount = 0;
        int remainingUnits = unitCount - 1; // Subtracting the center unit
        int unitsInRing = baseUnitCount; // The first ring starts with 6 units

        while (remainingUnits > 0)
        {
            ringCount++;
            remainingUnits -= unitsInRing;
            unitsInRing += baseUnitCount; // Each new ring has 6 more units than the previous one
        }

        return ringCount;
    }

    private void CalcRingSizeAndAmount(int ringCount, out float[] ringDistances, out int[] unitsPerRing)
    {
        ringDistances = new float[ringCount];
        unitsPerRing = new int[ringCount];

        for (int i = 0; i < ringCount; i++)
        {
            ringDistances[i] = baseRingDistance * (i + 1);
            unitsPerRing[i] = baseUnitCount * (i + 1);
        }
    }
}
