using UnityEngine.UIElements;

namespace UI.TestMenu
{
    public class TestMenuController
    {
        private const string TabClassName = "tab";
        private const string CurrentlySelectedTabClassName = "currentlySelectedTab";
        private const string UnselectedContentClassName = "unselectedContent";

        private const string TabNameSuffix = "Tab";
        private const string ContentNameSuffix = "Content";

        private readonly VisualElement _root;

        public TestMenuController(VisualElement root)
        {
            _root = root;
        }

        public void RegisterCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach(tab =>
            {
                tab.RegisterCallback<ClickEvent>(TabOnClick);
            });
        }
    
        private void TabOnClick(ClickEvent evt)
        {
            Label clickedTab = evt.currentTarget as Label;
            if (!TabIsCurrentlySelected(clickedTab))
            {
                GetAllTabs().Where(
                    (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)
                ).ForEach(UnselectTab);
                SelectTab(clickedTab);
            }
        }
    
        private static bool TabIsCurrentlySelected(Label tab)
        {
            return tab.ClassListContains(CurrentlySelectedTabClassName);
        }
    
        private UQueryBuilder<Label> GetAllTabs()
        {
            return _root.Query<Label>(className: TabClassName);
        }
    
        private void SelectTab(Label tab)
        {
            tab.AddToClassList(CurrentlySelectedTabClassName);
            VisualElement content = FindContent(tab);
            content.RemoveFromClassList(UnselectedContentClassName);
        }
    
        private void UnselectTab(Label tab)
        {
            tab.RemoveFromClassList(CurrentlySelectedTabClassName);
            VisualElement content = FindContent(tab);
            content.AddToClassList(UnselectedContentClassName);
        }
    
        private static string GenerateContentName(Label tab) =>
            tab.name.Replace(TabNameSuffix, ContentNameSuffix);
    
        private VisualElement FindContent(Label tab)
        {
            return _root.Q(GenerateContentName(tab));
        }
    }
}
