namespace CleanApp.Services
{
    public class PaginationService<T>
    {
        public int CurrentPage { get; private set; } = 1;
        //setting max. orders to be displayed on each page
        public int PageSize { get; private set; } = 10;
        public int TotalPages
        {
            get
            {
                if (_source.Count == 0) 
                    return 1;

                int totalItemsCount = _source.Count;
                int size = PageSize;

                int pages = (totalItemsCount + size - 1) / size;

                return pages;
            }
        }

        private List<T> _source = new();

        public void SetSource(IEnumerable<T> source)
        {
            _source = source.ToList();

            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
        }

        public IEnumerable<T> GetPage()
        {
            return _source
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);
        }

        public void SetPageSize(int size)
        {
            PageSize = size;
            CurrentPage = 1;
        }

        public void PrevPage()
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }
        public void NextPage()
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
        }
        public void FirstPage()
        {
            CurrentPage = 1;
        }
        public void LastPage()
        {
            CurrentPage = TotalPages;
        }

        public void ResetPage()
        {
            CurrentPage = 1;
        }
    }
}
