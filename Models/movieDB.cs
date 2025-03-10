using Microsoft.EntityFrameworkCore;


namespace Movies.Models;

public partial class movieDB : DbContext
{
    public movieDB(DbContextOptions<movieDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Director> Directors { get; set; }
    public virtual DbSet<MovieCategory> MovieCategories { get; set; }
    public virtual DbSet<MovieActor> MovieActors { get; set; }

    //public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    //public virtual DbSet<Payment> Payments { get; set; }

    //public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<User> Users { get; set; }
    //public object MovieRepository { get; internal set; } = new object();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Data Source =HNDW-PHLINH-TTS\\SQLEXPRESS; Database =movieDB;User ID=sa;Password=Fbiphan2k4@;Encrypt=false;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.ActorsID).HasName("PK__Actors__E60C94727B530F37");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoriesID).HasName("PK__Categori__EFF907B09DB23D2A");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.DirectorID).HasName("PK__Director__26C69E26795B95C7");
        });

        //modelBuilder.Entity<Episode>(entity =>
        //{
        //    entity.HasKey(e => e.EpisodeId).HasName("PK__Episodes__AC6676151E8973F9");

        //    entity.HasOne(d => d.Series).WithMany(p => p.Episodes).HasConstraintName("FK__Episodes__Series__49C3F6B7");
        //});
        modelBuilder.Entity<MovieActor>()
      .HasKey(ma => new { ma.MovieID, ma.ActorsID });

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieID)
            .HasConstraintName("FK_MovieActor_Movie");

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActor)
            .HasForeignKey(ma => ma.ActorsID)
            .HasConstraintName("FK_MovieActor_Actor");

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2943AD52A39F2");

            entity.Property(e => e.Status).HasDefaultValue(1);

            entity.HasOne(d => d.Director).WithMany(p => p.Movies).HasConstraintName("FK__Movies__Director__4316F928");

            entity.HasMany(d => d.Actors)
             .WithMany(p => p.Movies)
             .UsingEntity<MovieActor>(
                 j => j.HasOne(mc => mc.Actor)
                       .WithMany()
                       .HasForeignKey(mc => mc.ActorsID)
                       .HasConstraintName("FK__MovieActo__Actor__5812160E"),
                 j => j.HasOne(mc => mc.Movie)
                       .WithMany()
                       .HasForeignKey(mc => mc.MovieID)
                       .HasConstraintName("FK__MovieActo__Movie__571DF1D5"),
                 j =>
                 {
                     j.HasKey(mc => new { mc.MovieID, mc.ActorsID })
                      .HasName("PK__MovieAct__75B25D7DE91BD012");
                     j.ToTable("MovieActor");
                 });

            entity.HasMany(d => d.Categories)
            .WithMany(p => p.Movies)
            .UsingEntity<MovieCategory>(
                j => j.HasOne(mc => mc.Category)
                      .WithMany()
                      .HasForeignKey(mc => mc.CategoriesID)
                      .HasConstraintName("FK__MovieCate__Categ__5070F446"),
                j => j.HasOne(mc => mc.Movie)
                      .WithMany()
                      .HasForeignKey(mc => mc.MovieID)
                      .HasConstraintName("FK__MovieCate__Movie__4F7CD00D"),
                j =>
                {
                    j.HasKey(mc => new { mc.MovieID, mc.CategoriesID })
                     .HasName("PK__MovieCat__552D0441A8F57D73");
                    j.ToTable("MovieCategories");
                });
        });

        //modelBuilder.Entity<Payment>(entity =>
        //{
        //    entity.HasKey(e => e.SubPaymentID).HasName("PK__Payment__813806001037AB0E");

        //    entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payment__UserID__4CA06362");
        //});

        //modelBuilder.Entity<Series>(entity =>
        //{
        //    entity.HasKey(e => e.SeriesId).HasName("PK__Series__F3A1C101CC1C20B4");

        //    entity.Property(e => e.Season).HasDefaultValue(1);

        //    entity.HasOne(d => d.Director).WithMany(p => p.Series).HasConstraintName("FK__Series__Director__46E78A0C");

        //    entity.HasMany(d => d.Actors).WithMany(p => p.Series)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "SeriesActor",
        //            r => r.HasOne<Actor>().WithMany()
        //                .HasForeignKey("ActorsId")
        //                .HasConstraintName("FK__SeriesAct__Actor__5BE2A6F2"),
        //            l => l.HasOne<Series>().WithMany()
        //                .HasForeignKey("SeriesId")
        //                .HasConstraintName("FK__SeriesAct__Serie__5AEE82B9"),
        //            j =>
        //            {
        //                j.HasKey("SeriesId", "ActorsId").HasName("PK__SeriesAc__CDC108463B24E581");
        //                j.ToTable("SeriesActor");
        //                j.IndexerProperty<int>("SeriesId").HasColumnName("SeriesID");
        //                j.IndexerProperty<int>("ActorsId").HasColumnName("ActorsID");
        //            });

        //    entity.HasMany(d => d.Categories).WithMany(p => p.Series)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "SeriesCategory",
        //            r => r.HasOne<Category>().WithMany()
        //                .HasForeignKey("CategoriesId")
        //                .HasConstraintName("FK__SeriesCat__Categ__5441852A"),
        //            l => l.HasOne<Series>().WithMany()
        //                .HasForeignKey("SeriesId")
        //                .HasConstraintName("FK__SeriesCat__Serie__534D60F1"),
        //            j =>
        //            {
        //                j.HasKey("SeriesId", "CategoriesId").HasName("PK__SeriesCa__ED5E517A2E8C616E");
        //                j.ToTable("SeriesCategories");
        //                j.IndexerProperty<int>("SeriesId").HasColumnName("SeriesID");
        //                j.IndexerProperty<int>("CategoriesId").HasColumnName("CategoriesID");
        //            });
        //});

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC901230A1");

            entity.Property(e => e.Createdat).HasDefaultValueSql("(getdate())");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}