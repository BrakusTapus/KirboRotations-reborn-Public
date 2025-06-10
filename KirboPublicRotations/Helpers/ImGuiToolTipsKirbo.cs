using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Textures.TextureWraps;
using RotationSolver.Basic.Attributes;
using RotationSolver.Basic.Data;

namespace KirboPublicRotations.Helpers;

internal static class ImGuiToolTipsKirbo
{
    const ImGuiWindowFlags TOOLTIP_FLAG =
          ImGuiWindowFlags.Tooltip |
          ImGuiWindowFlags.NoMove |
          ImGuiWindowFlags.NoSavedSettings |
          ImGuiWindowFlags.NoBringToFrontOnFocus |
          ImGuiWindowFlags.NoDecoration |
          ImGuiWindowFlags.NoInputs |
          ImGuiWindowFlags.AlwaysAutoResize;

    const string TOOLTIP_ID = "Kirbo's Tooltips";

    internal static void HoveredTooltip(string? text)
    {
        if (!ImGui.IsItemHovered())
        {
            return;
        }

        ShowTooltip(text);
    }

    internal static void ShowTooltip(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        ///ShowTooltip(() => ImGui.Text(text));
        ShowTooltip(() => ImGui.TextColored(ImGuiColors.DalamudGrey2, text));
    }

    internal static void ShowTooltip(Action act)
    {
        if (act == null)
        {
            return;
        }

        ImGui.SetNextWindowBgAlpha(1);

        using ImRaii.Color color = ImRaii.PushColor(ImGuiCol.BorderShadow, ImGuiColors.DalamudWhite);

        ImGui.SetNextWindowSizeConstraints(new Vector2(150, 0) * ImGuiHelpers.GlobalScale, new Vector2(1200, 1500) * ImGuiHelpers.GlobalScale);
        ImGui.SetWindowPos(TOOLTIP_ID, ImGui.GetIO().MousePos);

        if (ImGui.Begin(TOOLTIP_ID, TOOLTIP_FLAG))
        {
            act();
            ImGui.End();
        }
    }
}
