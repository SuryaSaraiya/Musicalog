using Musicalog.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musicalog.Common.Data
{
    public class AlbumsDbContext : DbContext
    {
        //public static AlbumsDbContext dbContext;
        private AlbumsDbContext() : base("MusicalogDbConnectionString")
        { }

        public static AlbumsDbContext Create()
        {
            //if (dbContext == null)
            //{
            //    dbContext = new AlbumsDbContext();
            //}
            //return dbContext;

            return new AlbumsDbContext();
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<AlbumArtists> AlbumArtists { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<AlbumType> AlbumTypes { get; set; }
    }
}
