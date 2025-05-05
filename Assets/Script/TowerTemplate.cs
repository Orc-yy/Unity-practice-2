using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab; // 타워 생성을 위한 프리팹
    public GameObject followTowerPrefab;
    public Weapon[] weapon; // 레벨별 타워(무기) 정보

    // Tip 클래스 내부에 구조체를 만들면 클래스 외부에서는 구조체 변수를 선언할 수 없다.
    // 권장하는 방법은 변수를 코드에서 조작하지 못하도록 모두 private로 설정하고
    // 모든 변수에 접근할 수 있는 프로퍼티를 제작
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; // 보여지는 타워 이미지
        public float damage;
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
