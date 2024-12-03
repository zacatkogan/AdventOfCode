using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public abstract class BaseDayWithData : AoCHelper.BaseDay
    {
        protected BaseDayWithData()
        {
            this._data ??= GetData();
        }

        public virtual string GetData() => new AdventOfCode.Utils.Downloader()
            .GetData(this.GetProblemDay(), this.GetProblemYear())
            .TrimEnd();

        public string Data 
        {
            get => _data ??= GetData();
            set => _data = value; 
        }
        private string _data;

        public string[] DataLines
        {
            get => _dataLines ??= _data.Split('\n');
            set => _dataLines = value;
        }
        private string[]? _dataLines;

        public virtual int GetProblemYear()
        {
            var type = this.GetType();
            var namespaces = type.Namespace!.Split('.');

            var match = Regex.Match(namespaces.Last(), "(year|aoc).*(?<year>\\d{4})", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new Exception("Unable to determine Problem Year from Namespace");
            }

            var yearRaw = match.Groups["year"];
            return int.Parse(yearRaw.Value);
        }

        public virtual int GetProblemDay()
        {
            return (int)CalculateIndex();
        }
    }
}
