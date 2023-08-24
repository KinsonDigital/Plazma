// <copyright file="MainWindow.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace PlazmaTesting;

using System.Drawing;
using System.Text;
using Scenes;
using Velaptor;
using Velaptor.Graphics.Renderers;
using Velaptor.UI;

/// <summary>
/// The main window of the application.
/// </summary>
public class MainWindow : Window
{
    private static readonly char[] UpperCaseChars =
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z',
    };
    private readonly Button nextButton;
    private readonly Button previousButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        TypeOfBorder = WindowBorder.Fixed;

        this.nextButton = new Button { Text = "-->" };
        this.previousButton = new Button { Text = "<--" };

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

    public static Rectangle ButtonsArea { get; private set; }

    /// <summary>
    /// Loads the applications content.
    /// </summary>
    protected override void OnLoad()
    {
        const int buttonSpacing = 15;
        const int rightMargin = 15;

        this.nextButton.Click += (_, _) => SceneManager.NextScene();

        this.previousButton.Click += (_, _) => SceneManager.PreviousScene();

        this.nextButton.LoadContent();
        this.previousButton.LoadContent();

        var buttonTops = (int)(Height - (new[] { this.nextButton.Height, this.previousButton.Height }.Max() + 20));
        var buttonGroupLeft = (int)(Width - (this.nextButton.Width + this.previousButton.Width + buttonSpacing + rightMargin));
        this.previousButton.Position = new Point(buttonGroupLeft, buttonTops);
        this.nextButton.Position = new Point(this.previousButton.Position.X + (int)this.previousButton.Width + buttonSpacing, buttonTops);

        SceneManager.LoadContent();

        var left = this.previousButton.Left;
        var right = this.nextButton.Right;
        var width = this.nextButton.Right - this.previousButton.Left;
        var height = (int)Math.Max(this.previousButton.Height, this.nextButton.Height);

        ButtonsArea = new Rectangle(left, right, width, height);

        base.OnLoad();
    }

    /// <summary>
    /// Updates the application.
    /// </summary>
    /// <param name="frameTime">The time passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        Title = $"Scene: {SceneManager.CurrentScene?.Name ?? "No Scene Loaded"}";

        this.nextButton.Update(frameTime);
        this.previousButton.Update(frameTime);

        SceneManager.Update(frameTime);
        base.OnUpdate(frameTime);
    }

    /// <summary>
    /// Renders the application.
    /// </summary>
    /// <param name="frameTime">The time passed for the current frame.</param>
    protected override void OnDraw(FrameTime frameTime)
    {
        IRenderer.Begin();

        SceneManager.Render();

        this.nextButton.Render();
        this.previousButton.Render();

        IRenderer.End();

        base.OnDraw(frameTime);
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
}
