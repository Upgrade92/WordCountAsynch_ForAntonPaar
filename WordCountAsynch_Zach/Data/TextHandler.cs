using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WordCountAsynch_Zach.Data
{
    public class TextHandler
    {
        /// <summary>
        /// splitting the content into an Array of single words
        /// </summary>
        /// <param name="content"></param>
        /// <returns>Array of single words</returns>
        public string[] splitContent(string content)
        {
            char[] sep = { ' ', ',', '!', '?', '"', '\'', '*', '-', '.', '\r', '\n', '\b' };
            string[] words = null;

            words = content.Split(sep);
            return words;
        }

        /// <summary>
        /// converts an Array of Words into a Dictionary and count the occurrances
        /// </summary>
        /// <param name="words"></param>
        /// <param name="_cts"></param>
        /// <returns>Dictionary of words and there occurrance </returns>
        public async Task<Dictionary<string, int>> countOccurrance(string[] words, CancellationToken _cts)
        {
            Dictionary<string, int> dictCounts = new Dictionary<string, int>();

            if (!(_cts.IsCancellationRequested))
            {
                await Task.Run(() =>
                {
                    foreach (string word in words)
                    {
                        try
                        {
                            if (_cts.IsCancellationRequested)
                            {
                                _cts.ThrowIfCancellationRequested();
                                dictCounts.Clear();
                                break;
                            }

                            if (!(dictCounts.ContainsKey(word)))
                            {
                                dictCounts.Add(word, 1);
                            }
                            else dictCounts[word]++;
                            dictCounts.Remove("");
                        }
                        catch (OperationCanceledException) { /*MessageBox.Show("cancel 2");*/ }
                    }
                });
                return dictCounts;
            }
            else return null;
        }
    }
}
