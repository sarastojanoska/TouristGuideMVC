using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Data;

namespace ZavrsenProekt.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ZavrsenProektContext(
                serviceProvider.GetRequiredService<DbContextOptions<ZavrsenProektContext>>()))
            {
                if (context.Poseta.Any() || context.TuristickiVodic.Any() || context.Turist.Any())
                {
                    return;
                }
                context.TuristickiVodic.AddRange(
                    new TuristickiVodic { /* Id = 1 */ Ime = "Sara", Prezime = "Stojanoska", Obrazovanie = "Glaven Vodic"},
                    new TuristickiVodic { /* Id = 2 */ Ime = "Tea", Prezime = "Dimitrovska", Obrazovanie = "Glaven Vodic" },
                    new TuristickiVodic { /* Id = 3 */ Ime = "Ina", Prezime = "Kovaceva", Obrazovanie = "Asistent Vodic" },
                    new TuristickiVodic { /* Id = 4 */ Ime = "Ana Marija", Prezime = "Trajkovska", Obrazovanie = "Asistent Vodic"},
                    new TuristickiVodic { /* Id = 5 */ Ime = "Andrea", Prezime = "Kovaceva", Obrazovanie = "Glaven Vodic"}
                    );
                context.SaveChanges();

                context.Turist.AddRange(
                    new Turist { /* Id = 1 */ Ime = "Medi", Prezime = "Dimitrovski", PassportId = "5D13167008", DatumPrijava = DateTime.Parse("2019-7-13")},
                    new Turist { /* Id = 2 */ Ime = "Didi", Prezime = "RockStar", PassportId = "3B13160706", DatumPrijava = DateTime.Parse("2020-7-16")},
                    new Turist { /* Id = 3 */ Ime = "Brat Ljube", Prezime = "Trajkovski", PassportId = "1C77717701", DatumPrijava = DateTime.Parse("2020-2-10")},
                    new Turist { /* Id = 4 */ Ime = "Gigi", Prezime = "Lepoticata", PassportId = "7E23232323", DatumPrijava = DateTime.Parse("2019-5-3")}
                    );
                context.SaveChanges();

                context.Poseta.AddRange(
                    new Poseta
                    {
                        //Id = 1,
                        Ime = "Visit the Dinosaurs gallery",
                        DatumPoseta = DateTime.Parse("2021-9-24"),
                        Znamenitost = "National History Museum,London",
                        Price = 500,
                        Komentar = "Meet incredible beasts in the famous Dinosaurs gallery",
                        TuristickiVodicId = context.TuristickiVodic.Single(d => d.Ime == "Tea" && d.Prezime == "Dimitrovska").Id
                    },
                    new Poseta
                    {
                        //Id = 2,
                        Ime = "Tantra: enlightenment to revolution",
                        DatumPoseta = DateTime.Parse("2021-2-24"),
                        Znamenitost = "British Museum, London",
                        Price = 3100 ,
                        Komentar = "Explore the radical force the religious, cultural and political landscape of India and beyond.",
                        TuristickiVodicId = context.TuristickiVodic.Single(d => d.Ime == "Ina" && d.Prezime == "Kovaceva").Id
                    },
                    new Poseta
                    {
                        //Id = 3,
                        Ime = "Impressionist Decorations: The Birth of Modern Decor",
                        DatumPoseta = DateTime.Parse("2021-9-11"),
                        Znamenitost = "The National Gallery,London",
                        Price = 1000,
                        Komentar = "Open the door to a world of Impressionist interiors",
                        TuristickiVodicId = context.TuristickiVodic.Single(d => d.Ime == "Ana Marija" && d.Prezime == "Trajkovska").Id
                    },
                    new Poseta
                    {
                        //Id = 4,
                        Ime = "Visit the London Tower Bridge",
                        DatumPoseta = DateTime.Parse("2021-7-13"),
                        Znamenitost = "London Tower Bridge",
                        Price = 3000,
                        Komentar = "The visit includes visit of the Towers,Glass Floor and Engine Rooms",
                        TuristickiVodicId = context.TuristickiVodic.Single(d => d.Ime == "Andrea" && d.Prezime == "Kovaceva").Id
                    },
                    new Poseta
                    {
                        //Id = 5,
                        Ime = "Diana: Designing for a Princess",
                        DatumPoseta = DateTime.Parse("2021-5-22"),
                        Znamenitost = "Kensington",
                        Price = 2000,
                        Komentar = "Elegant display at Diana, Princess of Wales former home of Kensington Palace, a piece from Diana's wardrobe.",
                        TuristickiVodicId = context.TuristickiVodic.Single(d => d.Ime == "Sara" && d.Prezime == "Stojanoska").Id
                    }
                    ); 
                context.SaveChanges();
                context.VkluciSe.AddRange(
                    new VkluciSe { PosetaId = 1, TouristId = 1 },
                    new VkluciSe { PosetaId = 1, TouristId = 2 },
                    new VkluciSe { PosetaId = 1, TouristId = 3 },
                    new VkluciSe { PosetaId = 1, TouristId = 4 },
                    new VkluciSe { PosetaId = 2, TouristId = 1 },
                    new VkluciSe { PosetaId = 2, TouristId = 3 },
                    new VkluciSe { PosetaId = 3, TouristId = 2 },
                    new VkluciSe { PosetaId = 3, TouristId = 1 },
                    new VkluciSe { PosetaId = 3, TouristId = 4 },
                    new VkluciSe { PosetaId = 4, TouristId = 1 },
                    new VkluciSe { PosetaId = 4, TouristId = 3 },
                    new VkluciSe { PosetaId = 5, TouristId = 4 },
                    new VkluciSe { PosetaId = 5, TouristId = 2 }
                    );
                context.SaveChanges();
            }
        }
    }
}
