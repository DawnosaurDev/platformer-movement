using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerMovement mov;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private DemoManager demoManager;

    [Header("Movement Tilt")]
    [SerializeField] private float maxTilt;
    [SerializeField] [Range(0, 1)] private float tiltSpeed;

    [Header("Particle FX")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;
    private ParticleSystem _jumpParticle;
    private ParticleSystem _landParticle;

    public bool startedJumping {  private get; set; }
    public bool justLanded { private get; set; }

    public float currentVelY;

    private void Start()
    {
        mov = GetComponent<PlayerMovement>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        anim = spriteRend.GetComponent<Animator>();

        demoManager = FindObjectOfType<DemoManager>();

        _jumpParticle = jumpFX.GetComponent<ParticleSystem>();
        _landParticle = landFX.GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        #region Tilt
        float tiltProgress;

        int mult = -1;

        if (mov.IsSliding)
        {
            tiltProgress = 0.25f;
        }
        else
        {
            tiltProgress = Mathf.InverseLerp(-mov.Data.runMaxSpeed, mov.Data.runMaxSpeed, mov.RB.velocity.x);
            mult = (mov.IsFacingRight) ? 1 : -1;
        }
            
        float newRot = ((tiltProgress * maxTilt * 2) - maxTilt);
        float rot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, tiltSpeed);
        spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);
        #endregion

        CheckAnimationState();

        ParticleSystem.MainModule jumpPSettings = _jumpParticle.main;
        jumpPSettings.startColor = new ParticleSystem.MinMaxGradient(demoManager.SceneData.foregroundColor);
        ParticleSystem.MainModule landPSettings = _landParticle.main;
        landPSettings.startColor = new ParticleSystem.MinMaxGradient(demoManager.SceneData.foregroundColor);
    }

    private void CheckAnimationState()
    {
        if (startedJumping)
        {
            anim.SetTrigger("Jump");
            GameObject obj = Instantiate(jumpFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            startedJumping = false;
            return;
        }

        if (justLanded)
        {
            anim.SetTrigger("Land");
            GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            Destroy(obj, 1);
            justLanded = false;
            return;
        }

        anim.SetFloat("Vel Y", mov.RB.velocity.y);
    }
}
