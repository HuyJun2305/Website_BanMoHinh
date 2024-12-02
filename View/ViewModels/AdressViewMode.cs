namespace View.ViewModels
{
    public class AddressViewMode
    {
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
        public List<Ward> Wards { get; set; }
    }

    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Ward
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
