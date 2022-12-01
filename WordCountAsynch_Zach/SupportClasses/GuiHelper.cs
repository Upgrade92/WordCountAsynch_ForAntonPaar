using System.Windows;
using System.Windows.Controls;

namespace WordCountAsynch_Zach.SupportClasses
{
    static public class GuiHelper
    {
        /// <summary>
        /// enables a button
        /// </summary>
        /// <param name="btn"></param>
        public static void enableButton(Button btn)
        {
            btn.IsEnabled = true;
        }

        /// <summary>
        /// disables a button
        /// </summary>
        /// <param name="btn"></param>
        public static void disableButton(Button btn)
        {
            btn.IsEnabled = false;
        }

        /// <summary>
        /// start Progressbar
        /// </summary>
        /// <param name="progressBar"></param>
        public static void startProgressBar(ProgressBar progressBar )
        {
            progressBar.IsIndeterminate = true;
        }

        /// <summary>
        /// ends Progressbar and fills it depending on if Listview is filled or not
        /// </summary>
        /// <param name="progressBar"></param>
        public static void endProgressBar(ProgressBar progressBar, ListView listView)
        {
            progressBar.IsIndeterminate = false;

            if(listView.Items.Count != 0)
            {
                progressBar.IsIndeterminate = false;
                progressBar.Value = 100;
            }
            else
            {
                progressBar.IsIndeterminate = false;
            }
        }

        /// <summary>
        /// enables and disables a Button depending on ListviewItems.Count
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="btn"></param>
        public static void toggleSaveButton(ListView listView, Button btn)
        {
            if (listView.Items.Count != 0)
            {
                enableButton(btn);
            }
            else
            {
                disableButton(btn);
            }
        }

        /// <summary>
        /// Displays Userinfo via a MessageBox
        /// </summary>
        public static void showInfo()
        {
            MessageBox.Show("This tools helps you to get the occurance of words within a text file.\n\n" +
                            "First of all select a text File by clicking on \"Select File\".\n" +
                            "After that you can start the counting Process by clicking \"Start Counting\".\n" +
                            "You are also able to export the shown data by simply pressing on \"Save\"."
                            , "Info");
        }
    }
}
