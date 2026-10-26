using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Configuration;

namespace CSLab08_LINQ
{
    [Database(Name = "QLHS")]
    public class QLHSDataContext : DataContext
    {
        public QLHSDataContext() : base(ConfigurationManager.ConnectionStrings["QLHS"].ConnectionString) { }

        public QLHSDataContext(string connectionString) : base(connectionString) { }

        public Table<Lop> Lops { get { return GetTable<Lop>(); } }
        public Table<HocSinh> HocSinhs { get { return GetTable<HocSinh>(); } }
    }

    [Table(Name = "dbo.Lop")]
    public class Lop
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = false)]
        public string MaLop { get; set; }

        [Column]
        public string TenLop { get; set; }
    }

    [Table(Name = "dbo.HocSinh")]
    public class HocSinh
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = false)]
        public string MaHS { get; set; }

        [Column]
        public string TenHS { get; set; }

        [Column]
        public DateTime NgaySinh { get; set; }

        [Column]
        public string DiaChi { get; set; }

        [Column]
        public float DTB { get; set; }

        [Column]
        public string MaLop { get; set; }
    }
}
