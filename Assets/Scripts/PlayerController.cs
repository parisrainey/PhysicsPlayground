using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ToggleRagdoll(false);
        Invoke(nameof(SetRagdollOn), 3);
    }

    private void FixedUpdate()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        float acceleration = 5;
        float maxSpeed = 1;

        if (movementInput.magnitude > 0.1f)
            _rigidbody.AddForce(movementInput * acceleration, ForceMode.VelocityChange);

        Vector2 horizontalClampedVelocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);
        horizontalClampedVelocity = horizontalClampedVelocity.normalized * maxSpeed;
        if (movementInput.magnitude == 0)
            horizontalClampedVelocity = Vector2.zero;
        Vector3 clampedVelocity = new Vector3(horizontalClampedVelocity.x, _rigidbody.velocity.y, horizontalClampedVelocity.y);
        _rigidbody.velocity = clampedVelocity;

    }
    private void SetRagdollOn() => ToggleRagdoll(true);
    public void ToggleRagdoll(bool enabled)
    {
        TryGetComponent(out Collider collider);
        if (collider)
            collider.enabled = !enabled;

        Animator anim = GetComponentInChildren<Animator>(true);
        if (anim)
            anim.enabled = !enabled;

        foreach (var item in GetComponentsInChildren<Rigidbody>(true))
            if (item != _rigidbody)
            {
                item.isKinematic = !enabled;
                item.velocity = _rigidbody.velocity;
            }
        foreach (var item in GetComponentsInChildren<Collider>(true))
            if (item != collider)
                item.enabled = enabled;

        _rigidbody.isKinematic = enabled;
    }
}