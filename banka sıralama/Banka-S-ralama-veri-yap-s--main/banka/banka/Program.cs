using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace banka
{
    class Program
    {
        static void Main(string[] args)
        {
            BankaKuyrugu bankaKuyrugu = new BankaKuyrugu();
            int secim;

            do
            {
                Console.WriteLine("\nLütfen yapmak istediğiniz işlemi seçiniz:" +
                    "\n1- müşteri ekle" +
                    "\n2- müşteri çıkar" +
                    "\n3- kuyruğu göster" +
                    "\n4- çıkış");
                secim = Convert.ToInt32(Console.ReadLine());

                switch (secim)
                {
                    case 1:
                        // Kullanıcıdan müşteri bilgilerini al
                        Console.Write("Müşterinin Adı: ");
                        string ad = Console.ReadLine();

                        Console.WriteLine("Öncelik Seçiniz: (1 - Hamile, 2 - Yaşlı, 3 - Sıradan)");
                        int oncelik = Convert.ToInt32(Console.ReadLine());

                        // Müşteriyi kuyruğa ekle
                        bankaKuyrugu.Ekle(new Musteri(ad, oncelik));
                        Console.WriteLine($"{ad} isimli müşteri kuyruga eklendi.");
                        break;

                    case 2:
                        // Müşteriyi kuyruktan çıkar
                        try
                        {
                            Musteri hizmetEdilenMusteri = bankaKuyrugu.Cikar();
                            Console.WriteLine($"{hizmetEdilenMusteri.Ad} isimli müşteri işlem yapıyor.");
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;

                    case 3:
                        // Kuyruktaki tüm müşterileri göster
                        Console.WriteLine("Kuyruktaki Müşteriler:");
                        bankaKuyrugu.KuyruguGoster();
                        break;

                    case 4:
                        // Çıkış işlemi
                        Console.WriteLine("Sistemden çıkış yapılıyor...");
                        break;

                    default:
                        Console.WriteLine("Geçersiz bir seçim yaptınız. Tekrar deneyiniz.");
                        break;
                }
            } while (secim != 4);
            Console.ReadLine();
        }

    }

    public class Musteri
    {
        public string Ad { get; set; }
        public int Oncelik { get; set; } // 1: Hamile, 2: Yaşlı, 3: Sıradan

        public Musteri(string ad, int oncelik)
        {
            Ad = ad;
            Oncelik = oncelik;
        }
    }

    public class Dugum
    {
        public Musteri Musteri { get; set; }
        public Dugum Sonraki { get; set; }

        public Dugum(Musteri musteri)
        {
            Musteri = musteri;
            Sonraki = null;
        }
    }

    public class BankaKuyrugu
    {
        private Dugum bas;

        public BankaKuyrugu()
        {
            bas = null;
        }

        // Yeni müşteri ekleme işlemi
        public void Ekle(Musteri musteri)
        {
            Dugum yeniDugum = new Dugum(musteri);

            // Baş düğüm yoksa veya öncelik yeni düğümden düşükse başa ekle
            if (bas == null || bas.Musteri.Oncelik > musteri.Oncelik)
            {
                yeniDugum.Sonraki = bas;
                bas = yeniDugum;
            }
            else
            {
                // Öncelik sırasına göre doğru yeri bul ve yeni düğümü ekle
                Dugum mevcut = bas;
                while (mevcut.Sonraki != null && mevcut.Sonraki.Musteri.Oncelik <= musteri.Oncelik)
                {
                    mevcut = mevcut.Sonraki;
                }
                yeniDugum.Sonraki = mevcut.Sonraki;
                mevcut.Sonraki = yeniDugum;
            }
        }

        // En yüksek öncelikli müşteriyi çıkarma işlemi
        public Musteri Cikar()
        {
            if (bas == null)
            {
                throw new InvalidOperationException("Kuyruk boş.");
            }

            Musteri hizmetEdilenMusteri = bas.Musteri;
            bas = bas.Sonraki;
            return hizmetEdilenMusteri;
        }

        // Kuyruktaki tüm müşterileri gösterme işlemi
        public void KuyruguGoster()
        {
            if (bas == null)
            {
                Console.WriteLine("Kuyruk boş.");
                return;
            }

            Dugum mevcut = bas;
            while (mevcut != null)
            {
                Console.WriteLine($"{mevcut.Musteri.Ad} (Öncelik: {mevcut.Musteri.Oncelik})");
                mevcut = mevcut.Sonraki;
            }
        }
    }
}
