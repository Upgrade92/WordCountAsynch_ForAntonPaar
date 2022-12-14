using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WordCountAsynch_Zach.Data;
using WordCountAsynch_Zach.SupportClasses;

namespace WordCountAsynch_Zach
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource _cts = null;
        private string filePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// opens openFileDialog to select a File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            resetGui();
            filePath = new FileHandler().openFile();
            
            if(!string.IsNullOrEmpty(filePath))
            {
                GuiHelper.enableButton(btnStartCounting);
            }
        }

        /// <summary>
        /// starts the programmlogic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_getOccurrences(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(filePath))
            {
                _cts = new CancellationTokenSource();
              
                GuiHelper.startProgressBar(progressBar);
                GuiHelper.disableButton(btnSelectFile);
                GuiHelper.disableButton(btnStartCounting);
                GuiHelper.enableButton(btnAbort);

                string[] words = await new FileHandler().readAndSplitFile(filePath, _cts.Token);
                Dictionary<string, int> wordDict = await new TextHandler().countOccurrance(words, _cts.Token);
                words = null;

                if (!(_cts.Token.IsCancellationRequested))
                {
                    foreach (KeyValuePair<string, int> entry in wordDict.OrderByDescending(x => x.Value))
                    {
                        Words wordToAdd = new Words(entry.Key.ToString(), entry.Value);
                        listViewWords.Items.Add(wordToAdd);
                    }
                }

                if(wordDict != null)
                {
                    wordDict.Clear();
                }

                GuiHelper.disableButton(btnAbort);
                GuiHelper.enableButton(btnSelectFile);
                GuiHelper.toggleSaveButton(listViewWords, btnSave);
                GuiHelper.endProgressBar(progressBar,listViewWords);
            }
        }

        /// <summary>
        /// export the content in Listview as TextFile by starting a new Thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (listViewWords.Items.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                await Task.Run(() =>
                {
                    foreach (Words word in listViewWords.Items)
                    {
                        sb.AppendLine($"{word.Word,-20}  {word.Occurrance.ToString(),10}");
                    }
                    new FileHandler().saveFileAsTxt(sb.ToString());
                });
            }
        }

        /// <summary>
        /// Cancels the process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
            GuiHelper.endProgressBar(progressBar,listViewWords);
            GuiHelper.enableButton(btnSelectFile);
            GuiHelper.disableButton(btnAbort);
            MessageBox.Show("canceled", "Info");

        }

        /// <summary>
        /// minimize Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinmize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// close//shutdown the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Mousedown event for DragMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Displays About Info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            GuiHelper.showInfo();                            
        }

        /// <summary>
        /// resets the Gui 
        /// </summary>
        private void resetGui()
        {
            progressBar.Value = 0;
            listViewWords.Items.Clear();
            filePath = string.Empty;
            GuiHelper.disableButton(btnStartCounting);
            GuiHelper.toggleSaveButton(listViewWords, btnSave);
        }     
    }
}
