namespace ESAN.LOGISTICA.API.Data;

public partial class Products
{
    public int IdProducto { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public int? IdCategory { get; set; }

    public virtual Categories? IdCategoryNavigation { get; set; }
}