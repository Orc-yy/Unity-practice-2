using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamgae;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;


    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // 출력해야하는 타워 정보를 받아와서 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true); // 타워 정보 Panel On
        UpdateTowerData(); // 타워 정보 갱신
        // 타워 오브젝트 주변에 표시되는 타워 공격범위 Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if (currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            textDamgae.text = "Damage : " + currentTower.Damage
                + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
        }
        else
        {
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);
            if (currentTower.WeaponType == WeaponType.Slow)
            {
                textDamgae.text = "Slow : " + currentTower.Slow * 100 + "%";
            }
            else if ( currentTower.WeaponType == WeaponType.Buff)
            {
                textDamgae.text = "Buff : " + currentTower.Buff * 100 + "%";
            }
        }

            imageTower.sprite = currentTower.TowerSprite;
        
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();

        // 업그레이드가 불가능해지면 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }
    public void OnClickEnventTowerUpgrade()
    {
        bool isSucces = currentTower.Upgrade();

        if(isSucces == true)
        {
            UpdateTowerData();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);

        }
        else
        {
            // 타워 업그레이드에 필요한 비용이 부족하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
        }
    }
    public void OnClickEventTowerSell()
    {
        currentTower.Sell();
        OffPanel();
    }
}
