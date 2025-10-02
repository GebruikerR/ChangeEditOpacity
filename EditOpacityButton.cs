using Inventor;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms; // Required for MsgBox/MessageBox
using Inventor.InternalNames;
using Inventor.InternalNames.Ribbon;

namespace ChangeEditOpacity
{
    public class EditOpacityButton
    {
        private readonly Inventor.Application _inventor;
        private ButtonDefinition _settingsButton;

        public EditOpacityButton(Inventor.Application inventor)
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
                "Change Edit Opacity",
                "ChangeEditOpacity",
                CommandTypesEnum.kEditMaskCmdType,
                Guid.NewGuid().ToString(),
                "Change the opacity of the other components",
                "No");

            // AddHandler becomes += in C# for event subscription
            _settingsButton.OnExecute += MyButton_OnExecute;
        }

        private void AddButtonDefinitionToRibbon()
        {
            //// Add the button control
            // Part Environment
            _inventor.UserInterfaceManager.Ribbons[InventorRibbons.Part].RibbonTabs[PartRibbonTabs.Tools].RibbonPanels[PartRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);
            // Assembly Environment
            _inventor.UserInterfaceManager.Ribbons[InventorRibbons.Assembly].RibbonTabs[AssemblyRibbonTabs.Tools].RibbonPanels[AssemblyRibbonPanels.ToolsTab.Options].CommandControls.AddButton(_settingsButton);
        }

        private void MyButton_OnExecute(NameValueMap Context)
        {
            try
            {

                ChangeEditOpacity.Execute(_inventor);

            }
            catch (Exception ex)
            {
                // MsgBox becomes MessageBox.Show()
                MessageBox.Show("Something went wrong while running rule. Message: " + ex.Message);
            }
        }
    }
}