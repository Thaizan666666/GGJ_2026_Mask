using UnityEngine;

public class MoveToZero : MonoBehaviour
{
    public GameObject targetObject;

    [Header("Movement Settings")]
    public float targetY = 0f; // ตำแหน่ง Y ที่ต้องการ
    public float moveSpeed = 5f;

    private void Update()
    {
        if (targetObject != null)
        {
            Vector3 currentPos = targetObject.transform.position;

            // เคลื่อนที่ลงล่าง (แกน Y) จนถึง 0
            if (currentPos.y > targetY)
            {
                currentPos.y -= moveSpeed * Time.deltaTime;

                // ไม่ให้ต่ำกว่า target
                if (currentPos.y < targetY)
                {
                    currentPos.y = targetY;
                }

                targetObject.transform.position = currentPos;
            }
            else
            {
                currentPos.y = targetY;
                targetObject.transform.position = currentPos;
                Debug.Log("Reached Y = 0!");
                this.enabled = false;
            }
        }
    }
}