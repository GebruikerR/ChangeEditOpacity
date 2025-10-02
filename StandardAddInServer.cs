using Inventor;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms; // Required for MessageBox

// Change the GUID here and use the same as in the addin file!
[Guid("4a478a3c-556f-4936-bb39-4d3bf3749e05"), ComVisible(true)]
public class StandardAddInServer : ApplicationAddInServer
{
    private MyButton _myButton;

    /// <summary>
    ///     Invoked by Autodesk Inventor after creating the AddIn. 
    ///     AddIn should initialize within this call.
    /// </summary>
    /// <param name="AddInSiteObject">
    ///     Input argument that specifies the object, which provides 
    ///     access to the Autodesk Inventor Application object.
    /// </param>
    /// <param name="FirstTime">
    ///     The FirstTime flag, if True, indicates to the Addin that this is the 
    ///     first time it is being loaded and to take some specific action.
    /// </param>
    public void Activate(ApplicationAddInSite AddInSiteObject, bool FirstTime)
    {
        try
        {
            // initialize the rule class
            // Assuming MyButton is a class that takes the Inventor.Application object in its constructor
            _myButton = new MyButton(AddInSiteObject.Application);
        }
        catch (Exception ex)
        {
            // Show a message if any thing goes wrong.
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    ///     Invoked by Autodesk Inventor to shut down the AddIn. 
    ///     AddIn should complete shutdown within this call.
    /// </summary>
    public void Deactivate()
    {
        // Clean up resources if necessary
        // e.g., releasing references to Inventor objects or removing UI elements
    }

    /// <summary>
    ///     Invoked by Autodesk Inventor in response to user requesting the execution 
    ///     of an AddIn-supplied command. AddIn must perform the command within this call.
    /// </summary>
    public void ExecuteCommand(int CommandID)
    {
        // Handle command execution if needed
    }

    /// <summary>
    ///     Gets the IUnknown of the object implemented inside the AddIn that supports AddIn-specific API.
    /// </summary>
    public object Automation
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}