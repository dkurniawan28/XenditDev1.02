using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace XenditDev1._02.ModelsNew
{
    public partial class PaymentGatewayContext : DbContext
    {
        public PaymentGatewayContext()
        {
        }

        public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetailBank> DetailBank { get; set; }
        public virtual DbSet<DetailCc> DetailCc { get; set; }
        public virtual DbSet<DetailEwallet> DetailEwallet { get; set; }
        public virtual DbSet<DetailLogTransaksi> DetailLogTransaksi { get; set; }
        public virtual DbSet<DetailOutlet> DetailOutlet { get; set; }
        public virtual DbSet<DetailQr> DetailQr { get; set; }
        public virtual DbSet<DetailWalletPayment> DetailWalletPayment { get; set; }
        public virtual DbSet<MasterPayment> MasterPayment { get; set; }
        public virtual DbSet<Merchant> Merchant { get; set; }
        public virtual DbSet<RequestLog> RequestLog { get; set; }
        public virtual DbSet<Transaksi> Transaksi { get; set; }
        public virtual DbSet<TransaksiPayment> TransaksiPayment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=rm-d9jrqh44yva4o181p.mssql.ap-southeast-5.rds.aliyuncs.com,3433;Database=PaymentGateway;User Id=pg;Password=semogajaya@1992;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<DetailBank>(entity =>
            {
                entity.HasKey(e => e.IdDetailBank)
                    .HasName("PK__DetailBa__A4B81079D9B3BA59");

                entity.Property(e => e.AccountHolderName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BankAccountNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BankBranch)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BankCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CollectionType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransferAmount).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdTransaksiPaymentNavigation)
                    .WithMany(p => p.DetailBank)
                    .HasForeignKey(d => d.IdTransaksiPayment)
                    .HasConstraintName("Fk_transaksibank");
            });

            modelBuilder.Entity<DetailCc>(entity =>
            {
                entity.HasKey(e => e.IdDetailCc)
                    .HasName("PK__DetailCc__DA526AD9FB0907F4");

                entity.Property(e => e.ApprovalCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorizationId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorizedAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BankReconciliationId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CaptureAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CardBrand)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CardType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cavv)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ChargeType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardTokenId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CvnCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Descriptor)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Eci)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdXendit)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IssuingBankName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MaskedCardNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantReferenceCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Metadata)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Xid)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTransaksiPaymentNavigation)
                    .WithMany(p => p.DetailCc)
                    .HasForeignKey(d => d.IdTransaksiPayment)
                    .HasConstraintName("Fk_transaksicc");
            });

            modelBuilder.Entity<DetailEwallet>(entity =>
            {
                entity.HasKey(e => e.IdDetailEwallet)
                    .HasName("PK__DetailEw__0D18110212D5648A");

                entity.Property(e => e.EwalletType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTransaksiPaymentNavigation)
                    .WithMany(p => p.DetailEwallet)
                    .HasForeignKey(d => d.IdTransaksiPayment)
                    .HasConstraintName("Fk_transaksiwallet");
            });

            modelBuilder.Entity<DetailLogTransaksi>(entity =>
            {
                entity.HasKey(e => e.IdDetailLogTransaksi)
                    .HasName("PK__DetailLo__A329C974F4943469");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdLogRequestNavigation)
                    .WithMany(p => p.DetailLogTransaksi)
                    .HasForeignKey(d => d.IdLogRequest)
                    .HasConstraintName("fk_logreq");

                entity.HasOne(d => d.IdTransaksiPaymentNavigation)
                    .WithMany(p => p.DetailLogTransaksi)
                    .HasForeignKey(d => d.IdTransaksiPayment)
                    .HasConstraintName("fk_Trpm");
            });

            modelBuilder.Entity<DetailOutlet>(entity =>
            {
                entity.HasKey(e => e.IdDetailOutlet)
                    .HasName("PK__DetailOu__52FC75370D4DDAB4");

                entity.Property(e => e.MerchantName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RetailOutletName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransferAmount).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdTransaksiPaymentNavigation)
                    .WithMany(p => p.DetailOutlet)
                    .HasForeignKey(d => d.IdTransaksiPayment)
                    .HasConstraintName("Fk_transaksioutlet");
            });

            modelBuilder.Entity<DetailQr>(entity =>
            {
                entity.HasKey(e => e.IdQr)
                    .HasName("PK__DetailQr__B77022E211000F49");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CallbackUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdXendit)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MetaData)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.QrString)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetailWalletPayment>(entity =>
            {
                entity.HasKey(e => e.IdDetailWalletPayment)
                    .HasName("PK__DetailWa__E1EA560C2E61D4E6");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BusinessId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CheckoutUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EwalletType)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdXendit)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasterPayment>(entity =>
            {
                entity.HasKey(e => e.IdMasterPayment)
                    .HasName("PK__MasterPa__6BAC1B295FBEF7E7");

                entity.Property(e => e.Invoice)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NamaPayment)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(e => e.IdMerchant)
                    .HasName("PK__Merchant__AA1ACDBCBA58536F");

                entity.Property(e => e.NamaMerchant)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UrlCallBack)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UrlWithdraw)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RequestLog>(entity =>
            {
                entity.HasKey(e => e.IdRequestLog)
                    .HasName("PK__RequestL__DC3F2278ED1A9DD0");

                entity.Property(e => e.Request)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Response)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaksi>(entity =>
            {
                entity.HasKey(e => e.IdTransaksi)
                    .HasName("PK__Transaks__DACCE832656A147B");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NominalCharge).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdReqLogNavigation)
                    .WithMany(p => p.Transaksi)
                    .HasForeignKey(d => d.IdReqLog)
                    .HasConstraintName("FK_Transaksi_IdReqLog");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Transaksi)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Transaksi_IdUser");
            });

            modelBuilder.Entity<TransaksiPayment>(entity =>
            {
                entity.HasKey(e => e.IdTransaksiPayment)
                    .HasName("PK__Transaks__24A41F0E3ABCE803");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Currency)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdXendit)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MerchantProfilePictureUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PayerEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentBy)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdMerchantNavigation)
                    .WithMany(p => p.TransaksiPayment)
                    .HasForeignKey(d => d.IdMerchan)
                    .HasConstraintName("Fk_merchanttransaksi");
            });
        }
    }
}
