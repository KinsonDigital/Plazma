// <copyright file="MainWindow.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting;

using System.Drawing;
using System.Text;
using KdGui;
using KdGui.Factories;
using Scenes;
using Velaptor;
using Velaptor.UI;

/// <summary>
/// The main window of the application.
/// </summary>
public class MainWindow : Window
{
    private static readonly char[] UpperCaseChars =
    [
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z',
    ];
    private readonly IControlFactory ctrlFactory;
    private IControlGroup? ctrlGroup;
    private INextPrevious? nextPrevButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        TypeOfBorder = WindowBorder.Fixed;

        this.ctrlFactory = new ControlFactory();

        var horizontalMovementScene = new HorizontalMovementScene
        {
            Name = SplitByUpperCase(nameof(HorizontalMovementScene)),
        };

        var verticalMovementScene = new VerticalMovementScene
        {
            Name = SplitByUpperCase(nameof(VerticalMovementScene)),
        };

        var colorScene = new ColorScene
        {
            Name = SplitByUpperCase(nameof(ColorScene)),
        };

        var angleScene = new AngleScene
        {
            Name = SplitByUpperCase(nameof(AngleScene)),
        };

        SceneManager.AddScene(verticalMovementScene, true);
        SceneManager.AddScene(horizontalMovementScene);
        SceneManager.AddScene(colorScene);
        SceneManager.AddScene(angleScene);
    }

    /// <summary>
    /// Loads the applications content.
    /// </summary>
    protected override void OnLoad()
    {
        this.nextPrevButton = this.ctrlFactory.CreateNextPrevious();

        this.ctrlGroup = this.ctrlFactory.CreateControlGroup();

        // NOTE: Remember, every group needs to have a different title so the
        // underlying ImGui group is unique.
        this.ctrlGroup.Title = "Prev/Next Group";
        this.ctrlGroup.Initialized += CtrlGroupOnInitialized;
        this.ctrlGroup.Width = 200;
        this.ctrlGroup.TitleBarVisible = false;
        this.ctrlGroup.AutoSizeToFitContent = true;

        this.ctrlGroup.Add(this.nextPrevButton);

        this.nextPrevButton.Next += NextPrev_OnNextClicked;
        this.nextPrevButton.Previous += NextPrev_OnPreviousClicked;

        base.OnLoad();
    }

    protected override void OnUnload()
    {
        this.ctrlGroup.Initialized -= CtrlGroupOnInitialized;
        this.nextPrevButton.Next -= NextPrev_OnNextClicked;
        this.nextPrevButton.Previous -= NextPrev_OnPreviousClicked;

        base.OnUnload();
    }

    /// <summary>
    /// Updates the application.
    /// </summary>
    /// <param name="frameTime">The time passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        Title = $"Scene: {SceneManager.CurrentScene?.Name ?? "No Scene Loaded"}";

        base.OnUpdate(frameTime);
    }

    /// <summary>
    /// Renders the application.
    /// </summary>
    /// <param name="frameTime">The time passed for the current frame.</param>
    protected override void OnDraw(FrameTime frameTime)
    {
        base.OnDraw(frameTime);

        this.ctrlGroup.Render();
    }

    /// <summary>
    /// Splits the given <param name="value"></param> based on uppercase characters.
    /// </summary>
    /// <param name="value">The value to split.</param>
    /// <returns>The value returned as a list of sections.</returns>
    private static string SplitByUpperCase(string value)
    {
        var currentSection = new StringBuilder();
        var sections = new List<string>();

        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];

            if (UpperCaseChars.Contains(character) && i != 0)
            {
                sections.Add(currentSection.ToString());

                currentSection.Clear();
                currentSection.Append(character);
            }
            else
            {
                currentSection.Append(character);
            }
        }

        sections.Add(currentSection.ToString());

        var result = sections.Aggregate(string.Empty, (current, section) => current + $"{section} ");

        return result.TrimEnd(' ');
    }

    private void CtrlGroupOnInitialized(object? sender, EventArgs e) =>
        this.ctrlGroup.Position = new Point((int)Width - (this.ctrlGroup.Width + 15), (int)Height - (this.ctrlGroup.Height + 15));

    private void NextPrev_OnNextClicked(object? sender, EventArgs e) => SceneManager.NextScene();

    private void NextPrev_OnPreviousClicked(object? sender, EventArgs e) => SceneManager.PreviousScene();
}
