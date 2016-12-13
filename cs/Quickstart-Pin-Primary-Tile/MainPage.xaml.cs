using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Quickstart_Pin_Primary_Tile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ShowLiveTileTipIfNeeded()
        {
            // You should  check if you've already shown this tip before,
            // and if so, don't show the tip to the user again.
            if (ApplicationData.Current.LocalSettings.Values.Any(i => i.Key.Equals("ShownLiveTileTip")))
            {
                // But for purposes of this Quickstart, we'll always show the tip
                //return;
            }

            // Store that you've shown this tip, so you don't show it again
            ApplicationData.Current.LocalSettings.Values["ShownLiveTileTip"] = true;

            // Get your own app list entry (which is the first entry assuming you have a single-app package)
            var appListEntry = (await Package.Current.GetAppListEntriesAsync())[0];

            // Get the Start screen manager
            var startScreenManager = StartScreenManager.GetDefault();

            // Check if Start supports pinning your app
            bool supportsPin = startScreenManager.SupportsAppListEntry(appListEntry);

            // If Start doesn't support pinning, don't show the tip
            if (!supportsPin)
            {
                ShowMessage("Start doesn't support pinning.");
                return;
            }

            // Check if you're pinned
            bool isPinned = await StartScreenManager.GetDefault().ContainsAppListEntryAsync(appListEntry);

            // If the tile is already pinned, don't show the tip
            if (isPinned)
            {
                ShowMessage("The tile is already pinned to Start!");
                return;
            }

            // Otherwise, show the tip
            FlyoutPinTileTip.ShowAt(ButtonShowTip);
        }

        private async void ButtonPinTile_Click(object sender, RoutedEventArgs e)
        {
            // Get your own app list entry (which is the first entry assuming you have a single-app package)
            var appListEntry = (await Package.Current.GetAppListEntriesAsync())[0];

            // And pin your app
            bool didPin = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(appListEntry);

            if (didPin)
            {
                ShowMessage("Success! Tile was pinned!");
            }
            else
            {
                ShowMessage("Tile was NOT pinned, did you click no on the dialog?");
            }
        }

        private void ButtonShowTip_Click(object sender, RoutedEventArgs e)
        {
            ShowLiveTileTipIfNeeded();
        }

        private async void ShowMessage(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }
    }
}
