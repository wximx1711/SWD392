using System.Text;
public static class Catalog
{
    public static readonly string[] Sports = ["Badminton", "Pickleball", "Table Tennis"];
    public static readonly string[] Surfaces = ["Wood", "Rubber", "Acrylic"];
    public static readonly string[] Statuses = ["Active", "Under Maintenance", "Inactive"];
    public static readonly int[] SlotMinutes = [30, 60, 90, 120];
    public static string Normalize(string value) => value.Trim().Normalize(NormalizationForm.FormC).ToUpperInvariant();
}
