namespace ShopApi.Service
{
    public class ResponseDataDto<T>
    {
        public int TotalItem { get; set; }
        public List<T> Items { get; set; }

        public ResponseDataDto(List<T> items, int totalItem)
        {
            TotalItem = totalItem;
            Items = items;
        }
    }
}
