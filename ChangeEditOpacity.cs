using Inventor;
using System;
using System.Windows;
using System.Threading;

namespace ChangeEditOpacity
{
    internal class ChangeEditOpacity
    {
        public static void Execute(Inventor.Application invApplication)
        {
            // Launch the UI logic on a new STA thread.
            // This is required for WPF components to run correctly.
            Thread thread = new Thread(() =>
            {
                try
                {
                    // === FIX: Check for existing System.Windows.Application instance ===
                    if (System.Windows.Application.Current == null)
                    {
                        // Create the WPF Application instance ONLY if it doesn't exist.
                        // This will run the WPF message pump for the Add-in's AppDomain.
                        new System.Windows.Application();
                    }

                    // 1. Get Inventor data before showing the dialog
                    var schemes = invApplication.ColorSchemes as ColorSchemes;

                    // 2. Create the WPF Window
                    // You must define a SimpleView class in a Views namespace for this to compile.
                    var window = new Views.SimpleView();

                    var comboBox = window.ColorSchemeComboBox;
                    comboBox.Items.Clear();

                    // 3. Populate the ComboBox (Logic remains the same)
                    if (schemes != null && schemes.Count > 0)
                    {
                        for (int i = 1; i <= schemes.Count; i++)
                        {
                            if (schemes[i] is ColorScheme scheme)
                            {
                                comboBox.Items.Add(scheme.Name);

                                if (invApplication.ActiveColorScheme is ColorScheme activeScheme && scheme.Name == activeScheme.Name)
                                    comboBox.SelectedItem = scheme.Name;
                            }
                        }
                    }
                    else
                    {
                        comboBox.Items.Add("(no color schemes found)");
                        comboBox.IsEnabled = false;
                    }

                    // 4. Handle OK Button Click (Logic remains the same)
                    window.OkButton.Click += (s, e) =>
                    {
                        var selectedName = comboBox.SelectedItem as string;
                        if (schemes != null && !string.IsNullOrEmpty(selectedName))
                        {
                            for (int i = 1; i <= schemes.Count; i++)
                            {
                                if (schemes[i] is ColorScheme scheme && scheme.Name == selectedName)
                                {
                                    scheme.Activate();
                                    break;
                                }
                            }
                        }
                        window.Close();
                    };

                    // 5. Handle Cancel Button Click (Logic remains the same)
                    window.CancelButton.Click += (s, e) => window.Close();

                    // 6. Show the dialog
                    window.ShowDialog();
                    // Do NOT call app.Shutdown() here, as it would close the single Application instance.
                }
                catch (Exception ex)
                {
                    // Use a thread-safe way to display an error if needed
                    MessageBox.Show("Error in UI thread: " + ex.Message, "Error");
                }
            });

            // Set the thread to Single-Threaded Apartment mode, required for WPF
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}