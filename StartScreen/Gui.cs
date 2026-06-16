using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class Gui : Control
{
	private const int WindowHeight = 648;
	private const int WindowWidth = 1152;
	private int CurrentWindowHeight = 648;
	private int CurrentWindowWidth = 1152;
	[Export]public Control[] ControlItems = new Control[] {};
	[Export]public Node2D[] Node2DItems = new Node2D[] {};
	private Dictionary<Object, Vector2> ItemScales = new Dictionary<object, Vector2>();
	private Dictionary<Object, Vector2> ItemPositions = new Dictionary<object, Vector2>();
	private PackedScene SettingsScene = GD.Load<PackedScene>("res://Settings/SettingsMenu.tscn");
	private PackedScene Game = GD.Load<PackedScene>("res://Game/Game.tscn");
	private PackedScene PauseMenu = GD.Load<PackedScene>("res://PauseMenu/PauseMenu.tscn");
	private Button PlayButton;
	private Button QuitButton;
	private Button SettingsButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(var item in this.GetChildren())
        {
            if (item is Control controlItem)
			{
				this.ControlItems.Append(controlItem);
			}
			else if (item is Node2D node2DItem)
			{
				this.Node2DItems.Append(node2DItem);
			}
        }
		this.PlayButton = this.GetNode<Button>("PlayButton");
		this.QuitButton = this.GetNode<Button>("QuitButton");
		this.SettingsButton = this.GetNode<Button>("SettingsButton");

		this.QuitButton.Pressed += this.Quit;

		this.SetDefaultScales();
		this.SetDefaultPositions();
		this.GetTree().Root.GetWindow().SizeChanged += UpdateGuiConfig;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SetDefaultScales()
	{
		foreach(var item in this.ControlItems)
		{
			this.ItemScales.Add(item, item.Scale);
		}
		foreach(var item in this.Node2DItems)
		{
			this.ItemScales.Add(item, item.Scale);
		}
	}

	public void SetDefaultPositions()
	{
		foreach(var item in this.ControlItems)
		{
			this.ItemPositions.Add(item, item.Position);
		}
		foreach(var item in this.Node2DItems)
		{
			this.ItemPositions.Add(item, item.Position);
		}
	}

	public void UpdateGuiConfig()
	{
		Vector2I WindowSize = this.GetTree().Root.GetWindow().Size;
		this.CurrentWindowWidth = WindowSize[0];
		this.CurrentWindowHeight = WindowSize[1];
		
		Vector2 Scale = new Vector2((float)this.CurrentWindowWidth/(float)WindowWidth, (float)this.CurrentWindowHeight/(float)WindowHeight);
		foreach(var key in this.ItemScales.Keys)
		{
			if (this.ControlItems.Contains(key))
			{
				Control item = (Control) key;
				item.Scale = this.ItemScales[key] * Scale;
				item.Position = this.ItemPositions[key] * Scale;
			}
			else if (this.Node2DItems.Contains(key))
			{
				Node2D item = (Node2D) key;
				item.Scale = this.ItemScales[key] * Scale;
				item.Position = this.ItemPositions[key] * Scale;
			}
		}
	}

	public void Quit()
    {
        this.GetTree().Quit();
    }

	public void OpenSettingsMenu()
    {
        this.AddChild(this.SettingsScene.Instantiate());
    }

	public void PlayGame()
    {
        this.GetTree().ChangeSceneToPacked(this.Game);
    }
}
