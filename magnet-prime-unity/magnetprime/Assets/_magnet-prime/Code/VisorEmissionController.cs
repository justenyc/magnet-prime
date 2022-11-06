using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using DG.Tweening;

public class VisorEmissionController : MonoBehaviour
{
    [Header("Targets")]
    public MeshRenderer helmetMeshRenderer;
    public MeshRenderer glassMeshRenderer;

    [Header("Properties")]
    public Color positiveColor = Color.red;
    public Color negativeColor = Color.blue;
    public Material mat;
    public Material visorMat;
    public Material damageIndicatorMat;
    // Start is called before the first frame update
    void Start()
    {
        FirstPersonController fpc = FindObjectOfType<FirstPersonController>();
        Health playerHealth = fpc.GetComponent<Health>();

        fpc.PolarityChanged += ChangeColor;
        playerHealth.TakeDamageAction += IndicateDamage;
        playerHealth.DieAction += VisorDeathEffect;

        mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.EnableKeyword("_BaseColor");
        ChangeColor(fpc.polarity);
    }

    void ChangeColor(int polarity)
    {
        if(polarity == 1)
        {
            mat.SetColor("_BaseColor", positiveColor);
            return;
        }
        mat.SetColor("_BaseColor", negativeColor);
    }

    public void IndicateDamage(float modifier = 0)
    {
        Color damageColor = new Color(0.5f, 0, 0.5f);
        Color lerpedDamageColor = new Color(
            ((1 - damageColor.r) * modifier) + damageColor.r,
            ((1 - damageColor.g) * modifier) + damageColor.g,
            ((1 - damageColor.b) * modifier) + damageColor.b);

        helmetMeshRenderer.materials[1].DOKill();
        helmetMeshRenderer.materials[1].SetFloat("Vector1_3871237419634dc7a79a977f78891bb7", 1 - modifier);
        helmetMeshRenderer.materials[1].SetColor("Main_Color", lerpedDamageColor);

        glassMeshRenderer.materials[1].DOKill();
        glassMeshRenderer.materials[1].SetFloat("Vector1_3871237419634dc7a79a977f78891bb7", 1 - modifier);
        glassMeshRenderer.materials[1].SetColor("Main_Color", lerpedDamageColor);
    }

    void VisorDeathEffect()
    {

    }
}
