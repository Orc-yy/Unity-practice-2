using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    [SerializeField]
    private EnemySpawner enemySpawner; // 현재 맵에 존재하는 적 리스트 정보를 얻기 위해..
    [SerializeField]
    private PlayerGold playerGold; // 건설하고 골드 감소용
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    private int towerType;
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        if (isOnTowerButton == true)
        {
            return;
        }
        // 타워 건설 가능 여부 확인
        // 타워를 건설할 만큼 돈이 없으면 타워 건설 x
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        isOnTowerButton = true;

        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        // 타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        // 타워 건설 여부 확인
        // 1. 타워를 건설할 만큼 돈이 없으면 타워 건설 X
        if (isOnTowerButton == false)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        //타워 건설 가능 여부 확인
        // 1. 현재 타일의 위치에 이미 타워가 건설되어 있다면 타워 건설 X
        if (tile.IsBuildTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        // 다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
        isOnTowerButton = false;
        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        // 타워가 건설에 필요한 골드만큼 감소
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // 선택한 타일의 위치에 타워 건설
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        // 타워 무기에 enemySpawner 정보 전달 
        clone.GetComponent<TowerWeapon>().Setup(this,enemySpawner, playerGold, tile);

        // 새로 배치되는 타워가 버프 타워 주변에 배치될 경우
        // 버프  효과를 받을 수 있도록 모든 버프 타워의 버프 효과 갱신
        OnBuffAllBuffTowers();

        // 타워를 배치했기 때문에 마우스를 따라다니는 임시 타워 삭제
        Destroy(followTowerClone);
        // 타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                // 마우스를 따라다니는 임시 타워 삭제
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }
    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
