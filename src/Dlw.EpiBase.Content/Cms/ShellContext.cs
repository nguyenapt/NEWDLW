using EPiServer.Editor;

namespace Dlw.EpiBase.Content.Cms
{
    public class ShellContext : IShellContext
    {
        public bool PageIsInEditMode => PageEditing.PageIsInEditMode;
    }
}