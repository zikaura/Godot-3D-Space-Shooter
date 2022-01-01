using Godot;
using System;

public class EnemySpawner : Spatial
{
    private Node main;
    private PackedScene Enemy = GD.Load<PackedScene>("res://enemy.tscn");

    public override void _Ready()
    {
        main = GetTree().CurrentScene;
    }

    public void Spawn()
    {
        KinematicBody enemy = Enemy.Instance<KinematicBody>();
        main.AddChild(enemy);
        var newEnemyTransform = enemy.Transform;
        var newEnemyOrigin = Transform.origin + new Vector3(
            (float) GD.RandRange(-15, 15),
            (float) GD.RandRange(-10, 10), 0);
        newEnemyTransform.origin = newEnemyOrigin;
        enemy.Transform = newEnemyTransform;
    }

    public void _OnTimerTimeout()
    {
        Spawn();
    }

}

