## Why do we need encryption?

> Encryption works best if it is ubiquitous and automatic. It should be enabled for everything by default, not a feature you only turn on when you’re doing something you consider worth protecting. - [Bruce Schneier](https://en.wikipedia.org/wiki/Bruce_Schneier), Cryptographer, Privacy and Security Specialist

## Real world example

Imagine that you have a sum of money (let's say 500$), you are in a store in the suburbs with lots of unkind people around you and your only friend asks you how much money you currently have. We'll assume that if anyone besides your friend hears you say out loud "500$", they'll rob you instantly. So, how do you tell your friend how much money do you have?

That's where encryption comes in - you define a specific communication channel (let's say hand gestures) and with one hand you display a high-five :hand: (meaning a 5), with the other hand the ok-sign :ok_hand: (meaning that you multiply the previous with 100), you clap your hands :clap: (meaning that you are talking cash) and then you point the finger at him :point_right: (meaning that you are sending the encrypted message). Your friend understands your sign language (decrypts and reads the message) and waves an ok-sign :thumbsup: (meaning that he understood the message) then points the finger at you :point_left: (sends an encrypted response back). The others don't understand what you two are communicating, so they get on with their lives.

## The project explained

<table>
<tr>
<td>
<a href="https://laurentiu.microsoft.pub.ro/wp-content/uploads/sites/3/2016/04/01-3.png" rel="attachment wp-att-71"><img class="alignleft wp-image-71 size-medium" src="https://laurentiu.microsoft.pub.ro/wp-content/uploads/sites/3/2016/04/01-3-248x300.png" alt="01" width="248" height="300" /></a></td>
</td>
<td>
<strong>MainPage.xaml</strong>: <br>User interface implementation consisting of 3 <a href="https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.textblock.aspx" target="_blank">TextBlocks</a>, 3 <a href="https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.textbox.aspx" target="_blank">TextBoxes</a> and 3 <a href="https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.button.aspx" target="_blank">Buttons</a>.
<br><br>
<strong>MainPage.xaml.cs</strong>: <br>Backend implementation consisting of 3 methods used for handlind the click events of each of the 3 buttons.
<br><br>
<strong>SymmetricEncryption.cs</strong>: <br>Implementation of a basic encryption and decryption capability which I will detaliate below.
</td>
</tr>
</table>

## The code - MainPage.xaml

Besides [the elements and attributes you already know](https://laurentiu.microsoft.pub.ro/2016/04/03/hello-universal-windows-platform/), in this example we will make use of the [StackPanel](https://msdn.microsoft.com/library/windows/apps/windows.ui.xaml.controls.stackpanel.aspx), [TextBox](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.textbox.aspx) and [Button](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.button.aspx) XAML elements. We will also learn how to create tables using [column](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.grid.columndefinitions.aspx) and [row](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.grid.rowdefinitions.aspx) definitions.

We can create rows and columns using the `<RowDefinition>` and `<ColumnDefinition>` tags inside (and only inside) the `<Grid.RowDefinitions>` tag. We the specify the `width` and `height` we want for our columns and rows. If we specify the `*` symbol after a number, then that column will strech to fit the grid's maximum width/height.

The `StackPanel` element is used in this context to proper align the elements without that much of a headache. This element behaves just like a [stack](https://en.wikipedia.org/wiki/Stack_(abstract_data_type)) in which you can specify an `Orientation` for your data.

We use the `TextBox` element to collect the information we want to encrypt from the user but also to display the encrypted an decrypted data via the `IsReadOnly` attribute. In order to manipulate the data from the user interfaces in the backend, we need to specify a variable name to the current textbox. We do that by assigning a unique name to the `x:Name` attribute (more about XAML namespaces [here](https://msdn.microsoft.com/en-us/windows/uwp/xaml-platform/xaml-overview#xaml-namespaces)).

The `Margin` attribute is used to **externally** align the current element within the parent element and the `Padding` attribute is used to **internally** align the current child elements within the current element.

The `Button` element is used to invoke an action (delegate an event - more on this soon :sunglasses:) so that the application can begin encrypting/decrypting the input data. To specify which method is executed when the user presses the button, we will set the `Click` attribute with the name of the method we want to call (the method must exist within `MainPage.xaml.cs` file).

## The code - Main.xaml.cs

The `readonly`keyword is used in this context to specify that the `encryptionProvider` variable should and can only be assigned in the constructor unlike the `const` keyword, which forces the developer to instantiate/assign the variable at declaration.

To display a pop-up style notification in the UI, we use an instance of the `MessageDialog` class. We then call the `ShowAsync()` method and await its termination using the **async/await** concept in C# (more on this soon :sunglasses:).

`Encrypt_Button_Click()` - This method is called when the user presses the **Encrypt** button in the UI. All this method does, is to the pass message we want to encrypt to the `SymmetricEncryption` class (which is viewed in this context as a service - more on this soon :sunglasses:).

`Decrypt_Button_Click()` - This method is called when the user presses the **Decrypt** button in the UI. All this method does, is to the pass message we want to decrypt to the `SymmetricEncryption` class.

`Reset_Button_Click()` - This method is called when the user presses the **Reset** button in the UI. All this method does, is to put the UI in the original state before encrypting/decrypting text without having to restart the app.

## The code - SymmetricEncryption.cs

All the encrypting algorithms are based on pure, hardcore mathematics. The algorithm we will use in this exampl is the Advanced Encryption Standard ([AES](https://en.wikipedia.org/wiki/Advanced_Encryption_Standard)) algorithm coupled with an electronic codebook ([ECB](https://en.wikipedia.org/wiki/Block_cipher_mode_of_operation#ECB)) mode of operation and [PKCS#7 padding](https://en.wikipedia.org/wiki/Padding_(cryptography)#PKCS7). If you want to learn more about cryptography and how it works you can check [this resource](http://www.di-mgt.com.au/cryptopad.html).

I've adapted the current example in a way that you can use any algorithm you want that is provided by the `SymmetricAlgorithmNames` class and whether to persist the encryption to disk or keeping it in RAM. The `Encrypt()` and `Decrypt()` methods have been adapted as well to no longer require to include a certain assembly in a class that doesn't need to expose the encryption service.

## Running the application

We are now ready to build, deploy and run our app. We go to **Debug > Start debugging** or press **F5** on our keyboard. After it builds and deploys successfully, you should see the following window pop-up

[![01](https://laurentiu.microsoft.pub.ro/wp-content/uploads/sites/3/2016/04/01-4.png)](https://laurentiu.microsoft.pub.ro/wp-content/uploads/sites/3/2016/04/01-4.png)

## In conclusion

As you may have observed, a bit of C# coding knowledge is required in order to create a basic encrypting/decrypting app in the Universal Windows Platform. For a better understanding of how powerful the C# language really is, you can check out [this repository](https://github.com/microsoft-dx/csharp-fundamentals/) full with basic C# projects. If you want to go deeply into advanced C# topics, you can check out <del>this repository</del> (will be created soon :sunglasses:)

So there you have it, a basic encrypting/decrypting UWP app. Stay tuned on [this blog](https://laurentiu.microsoft.pub.ro/) (and star the [microsoft-dx organization](https://github.com/microsoft-dx/)) to emerge in the beautiful world of "there's an app for that".

Adapted from: [msdn.microsoft.com](https://msdn.microsoft.com/en-us/library/windows/apps/windows.security.cryptography.core.cryptographicengine.encrypt.aspx)
