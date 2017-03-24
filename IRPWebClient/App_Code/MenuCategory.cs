using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///MenuCategory 的摘要说明
/// </summary>
public class MenuCategory
{
    public MenuCategory()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public string MenuName { get; set; }
    public string MenuText { get; set; }
    public string MenuURL { get; set; }
    private MenuCategory _ParentMenu = null;

    public MenuCategory ParentMenu
    {
        get { return _ParentMenu; }
        set { _ParentMenu = value; }
    }

    private List<MenuCategory> _ChildMenus = new List<MenuCategory>();

    public List<MenuCategory> ChildMenus
    {
        get { return _ChildMenus; }
        set { _ChildMenus = value; }
    }
}
