using UnityEngine;
using UnityEngine.XR;

// Coloque nos objetos "Left Hand" e "Right Hand". Detecta o soco pelo movimento real da mão
// (velocidade), sem botão, e aplica um dano fixo no slime atingido.
[RequireComponent(typeof(Rigidbody))]
public class PunchHand : MonoBehaviour
{
    [Header("Soco")]
    [Tooltip("Dano fixo por soco.")]
    [SerializeField] private int damage = 1;
    [Tooltip("Velocidade mínima da mão (m/s) para contar como soco.")]
    [SerializeField] private float minPunchSpeed = 1.5f;

    [Header("Collider da mão (criado automaticamente se não houver)")]
    [SerializeField] private float colliderRadius = 0.08f;
    [SerializeField] private Vector3 colliderCenter = Vector3.zero;

    [Header("Vibração")]
    [SerializeField] private XRNode handNode = XRNode.RightHand;
    [Range(0f, 1f)]
    [SerializeField] private float hapticAmplitude = 0.6f;
    [SerializeField] private float hapticDuration = 0.1f;

    public float Speed => velocity.magnitude;

    private Vector3 lastLocalPosition;
    private Vector3 velocity; // em coordenadas do mundo, sem o movimento de locomoção do jogador

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        if (GetComponent<Collider>() == null)
        {
            SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = colliderRadius;
            sphere.center = colliderCenter;
        }

        lastLocalPosition = RigSpacePosition();
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        if (dt <= 0f) return;

        // Mede a velocidade no espaço do rig (pai), assim andar/girar com o joystick não conta como soco.
        Vector3 current = RigSpacePosition();
        Vector3 localVelocity = (current - lastLocalPosition) / dt;
        lastLocalPosition = current;

        Vector3 worldVelocity = transform.parent != null
            ? transform.parent.TransformVector(localVelocity)
            : localVelocity;

        velocity = Vector3.Lerp(velocity, worldVelocity, 0.6f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Slime slime = other.GetComponentInParent<Slime>();
        if (slime == null) return;
        if (Speed < minPunchSpeed) return;

        if (slime.TakeHit(damage, velocity))
            Vibrate();
    }

    private Vector3 RigSpacePosition()
    {
        return transform.parent != null
            ? transform.parent.InverseTransformPoint(transform.position)
            : transform.position;
    }

    private void Vibrate()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);
        if (device.isValid)
            device.SendHapticImpulse(0u, hapticAmplitude, hapticDuration);
    }
}
