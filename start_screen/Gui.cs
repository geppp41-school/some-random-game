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
	Dictionary<Object, Vector2> ItemScales = new Dictionary<object, Vector2>();
	Dictionary<Object, Vector2> ItemPositions = new Dictionary<object, Vector2>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
}
