using Unity.VisualScripting;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;
    private void Awake()
    {
        towerWeapon = GetComponent<TowerWeapon>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        Movement2D movement2D = collision.GetComponent<Movement2D>();
        // �̵��ӵ� = �̵��ӵ� - �̵��ӵ� * ���ӷ�;
        movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }

        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
