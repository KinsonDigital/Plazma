// <copyright file="MinMax.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting.CustomControls;

using System.Drawing;
using KdGui;
using KdGui.Factories;

public class MinMax
{
    private readonly IControlGroup group;
    private readonly ICheckBox randomize;
    private readonly ISlider value;
    private readonly ISlider randomMin;
    private readonly ISlider randomMax;
    private readonly ICheckBox lockRanges;
    private readonly ICheckBox growShrink;

    public MinMax(string title, string minText = "Min", string maxText = "Max")
    {
        var ctrlFactory = new ControlFactory();

        this.randomMin = ctrlFactory.CreateSlider();
        this.randomMin.Text = minText;
        this.randomMin.Min = 0;
        this.randomMin.Max = 100;
        this.randomMin.Value = 50;
        this.randomMin.ValueChanged += RandomMin_OnValueChanged;
        this.randomMin.Visible = false;

        this.randomMax = ctrlFactory.CreateSlider();
        this.randomMax.Text = maxText;
        this.randomMax.Min = 0;
        this.randomMax.Max = 100;
        this.randomMax.Value = 50;
        this.randomMax.ValueChanged += RandomMax_OnValueChanged;
        this.randomMax.Visible = false;

        this.randomize = ctrlFactory.CreateCheckbox();
        this.randomize.IsChecked = false;
        this.randomize.LabelWhenChecked = "Randomized";
        this.randomize.LabelWhenUnchecked = "Not Randomized";
        this.randomize.CheckedChanged += Randomize_OnCheckedChanged;

        this.growShrink = ctrlFactory.CreateCheckbox();
        this.growShrink.IsChecked = true;
        this.growShrink.Visible = true;
        this.growShrink.LabelWhenChecked = "Growing";
        this.growShrink.LabelWhenUnchecked = "Shrinking";

        this.value = ctrlFactory.CreateSlider();
        this.value.Text = "Value";
        this.value.Visible = true;
        this.value.Min = 0;
        this.value.Max = 100;
        this.value.Value = 100;

        this.lockRanges = ctrlFactory.CreateCheckbox();
        this.lockRanges.IsChecked = true;
        this.lockRanges.Visible = false;
        this.lockRanges.LabelWhenChecked = "Locked";
        this.lockRanges.LabelWhenUnchecked = "Unlocked";
        this.lockRanges.CheckedChanged += LockRanges_OnCheckedChanged;

        this.group = ctrlFactory.CreateControlGroup();
        this.group.AutoSizeToFitContent = true;
        this.group.Title = title;
        this.group.Add(this.randomize);
        this.group.Add(this.lockRanges);
        this.group.Add(this.randomMin);
        this.group.Add(this.randomMax);
        this.group.Add(this.growShrink);
        this.group.Add(this.value);
        this.group.Render();
    }

    public string MinText
    {
        get => this.randomMin.Text;
        set => this.randomMin.Text = value;
    }

    public string MaxText
    {
        get => this.randomMax.Text;
        set => this.randomMax.Text = value;
    }

    public float Min
    {
        get => this.randomMin.Value;
        set
        {
            this.randomMin.Min = value;
            this.randomMax.Min = value;
        }
    }

    public float Max
    {
        get => this.randomMax.Value;
        set
        {
            this.randomMin.Max = value;
            this.randomMax.Max = value;
        }
    }

    public float MinValue
    {
        get => this.randomMin.Value;
        set => this.randomMin.Value = value;
    }

    public float MaxValue
    {
        get => this.randomMax.Value;
        set => this.randomMax.Value = value;
    }

    public Point Position
    {
        get => this.group.Position;
        set => this.group.Position = value;
    }

    public float Value
    {
        get => this.value.Value;
        set => this.value.Value = value;
    }

    public bool GrowValue => this.growShrink.IsChecked;

    public bool IsRandomized => this.randomize.IsChecked;

    public bool IsNotRandomized => !this.randomize.IsChecked;

    public float Left => this.group.Left;

    public float Top => this.group.Top;

    public float Right => this.group.Right;

    public float Bottom => this.group.Bottom;

    public float Width => this.group.Width;

    public float Height => this.group.Height;

    public void Render() => this.group.Render();

    private void Randomize_OnCheckedChanged(object? sender, bool e)
    {
        this.growShrink.Visible = !this.growShrink.Visible;
        this.value.Visible = !this.value.Visible;
        this.randomMin.Visible = !this.randomMin.Visible;
        this.randomMax.Visible = !this.randomMax.Visible;
        this.lockRanges.Visible = !this.lockRanges.Visible;
    }

    private void RandomMin_OnValueChanged(object? sender, float e)
    {
        if (this.lockRanges.IsChecked)
        {
            this.randomMax.Value = e;
        }
    }

    private void RandomMax_OnValueChanged(object? sender, float e)
    {
        if (this.lockRanges.IsChecked)
        {
            this.randomMin.Value = e;
        }
    }

    private void LockRanges_OnCheckedChanged(object? sender, bool e)
    {
        if (!e)
        {
            return;
        }

        if (Math.Abs(this.randomMin.Value - this.randomMax.Value) < 0.0001)
        {
            return;
        }

        var minValue = Math.Min(this.randomMin.Value, this.randomMax.Value);
        var maxValue = Math.Max(this.randomMax.Value, this.randomMax.Value);
        var delta = maxValue - minValue;

        var newMinValue = minValue + (delta / 2f);
        var newMaxValue = maxValue - (delta / 2f);

        this.randomMin.Value = newMinValue;
        this.randomMax.Value = newMaxValue;
    }
}
