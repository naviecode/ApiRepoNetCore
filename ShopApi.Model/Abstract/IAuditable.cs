namespace ShopApi.Model.Abstract
{
    public interface IAuditable
    {
        DateTime? CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string UpdatedBy { get; set; }

        string MetaKeyWord { get; set; }
        string MetaDesc { get; set; }

        bool Status { get; set; }
    }
}
