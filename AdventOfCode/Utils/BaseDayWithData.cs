namespace AdventOfCode
{
    public abstract class BaseDayWithData : AoCHelper.BaseDay
    {
        protected BaseDayWithData()
        {
        }

        public virtual string GetData() => new AdventOfCode.Utils.Downloader()
            .GetData(this.GetProblemDay(), this.GetProblemYear())
            .TrimEnd();

        public string Data 
        {
            get => _data ??= GetData();
            set => _data = value; 
        }
        private string? _data;

        public virtual int GetProblemYear()
        {
            var type = this.GetType();
            var namespaces = type.Namespace!.Split('.');
            
            var yearSegment = namespaces.FirstOrDefault(
                x => x.StartsWith("year", StringComparison.OrdinalIgnoreCase)
            );

            if (yearSegment is null)
            {
                throw new Exception("Unable to determine Problem Year from Namespace");
            }

            return int.Parse(yearSegment.Substring(yearSegment.Length-4));
        }

        public virtual int GetProblemDay()
        {
            return (int)CalculateIndex();
        }
    }
}
