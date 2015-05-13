using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QinJilu.Web.Areas.Main.Models
{
    public class EFData
    {
        public static List<NewsInfo> GetNews(string[] colNames, int skip, int take)
        {
            if (colNames != null && colNames.Length > 0)
            {
                using (DataContext db = new DataContext())
                {
                    var nids = db.CategoryMap.Where(x => colNames.Contains(x.CategoryName)).Select(x => x.nid);
                    var lst = db.NewsInfo
                        .Where(x => nids.Contains(x.nid))
                        .OrderByDescending(x => x.sourcetime)
                        .Take(take).Skip(skip)
                    .Select(x => new NewsInfo
                    {
                        abs = x.abs,
                        //url = x.url,
                        title = x.title,
                        img = x.img,
                        //tag = x.tag,
                        sourcetime = x.sourcetime,
                        Id = x.Id,
                        //content = string.Empty,
                        //nid = x.nid,
                        //site = x.site
                    })
                        .ToList();
                    return lst;
                }
            }

            using (DataContext db = new DataContext())
            {
                var lst = db.NewsInfo
                    .OrderByDescending(x => x.sourcetime)
                    .Take(take).Skip(skip)
                    .Select(x => new NewsInfo
                    {
                        abs = x.abs,
                        //url = x.url,
                        title = x.title,
                        img = x.img,
                        //tag = x.tag,
                        sourcetime = x.sourcetime,
                        Id = x.Id,
                        //content = string.Empty,
                        //nid = x.nid,
                        //site = x.site
                    })
                    .ToList();
                return lst;
            }
        }


        public static NewsInfo GetOne(int id)
        {
            using (DataContext db = new DataContext())
            {
                var m = db.NewsInfo
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                return m;
            }
        }

        public static byte[] GetImg(string img_mark)
        {
            string nid = img_mark.Split('_')[0];
            int index = int.Parse(img_mark.Split('_')[1]);
            using (DataContext db = new DataContext())
            {
                var m = db.ImageResources
                    .Where(x => x.nid == nid && x.index == index)
                    .Select(x => x.Image)
                    .FirstOrDefault();
                return m;
            }
        }


    }





    public class DataContext : DbContext
    {

        public DbSet<NewsInfo> NewsInfo { get; set; }
        public DbSet<JsonInfo> JsonInfo { get; set; }
        public DbSet<CategoryMap> CategoryMap { get; set; }
        public DbSet<ImageResources> ImageResources { get; set; }


        public DataContext()
            : base("name=SQL_QinJilu")
        {
            Database.SetInitializer(new ServicesEntitiesIntializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Model.ProductChemicalProperties>().Property(p => p.MolImg).HasColumnType("image");
            //modelBuilder.Entity<CompanyProperties>().HasKey(x => x.ticks).Property(a => a.ticks).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.DatabaseGeneratedOption.Identity);

        }

        internal void Commit()
        {
            base.Configuration.AutoDetectChangesEnabled = true;
            base.SaveChanges();
        }
    }


    internal class ServicesEntitiesIntializer : System.Data.Entity.CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {

        }
    }





    public class CategoryMap
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string nid { get; set; }
    }
    public class ImageResources
    {
        public int Id { get; set; }
        public string nid { get; set; }
        public int index { get; set; }

        public string original { get; set; }

        public byte[] Image { get; set; }

        public int width { get; set; }
        public int height { get; set; }

        public bool HasDownload { get; set; }

    }
    public class JsonInfo
    {
        public int Id { get; set; }

        public string nid { get; set; }
        public string Json { get; set; }
        public bool HasParse { get; set; }
    }
    public class NewsInfo
    {
        public int Id { get; set; }

        public DateTime sourcetime { get; set; }
        public string nid { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string abs { get; set; }
        public string tag { get; set; }

        public string site { get; set; }
        public string url { get; set; }


    }
}