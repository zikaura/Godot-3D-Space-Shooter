using Godot;
using System;

public class Enemy : KinematicBody
{
    private double spd = GD.RandRange(20, 50);

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(new Vector3(0, 0, (float)spd));
        if (Transform.origin.z > 10) QueueFree();
    }
}
