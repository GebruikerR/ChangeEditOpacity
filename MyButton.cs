using Inventor;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms; // Required for MsgBox/MessageBox
using Inventor.InternalNames;
using Inventor.InternalNames.Ribbon;
public class MyButton
{
    private Inventor.Application _inventor;
    private ButtonDefinition _settingsButton;

    public MyButton(Inventor.Application inventor)
    {
        _inventor = inventor;

        SetupButtonDefinition();
        AddButtonDefinitionToRibbon();
    }

    private void SetupButtonDefinition()
    {
        ControlDefinitions conDefs = _inventor.CommandManager.ControlDefinitions;

        // Use a consistent GUID for production, but Guid.NewGuid().ToString() works for testing
        // Note: For a real Inventor Add-In, you should use a fixed GUID here
        // and register it correctly in the .addin file.
        _settingsButton = conDefs.AddButtonDefinition(
            "Change Background",
            "ChangeEditOpacity",
            CommandTypesEnum.kEditMaskCmdType,
            Guid.NewGuid().ToString(),
            "Choose the In-Canvas color scheme",
            "No");

        // AddHandler becomes += in C# for event subscription
        _settingsButton.OnExecute += MyButton_OnExecute;
    }

    private void AddButtonDefinitionToRibbon()
    {
        //// Access the Assembly ribbon
        //Ribbon ribbon = _inventor.UserInterfaceManager.Ribbons[InventorRibbons.ZeroDoc];

        //// Access the Manage tab (where iLogic usually lives)
        //RibbonTab ribbonTab = ribbon.RibbonTabs[ZeroDocRibbonTabs.Tools];

        //// Access the iLogic panel within the Manage tab
        //RibbonPanel ribbonPanel = ribbonTab.RibbonPanels[ZeroDocRibbonPanels.ToolsTab.Options];

        //// Add the button control

        // Part Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.Part].RibbonTabs[PartRibbonTabs.Tools].RibbonPanels[PartRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);

        // Assembly Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.Assembly].RibbonTabs[AssemblyRibbonTabs.Tools].RibbonPanels[AssemblyRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);

        // Drawing Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.Drawing].RibbonTabs[DrawingRibbonTabs.Tools].RibbonPanels[DrawingRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);

        // ZeroDoc Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.ZeroDoc].RibbonTabs[ZeroDocRibbonTabs.Tools].RibbonPanels[ZeroDocRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);

        // Presentation Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.Presentation].RibbonTabs[PresentationRibbonTabs.Tools].RibbonPanels[PresentationRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);

        // iFeatures Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.iFeatures].RibbonTabs[iFeatureRibbonTabs.Tools].RibbonPanels[iFeatureRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);

        // UnknownDocument Environment
        _inventor.UserInterfaceManager.Ribbons[InventorRibbons.UnknownDocument].RibbonTabs[UnknownDocumentRibbonTabs.Tools].RibbonPanels[UnknownDocumentRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);
    }

    private void MyButton_OnExecute(NameValueMap Context)
    {
        try
        {
            ChangeEditOpacity.ChangeEditOpacity.Execute(_inventor);

        }
        catch (Exception ex)
        {
            // MsgBox becomes MessageBox.Show()
            MessageBox.Show("Something went wrong while running rule. Message: " + ex.Message);
        }
    }
}