using System.Windows.Controls;
using System.Windows;

namespace ChangeEditOpacity.Views;
/// <summary>
/// Interaction logic for SimpleView.xaml
/// </summary>
public partial class SimpleView : Window
{
    // Property to pass the initial opacity value from the main execution thread (ChangeEditOpacity.cs)
    // The XAML Slider will bind to this property for its initial value.
    public long InitialOpacityValue
    {
        get => (long)GetValue(InitialOpacityValueProperty);
        set => SetValue(InitialOpacityValueProperty, value);
    }

    public static readonly DependencyProperty InitialOpacityValueProperty =
        DependencyProperty.Register(nameof(InitialOpacityValue), typeof(long), typeof(SimpleView), new PropertyMetadata(50L)); // Default to 50%

    // Property to retrieve the selected opacity value from the Slider when the user clicks OK.
    // This value is automatically updated via the TwoWay binding in XAML on the OpacitySlider.
    public long SelectedOpacityValue
    {
        get => InitialOpacityValue; // Since the XAML binds Slider.Value to InitialOpacityValue (Mode=TwoWay), we can read the final value from here.
    }

    public SimpleView()
    {
        InitializeComponent();

        // Ensure the DataContext is set to the window instance itself for binding to work correctly
        // (This is already set in XAML but is good practice here as well)
        this.DataContext = this;
    }
}
