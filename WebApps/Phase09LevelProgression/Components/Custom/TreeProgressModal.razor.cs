namespace Phase09LevelProgression.Components.Custom;
public partial class TreeProgressModal
{
    [Parameter]
    [EditorRequired]
    public TreeView Tree { get; set; }
    private string ReadyText => $"Ready In {TreeManager.TimeLeftForResult(Tree)}";
    private string ProduceText => $"{TreeManager.GetProduceAmount(Tree)} {TreeManager.GetTreeName(Tree)}";

}