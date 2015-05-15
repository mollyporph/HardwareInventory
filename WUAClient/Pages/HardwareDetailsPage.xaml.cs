using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using HardwareInventory.Datamodel;
using HardwareInventory.Repository;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HardwareInventory.Pages
{
    public class HardwareItemViewModel : INotifyPropertyChanged
    {
        private bool _isEditMode = false;
        private HardwareItem _item = new HardwareItem();

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }

        public HardwareItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public sealed partial class HardwareDetailsPage : Page
    {
       private HardwareItemViewModel ViewModel { get; set; }
       private readonly DispatcherTimer _suggestionImageTimer = new DispatcherTimer();
        public HardwareDetailsPage()
        {
            ViewModel = new HardwareItemViewModel {IsEditMode =false};
            _suggestionImageTimer.Interval = TimeSpan.FromMilliseconds(600);
            _suggestionImageTimer.Tick += (sender, o) => { GetSuggestedImages(); };
            this.InitializeComponent();
            MainGrid.DataContext = ViewModel;
         

        }

        private async void GetSuggestedImages()
        {
          _suggestionImageTimer.Stop();
            var imageUrls = await HardwareRepository.GetSuggestedImagesForItem(itemNameTextBox.Text);
            GoogleImageGrid.ItemsSource = imageUrls;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs te)
        {
            _suggestionImageTimer.Start();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Item = e.Parameter as HardwareItem;
        }

        public void ManualEnterImageUrl(object sender, RoutedEventArgs e)
        {
            manualImageUrlTextBox.Visibility = Visibility.Visible;
        }

        public void ManualImageUrlTextBox_TextChanged(object sender, RoutedEventArgs e)
        {

            UpdateImageSource();
        }
        public void BackButton_Pressed(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void UpdateImageSource()
        {
            //Since HardwareItem isn't following propertychanged notification. This is workaround until that.
            var newImage = new BitmapImage { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
            if (!Uri.IsWellFormedUriString(ViewModel.Item.ImageUrl, UriKind.Absolute)) return;
            var uri = new Uri(ViewModel.Item.ImageUrl);
            newImage.UriSource = uri;
            ItemImage.Source = newImage;
        }
        public async void EnableEditOrSave(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsEditMode)
            {

                //SaveButton Clicked
                await HardwareRepository.UpdateItem(this.ViewModel.Item);
               
            }
            else
            {
                // Get suggested images
                GetSuggestedImages();
            }
            //Toggle
            ViewModel.IsEditMode = !ViewModel.IsEditMode;

        }


        public void GridViewItemClick(object sender, ItemClickEventArgs e)
        {
            var url = e.ClickedItem as string;
            ViewModel.Item.ImageUrl = url;
            UpdateImageSource();
        }
    }

    public class BoolToEditOrSaveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((bool) value) ? "Save" : "Edit";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string)) return false;
            return value.ToString() != "Edit";
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool) value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility) value == Visibility.Visible;
        }

    }
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

