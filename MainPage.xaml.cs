using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EncryptionDecryption
{
    /// <summary>
    /// The main page used to demonstrate the encryption and decryption
    /// capabilities in the Universal Windows Platform
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly SymmetricEncryption encryptionProvider;

        public MainPage()
        {
            this.InitializeComponent();

            encryptionProvider = new SymmetricEncryption();
        }

        private async void Encrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputData.Text))
            {
                var dialog = new MessageDialog("You need to specify a string to encrypt !!");
                await dialog.ShowAsync();

                return;
            }

            var result = encryptionProvider.Encrypt(inputData.Text);

            encryptedData.Text = result;
        }

        private async void Decrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(encryptedData.Text))
            {
                var dialog = new MessageDialog("You need to encrypt a string first !!");
                await dialog.ShowAsync();

                return;
            }

            var result = encryptionProvider.Decrypt(encryptedData.Text);

            decryptedData.Text = result;
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            inputData.Text = string.Empty;
            encryptedData.Text = string.Empty;
            decryptedData.Text = string.Empty;
        }
    }
}
