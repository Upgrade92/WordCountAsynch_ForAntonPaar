using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordCountAsynch_Zach.Data;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace WordCountAsynch_Zach.SupportClasses
{
    public class FileHandler
    {

        /// <summary>
        /// get the path of a chosen File via OpenFileDialog
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public string openFile()
        {
            var fileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                else return null;
            }
        }

        /// <summary>
        /// saves the content as txt File by starting a new Thread
        /// </summary>
        /// <param name="content"></param>
        public void saveFileAsTxt(string content)
        {
            Thread t = new Thread((ThreadStart)(() =>
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = "DefaultOutputTxt.txt";
                save.Filter = "Text File | *.txt";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(save.OpenFile()))
                    {
                        writer.WriteLine(content);
                        writer.Dispose();
                        writer.Close();
                    }
                }
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            MessageBox.Show("saved!", "Info");
        }

        /// <summary>
        /// reading the file using StreamReader and splitting it into an Array
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Array of single Words</returns>
        public async Task<string[]> readAndSplitFile(string path, CancellationToken _cts)
        {

            string[] wordsToAdd = null;
            List<string> wordList = new List<string>();

            await Task.Run(() => {
                try
                {
                    foreach (string line in File.ReadAllLines(path))
                    {
                        wordsToAdd = new TextHandler().splitContent(line);
                        if (_cts.IsCancellationRequested)
                        {
                            _cts.ThrowIfCancellationRequested();
                            wordList.Clear();
                        }
                        wordList.AddRange(wordsToAdd.ToList());
                    }
                }
                catch (OperationCanceledException) { }
            });
            return wordList.ToArray();
        }
    }
}
