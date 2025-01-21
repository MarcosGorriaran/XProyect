using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HPBar : MonoBehaviour
{
    public HPManager trackedHp;
    private Slider _slider;

    private void Start()
    {
        trackedHp.onHPChange += UpdateBar;
        _slider = GetComponent<Slider>();
        _slider.maxValue = trackedHp.GetMaxHp();
        _slider.minValue = 0;
        _slider.value = trackedHp.GetHp();
    }
    private void UpdateBar(float damage)
    {
        _slider.value = trackedHp.GetHp();
    }
}
