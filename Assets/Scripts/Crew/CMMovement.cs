using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMMovement : MonoBehaviour
{
    public float speed;

    public Animator animator;

    private Vector2 targetPosition;
    // Start is called before the first frame update

    void Awake()
    {
        targetPosition = transform.position;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if ((Vector2)transform.position != targetPosition)
        {
            // Convertimos targetPosition a Vector3 para que coincida con transform.position
            Vector3 targetPos3D = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

            // Calculamos la dirección hacia la posición objetivo
            Vector3 direction = (targetPos3D - transform.position).normalized;

            // Movemos el objeto hacia la posición objetivo
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f);

            // Animamos el movimiento
            AnimateMovement(direction);
        }
        else
        {
            // Si hemos llegado al objetivo, detenemos la animación
            AnimateMovement(Vector3.zero);
        }
    }

    void AnimateMovement(Vector3 direction)
    {
        if (animator != null)
        {
            if (direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    public void setTargetPosition(Vector2 position)
    {
        this.targetPosition = position;
    }
}
