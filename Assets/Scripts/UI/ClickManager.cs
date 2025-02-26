using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    public LayerMask crewLayer;
    public LayerMask facilityLayer;

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) // Si se hace click izquierdo
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Primero, intenta detectar un tripulante
                Collider2D hitCrew = Physics2D.OverlapPoint(mousePosition, crewLayer);
                if (hitCrew != null)
                {
                    hitCrew.GetComponent<CMBehaviour>()?.OnClick();
                    return; // No sigue detectando si encontró un tripulante
                }

                // Si no hay tripulante, detecta la facility
                Collider2D hitFacility = Physics2D.OverlapPoint(mousePosition, facilityLayer);
                if (hitFacility != null)
                {
                    hitFacility.GetComponent<FCBehaviour>()?.OnClick();
                }
            }


        }
    }
}
