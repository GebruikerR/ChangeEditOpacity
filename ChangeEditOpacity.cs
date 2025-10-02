using Inventor;
using System;
using System.Windows;
using System.Threading;

namespace ChangeEditOpacity
{
    internal class ChangeEditOpacity
    {
        /// <summary>
        /// Reads the current InactiveComponentsShadeOpacity from Inventor, 
        /// shows a dialog to change it, and applies the new value on OK.
        /// </summary>
        /// <param name="invApplication">The running Inventor Application object.</param>
        public static void Execute(Inventor.Application invApplication)
        {
            // Launch the UI logic on a new STA thread.
            // This is required for WPF components to run correctly.
            Thread thread = new(() =>
            {
                try
                {
                    // === FIX: Check for existing System.Windows.Application instance ===
                    if (System.Windows.Application.Current == null)
                    {
                        // Create the WPF Application instance ONLY if it doesn't exist.
                        // This will run the WPF message pump for the Add-in's AppDomain.
                        _ = new System.Windows.Application();
                    }

                    // 1. Get Inventor Display Options and the current Opacity value
                    DisplayOptions displayOptions = invApplication.DisplayOptions;
                    // InactiveComponentsShadeOpacity returns a Long (percentage 0-100)
                    long currentOpacity = displayOptions.InactiveComponentsShadeOpacity;

                    // 2. Create the WPF Window
                    // You must define a SimpleView class in a Views namespace for this to compile.
                    var window = new Views.SimpleView();

                    // ASSUMPTION: The SimpleView class now exposes a public property named 
                    // 'InitialOpacityValue' which is used to initialize the UI control (e.g., a Slider).
                    // The user must update SimpleView.cs to expose this property.
                    try
                    {
                        // Use reflection as a general placeholder for a public property setter,
                        // as we cannot see the SimpleView definition here.
                        window.GetType().GetProperty("InitialOpacityValue")?.SetValue(window, currentOpacity);
                    }
                    catch { /* Fail silently if the property is not found or cannot be set */ }


                    // 3. Handle OK Button Click (Logic updated to set the opacity)
                    window.OkButton.Click += (s, e) =>
                    {
                        long newOpacity = -1; // Default to an invalid value
                        try
                        {
                            // ASSUMPTION: The final selected opacity value is available via a public property 
                            // on the window (e.g., SelectedOpacityValue) after the user interacts with the UI.

                            object? valueObj = window.GetType().GetProperty("SelectedOpacityValue")?.GetValue(window);

                            if (valueObj != null)
                            {
                                // The Inventor API expects a long (percentage 0-100)
                                newOpacity = Convert.ToInt64(valueObj);
                            }

                            // The property is a percentage (0-100)
                            if (newOpacity >= 0 && newOpacity <= 100)
                            {
                                // Apply the new opacity to the Inventor setting
                                displayOptions.InactiveComponentsShadeOpacity = (int)newOpacity;
                            }
                            else
                            {
                                // Inform user if the retrieved value is out of range
                                MessageBox.Show("Opacity value is outside the valid range (0-100). No change applied.", "Validation Error");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle conversion or Inventor API error
                            MessageBox.Show($"Failed to apply new opacity setting: {ex.Message}", "Application Error");
                        }

                        // Close the window after trying to apply changes
                        window.Close();
                    };

                    // 4. Handle Cancel Button Click (Logic remains the same)
                    window.CancelButton.Click += (s, e) => window.Close();

                    // 5. Show the dialog
                    window.ShowDialog();
                    // Do NOT call app.Shutdown() here, as it would close the single Application instance.
                }
                catch (Exception ex)
                {
                    // Use a thread-safe way to display an error if needed
                    System.Windows.MessageBox.Show("Error in UI thread: " + ex.Message, "Error");
                }
            });

            // Set the thread to Single-Threaded Apartment mode, required for WPF
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
