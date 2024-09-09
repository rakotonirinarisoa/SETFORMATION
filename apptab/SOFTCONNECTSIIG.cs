using System.Data.Entity;
using apptab.Models;

namespace apptab
{
    public partial class SOFTCONNECTSIIG : DbContext
    {
        public SOFTCONNECTSIIG()
            : base("name=SOFTCONNECTSIIG")
        {
        }

        public virtual DbSet<OPA_ANOMALIE> OPA_ANOMALIE { get; set; }
        public virtual DbSet<OPA_ANOMALIEBR> OPA_ANOMALIEBR { get; set; }
        public virtual DbSet<OPA_BASE> OPA_BASE { get; set; }
        public virtual DbSet<OPA_CRYPTO> OPA_CRYPTO { get; set; }
        public virtual DbSet<OPA_DATABASE> OPA_DATABASE { get; set; }
        public virtual DbSet<OPA_DONNEURORDRE> OPA_DONNEURORDRE { get; set; }
        public virtual DbSet<OPA_FTP> OPA_FTP { get; set; }
        public virtual DbSet<OPA_HISTORIQUE> OPA_HISTORIQUE { get; set; }
        public virtual DbSet<OPA_REGLEMENT> OPA_REGLEMENT { get; set; }
        public virtual DbSet<OPA_REGLEMENTBR> OPA_REGLEMENTBR { get; set; }
        public virtual DbSet<OPA_SOCIETES> OPA_SOCIETES { get; set; }
        public virtual DbSet<SI_ACTIVITE> SI_ACTIVITE { get; set; }
        public virtual DbSet<SI_CATEGORIE> SI_CATEGORIE { get; set; }
        public virtual DbSet<SI_CONVENTION> SI_CONVENTION { get; set; }
        public virtual DbSet<SI_DELAISTRAITEMENT> SI_DELAISTRAITEMENT { get; set; }
        public virtual DbSet<SI_ENGAGEMENT> SI_ENGAGEMENT { get; set; }
        public virtual DbSet<SI_FINANCEMENT> SI_FINANCEMENT { get; set; }
        public virtual DbSet<SI_MAIL> SI_MAIL { get; set; }
        public virtual DbSet<SI_MAPPAGES> SI_MAPPAGES { get; set; }
        public virtual DbSet<SI_MINISTERE> SI_MINISTERE { get; set; }
        public virtual DbSet<SI_MISSION> SI_MISSION { get; set; }
        public virtual DbSet<SI_PARAMETAT> SI_PARAMETAT { get; set; }
        public virtual DbSet<SI_PROCEDURE> SI_PROCEDURE { get; set; }
        public virtual DbSet<SI_PROGRAMME> SI_PROGRAMME { get; set; }
        public virtual DbSet<SI_PROJETS> SI_PROJETS { get; set; }
        public virtual DbSet<SI_PROSOA> SI_PROSOA { get; set; }
        public virtual DbSet<SI_ROLES> SI_ROLES { get; set; }
        public virtual DbSet<SI_SOAS> SI_SOAS { get; set; }
        public virtual DbSet<SI_TRAITANNUL> SI_TRAITANNUL { get; set; }
        public virtual DbSet<SI_TRAITPROJET> SI_TRAITPROJET { get; set; }
        public virtual DbSet<SI_USERS> SI_USERS { get; set; }
        public virtual DbSet<OPA_HISTORIQUEBR> OPA_HISTORIQUEBR { get; set; }
        public virtual DbSet<OPA_ROLES> OPA_ROLES { get; set; }
        public virtual DbSet<OPA_DROITS> OPA_DROITS { get; set; }
        public virtual DbSet<SI_MAPUSERPROJET> SI_MAPUSERPROJET { get; set; }
        public virtual DbSet<SI_MENU> SI_MENU { get; set; }
        public virtual DbSet<HSI_MOTIF> HSI_MOTIF { get; set; }
        public virtual DbSet<SI_MOTIF> SI_MOTIF { get; set; }
        public virtual DbSet<SI_GEDLIEN> SI_GEDLIEN { get; set; }
        public virtual DbSet<SI_PRIVILEGE> SI_PRIVILEGE { get; set; }
        public virtual DbSet<SI_TYPECRITURE> SI_TYPECRITURE { get; set; }
        public virtual DbSet<SI_TRAITANNULAVANCE> SI_TRAITANNULAVANCE { get; set; }
        public virtual DbSet<SI_TRAITAVANCE> SI_TRAITAVANCE { get; set; }

        public virtual DbSet<HSI_PROSOA> HSI_PROSOA { get; set; }
        public virtual DbSet<HSI_ACTIVITE> HSI_ACTIVITE { get; set; }
        public virtual DbSet<HSI_CATEGORIE> HSI_CATEGORIE { get; set; }
        public virtual DbSet<HSI_CONVENTION> HSI_CONVENTION { get; set; }
        public virtual DbSet<HSI_ENGAGEMENT> HSI_ENGAGEMENT { get; set; }
        public virtual DbSet<HSI_FINANCEMENT> HSI_FINANCEMENT { get; set; }
        public virtual DbSet<HSI_MINISTERE> HSI_MINISTERE { get; set; }
        public virtual DbSet<HSI_MISSION> HSI_MISSION { get; set; }
        public virtual DbSet<HSI_PROCEDURE> HSI_PROCEDURE { get; set; }
        public virtual DbSet<HSI_PROGRAMME> HSI_PROGRAMME { get; set; }
        public virtual DbSet<HSI_PROJETS> HSI_PROJETS { get; set; }
        public virtual DbSet<HSI_USERS> HSI_USERS { get; set; }
        public virtual DbSet<HOPA_CRYPTO> HOPA_CRYPTO { get; set; }
        public virtual DbSet<HOPA_FTP> HOPA_FTP { get; set; }
        public virtual DbSet<HSI_DELAISTRAITEMENT> HSI_DELAISTRAITEMENT { get; set; }
        public virtual DbSet<HSI_MAIL> HSI_MAIL { get; set; }
        public virtual DbSet<HSI_PARAMETAT> HSI_PARAMETAT { get; set; }
        public virtual DbSet<OPA_VALIDATIONS> OPA_VALIDATIONS { get; set; }
        public virtual DbSet<OPA_HCANCEL> OPA_HCANCEL { get; set; }
        public virtual DbSet<HSI_SOAS> HSI_SOAS { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OPA_ANOMALIE>()
               .Property(e => e.ID)
               .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_ANOMALIE>()
                .Property(e => e.NUM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_HISTORIQUE>()
                .Property(e => e.NUMENREG)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_REGLEMENT>()
                .Property(e => e.NUM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_REGLEMENT>()
                .Property(e => e.MONTANT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_REGLEMENTBR>()
                .Property(e => e.ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_REGLEMENTBR>()
                .Property(e => e.MONTANT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Debit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Credit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.MontantDevise)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Devise)
                .IsFixedLength();

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Debit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Credit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.MontantDevise)
                .HasPrecision(18, 0);
            modelBuilder.Entity<OPA_VALIDATIONS>()
              .Property(e => e.Devise)
              .IsFixedLength();
            modelBuilder.Entity<OPA_VALIDATIONS>()
               .Property(e => e.Debit)
               .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Credit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.MontantDevise)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.Devise)
                .IsFixedLength();

            modelBuilder.Entity<OPA_VALIDATIONS>()
                .Property(e => e.MONTANT)
                .HasPrecision(18, 0);
        }
    }
}
