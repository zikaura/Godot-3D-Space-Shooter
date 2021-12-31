using Godot;
using System;

public class Bullet : KinematicBody
{
    private Node main;
    private Vector3 velo = new Vector3();
    private AudioStreamPlayer explodeSound;
    private PackedScene killParticles = GD.Load<PackedScene>("res://KillParticles.tscn");

    public override void _Ready()
    {
        var sceneTree = GetTree();
        main = sceneTree.CurrentScene;
        explodeSound = GetNode<AudioStreamPlayer>("EnemyExplode");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(velo);
    }

    public void _OnAreaBodyEntered(Node body)
    {
        if (body.IsInGroup("Enemies"))
        {
            var particles = killParticles.Instance<Particles>();
            main.AddChild(particles);

            var newParticleTransform = particles.Transform;
            newParticleTransform.origin = Transform.origin;
            particles.Transform = newParticleTransform;

            body.QueueFree();
            explodeSound.Play();
            Visible = false;

            GetNode<CollisionShape>("Area/CollisionShape").Disabled = true;
        }
    }

    public void _OnLightTimerTimeout()
    {
        GetNode<OmniLight>("OmniLight").Visible = false;
    }
}

