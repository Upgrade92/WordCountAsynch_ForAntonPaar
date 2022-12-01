namespace WordCountAsynch_Zach.Data
{
    public class Words
    {
        public string Word { get; set; }
        public int Occurrance { get; set; }

        public Words(string word, int occurrance)
        {
            Word = word;
            Occurrance = occurrance;
        }

    }
}
