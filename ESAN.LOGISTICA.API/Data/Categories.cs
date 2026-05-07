namespace ESAN.LOGISTICA.API.Data;

public partial class Categories
{
    public int IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Products> Products { get; set; }
        = new List<Products>();
}