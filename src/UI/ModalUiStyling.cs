using Godot;

namespace MultiplayerSaveSlots.UI;

internal static class ModalUiStyling
{
    private static readonly Color PanelBackground = new(0.07f, 0.045f, 0.035f, 0.98f);
    private static readonly Color PanelBorder = new(0.76f, 0.52f, 0.22f, 1f);
    private static readonly Color ButtonBackground = new(0.16f, 0.10f, 0.07f, 1f);
    private static readonly Color ButtonHoverBackground = new(0.24f, 0.15f, 0.09f, 1f);
    private static readonly Color ButtonPressedBackground = new(0.10f, 0.065f, 0.05f, 1f);
    private static readonly Color FocusBorder = new(0.97f, 0.78f, 0.26f, 1f);
    private static readonly Color TitleText = new(0.96f, 0.78f, 0.30f, 1f);
    private static readonly Color BodyText = new(0.98f, 0.94f, 0.84f, 1f);
    private static readonly Color PreviewBackground = new(0.10f, 0.06f, 0.045f, 0.92f);

    public static void PrepareModalRoot(Control root)
    {
        root.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        root.OffsetLeft = 0;
        root.OffsetTop = 0;
        root.OffsetRight = 0;
        root.OffsetBottom = 0;
        root.MouseFilter = Control.MouseFilterEnum.Stop;
        root.ZAsRelative = false;
        root.ZIndex = 100;
    }

    public static PanelContainer CreatePanel(Vector2 size, float halfWidth, float halfHeight)
    {
        var panel = new PanelContainer
        {
            CustomMinimumSize = size,
            MouseFilter = Control.MouseFilterEnum.Stop,
            ZIndex = 101
        };
        panel.SetAnchorsPreset(Control.LayoutPreset.Center);
        panel.OffsetLeft = -halfWidth;
        panel.OffsetTop = -halfHeight;
        panel.OffsetRight = halfWidth;
        panel.OffsetBottom = halfHeight;
        panel.AddThemeStyleboxOverride("panel", CreatePanelStyle());
        return panel;
    }

    public static void StyleTitle(Label label)
    {
        label.AddThemeColorOverride("font_color", TitleText);
        label.AddThemeColorOverride("font_shadow_color", new Color(0f, 0f, 0f, 0.45f));
        label.AddThemeConstantOverride("shadow_offset_x", 3);
        label.AddThemeConstantOverride("shadow_offset_y", 2);
        label.AddThemeFontSizeOverride("font_size", 34);
    }

    public static void StyleBody(Label label, int fontSize = 22)
    {
        label.AddThemeColorOverride("font_color", BodyText);
        label.AddThemeColorOverride("font_shadow_color", new Color(0f, 0f, 0f, 0.35f));
        label.AddThemeConstantOverride("shadow_offset_x", 2);
        label.AddThemeConstantOverride("shadow_offset_y", 1);
        label.AddThemeFontSizeOverride("font_size", fontSize);
    }

    public static void StyleButton(Button button)
    {
        button.AddThemeStyleboxOverride("normal", CreateButtonStyle(ButtonBackground, PanelBorder, 2));
        button.AddThemeStyleboxOverride("hover", CreateButtonStyle(ButtonHoverBackground, FocusBorder, 2));
        button.AddThemeStyleboxOverride("pressed", CreateButtonStyle(ButtonPressedBackground, FocusBorder, 2));
        button.AddThemeStyleboxOverride("focus", CreateButtonStyle(new Color(0f, 0f, 0f, 0f), FocusBorder, 3));
        button.AddThemeStyleboxOverride("disabled", CreateButtonStyle(new Color(0.08f, 0.08f, 0.08f, 0.7f), PanelBorder, 1));
        button.AddThemeColorOverride("font_color", BodyText);
        button.AddThemeColorOverride("font_hover_color", BodyText);
        button.AddThemeColorOverride("font_pressed_color", BodyText);
        button.AddThemeColorOverride("font_focus_color", BodyText);
        button.AddThemeFontSizeOverride("font_size", 22);
        button.FocusMode = Control.FocusModeEnum.All;
    }

    public static void StyleSelectedButton(Button button)
    {
        StyleButton(button);
        button.AddThemeStyleboxOverride("normal", CreateButtonStyle(ButtonBackground, FocusBorder, 3));
        button.AddThemeColorOverride("font_color", BodyText);
    }

    public static void StyleBadgePanel(PanelContainer panel)
    {
        panel.AddThemeStyleboxOverride("panel", CreateBadgeStyle());
    }

    public static void StyleIconSlotPanel(PanelContainer panel)
    {
        panel.AddThemeStyleboxOverride("panel", CreateTransparentStyle());
    }

    public static void StylePreviewFrame(PanelContainer panel)
    {
        panel.AddThemeStyleboxOverride("panel", CreateButtonStyle(PreviewBackground, PanelBorder, 2));
    }

    public static void StyleSectionTitle(Label label)
    {
        label.AddThemeColorOverride("font_color", TitleText);
        label.AddThemeColorOverride("font_shadow_color", new Color(0f, 0f, 0f, 0.35f));
        label.AddThemeConstantOverride("shadow_offset_x", 2);
        label.AddThemeConstantOverride("shadow_offset_y", 1);
        label.AddThemeFontSizeOverride("font_size", 20);
    }

    private static StyleBoxFlat CreatePanelStyle()
    {
        var style = new StyleBoxFlat
        {
            BgColor = PanelBackground,
            BorderColor = PanelBorder,
            ShadowColor = new Color(0f, 0f, 0f, 0.65f),
            ShadowSize = 18,
            ShadowOffset = new Vector2(0, 8)
        };
        style.SetBorderWidthAll(3);
        style.SetCornerRadiusAll(8);
        style.SetContentMarginAll(22);
        return style;
    }

    private static StyleBoxFlat CreateButtonStyle(Color background, Color border, int borderWidth)
    {
        var style = new StyleBoxFlat
        {
            BgColor = background,
            BorderColor = border
        };
        style.SetBorderWidthAll(borderWidth);
        style.SetCornerRadiusAll(6);
        style.SetContentMarginAll(10);
        return style;
    }

    private static StyleBoxFlat CreateBadgeStyle()
    {
        var style = new StyleBoxFlat
        {
            BgColor = ButtonBackground,
            BorderColor = PanelBorder
        };
        style.SetBorderWidthAll(2);
        style.SetCornerRadiusAll(5);
        style.SetContentMarginAll(0);
        return style;
    }

    private static StyleBoxFlat CreateTransparentStyle()
    {
        var style = new StyleBoxFlat
        {
            BgColor = new Color(0f, 0f, 0f, 0f),
            BorderColor = new Color(0f, 0f, 0f, 0f)
        };
        style.SetBorderWidthAll(0);
        style.SetCornerRadiusAll(0);
        style.SetContentMarginAll(0);
        return style;
    }
}
