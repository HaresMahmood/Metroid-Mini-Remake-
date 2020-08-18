using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SideScrollingCameraController : MonoBehaviour
{
    #region Variables

    #region Inspector Fields

    [Header("Setup")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] [Range(1f, 10f)] private float moveSpeed  = 5f;

    #endregion

    private Vector3 velocity;

    #endregion

    #region Unity Methods

    private void LateUpdate()
    {
        if (target)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, target.position.y, target.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

    }

    #endregion
}

