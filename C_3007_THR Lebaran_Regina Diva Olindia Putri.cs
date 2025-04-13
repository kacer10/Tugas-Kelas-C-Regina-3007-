using System;

public class Karyawan
{
    private string nama;
    private string id;
    private double gajiPokok;
    public string Nama
    {
        get { return nama; }
        set { nama = value; }
    }

    public string ID
    {
        get { return id; }
        set { id = value; }
    }

    public double GajiPokok
    {
        get { return gajiPokok; }
        set { gajiPokok = value; }
    }
    public Karyawan(string nama, string id, double gajiPokok)
    {
        this.nama = nama;
        this.id = id;
        this.gajiPokok = gajiPokok;
    }
    public Karyawan()
    {
    }
    public virtual double HitungGaji()
    {
        return gajiPokok;
    }
}

public class KaryawanTetap : Karyawan
{
    private const double BONUS_TETAP = 500000;
    public KaryawanTetap(string nama, string id, double gajiPokok)
        : base(nama, id, gajiPokok)
    {
    }
    public KaryawanTetap() : base()
    {
    }

    public override double HitungGaji()
    {
        return GajiPokok + BONUS_TETAP;
    }
}

public class KaryawanKontrak : Karyawan
{
    private const double POTONGAN_KONTRAK = 200000;
    public KaryawanKontrak(string nama, string id, double gajiPokok)
        : base(nama, id, gajiPokok)
    {
    }
    public KaryawanKontrak() : base()
    {
    }

    public override double HitungGaji()
    {
        return GajiPokok - POTONGAN_KONTRAK;
    }
}

public class KaryawanMagang : Karyawan
{
    public KaryawanMagang(string nama, string id, double gajiPokok)
        : base(nama, id, gajiPokok)
    {
    }
    public KaryawanMagang() : base()
    {
    }
    public override double HitungGaji()
    {
        return GajiPokok;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Sistem Manajemen Karyawan ===");
        Console.Write("Masukkan jenis karyawan (tetap/kontrak/magang): ");
        string jenisKaryawan = Console.ReadLine();

        Console.Write("Masukkan nama: ");
        string nama = Console.ReadLine();

        Console.Write("Masukkan ID: ");
        string id = Console.ReadLine();

        Console.Write("Masukkan gaji pokok: ");
        double gajiPokok = Convert.ToDouble(Console.ReadLine());

        Karyawan karyawan = null;

        switch (jenisKaryawan.ToLower())
        {
            case "tetap":
                karyawan = new KaryawanTetap(nama, id, gajiPokok);
                break;
            case "kontrak":
                karyawan = new KaryawanKontrak(nama, id, gajiPokok);
                break;
            case "magang":
                karyawan = new KaryawanMagang(nama, id, gajiPokok);
                break;
            default:
                Console.WriteLine("Jenis karyawan tidak valid!");
                return;
        }

        Console.WriteLine("\n=== Info Karyawan ===");
        Console.WriteLine($"Nama: {karyawan.Nama}");
        Console.WriteLine($"ID: {karyawan.ID}");
        Console.WriteLine($"Gaji Akhir: Rp {karyawan.HitungGaji()}");
    }
}