namespace Music.DataAccess.Migrations
{
    using Music.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Music.DataAccess.MusicDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Music.DataAccess.MusicDbContext context)
        {
            context.Songs.AddOrUpdate( 
                s => s.Name, 
                new Song { Name = "The End." },
                new Song { Name = "Dead!" },
                new Song { Name = "This Is How I Disappear" },
                new Song { Name = "The Sharpest Lives" }
                );
        }
    }
}
