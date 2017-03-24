<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebMenuControl.ascx.cs"
    Inherits="UserControls_WebMenuControl" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<ComponentArt:Menu runat="server" ID="ubsMenu1" Orientation="Horizontal" CssClass="TopMenuGroup"
    DefaultGroupCssClass="MenuGroup" DefaultSubGroupExpandOffsetX="-10" DefaultSubGroupExpandOffsetY="-5"
    DefaultItemLookId="DefaultItemLook" TopGroupItemSpacing="0" DefaultGroupItemSpacing="2"
    ScrollingEnabled="true" ImagesBaseUrl="images/menu" ExpandDelay="100" Height="20px">
    <ItemLooks>
        <ComponentArt:ItemLook LookId="TopItemLook" CssClass="TopMenuItem" HoverCssClass="TopMenuItemHover" />
        <ComponentArt:ItemLook LookId="DefaultItemLook" CssClass="MenuItem" HoverCssClass="MenuItemHover"
            ExpandedCssClass="MenuItemExpanded" ActiveCssClass="MenuItemActive" />
        <ComponentArt:ItemLook LookId="SpacerItemLook" CssClass="TopMenuItem" />
    </ItemLooks>
</ComponentArt:Menu>
