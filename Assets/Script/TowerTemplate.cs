using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab; // Ÿ�� ������ ���� ������
    public GameObject followTowerPrefab;
    public Weapon[] weapon; // ������ Ÿ��(����) ����

    // Tip Ŭ���� ���ο� ����ü�� ����� Ŭ���� �ܺο����� ����ü ������ ������ �� ����.
    // �����ϴ� ����� ������ �ڵ忡�� �������� ���ϵ��� ��� private�� �����ϰ�
    // ��� ������ ������ �� �ִ� ������Ƽ�� ����
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; // �������� Ÿ�� �̹���
        public float damage;
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
