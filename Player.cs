using Godot;
using System;

public class Player : KinematicBody
{
    private const int MAX_SPEED = 300;
    private const float COOLDOWN = 8.0f;
    private const float ACCELERATION = 0.75f;

    private float cooldown = 0.0f;
    private Vector3 velocity = new Vector3();
    private Vector3 inputVector = new Vector3();

    private Node main;
    private Particles particles;
    private PackedScene _bullet;
    private Spatial[] guns = new Spatial[2];

    public override void _Ready()
    {
        guns[0] = GetNode<Spatial>("Gun0");
        guns[1] = GetNode<Spatial>("Gun1");
        particles = GetNode<Particles>("Particles");
        _bullet = GD.Load<PackedScene>("res://Bullet.tscn");

        particles.Emitting = true;
        main = GetTree().CurrentScene;
    }

    public override void _PhysicsProcess(float delta)
    {
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        inputVector.y = Input.GetActionStrength("ui_up") - Input.GetActionStrength("ui_down");
        inputVector = inputVector.Normalized();

        velocity.x = Mathf.MoveToward(velocity.x, inputVector.x * MAX_SPEED, ACCELERATION);
        velocity.y = Mathf.MoveToward(velocity.y, inputVector.y * MAX_SPEED, ACCELERATION);
        
        Vector3 prevPlayerRot = RotationDegrees;
        prevPlayerRot.z = velocity.x * -2;
        prevPlayerRot.x = velocity.y * 0.5f;
        prevPlayerRot.y = velocity.x * -0.5f;
        RotationDegrees = prevPlayerRot;
        
        velocity = MoveAndSlide(velocity);

        Transform prevTransform = Transform;
        prevTransform.origin.x = Mathf.Clamp(prevTransform.origin.x, -15, 15);
        prevTransform.origin.y = Mathf.Clamp(prevTransform.origin.y, -10, 10);
        Transform = prevTransform;

        // Shooting
        if (Input.IsActionPressed("ui_accept") && cooldown <= 0)
        {
            cooldown = COOLDOWN * delta;
            foreach (Spatial gun in guns)
            {
                KinematicBody bullet = _bullet.Instance<KinematicBody>();
                main.AddChild(bullet);
                bullet.Transform = gun.GlobalTransform;
                bullet.Set("velo", bullet.Transform.basis.z * -600);
            }
        }

        // Cooldown
        if (cooldown > 0) cooldown -= delta;
    }
    
}
