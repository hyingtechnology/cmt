﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace cmt.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class cmtEntities : DbContext
    {
        public cmtEntities()
            : base("name=cmtEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<News> news { get; set; }
        public virtual DbSet<NewsFile> news_file { get; set; }
        public virtual DbSet<User> user { get; set; }
        public virtual DbSet<Knowledge> knowledge { get; set; }
        public virtual DbSet<KnowledgeFile> knowledge_file { get; set; }
        public virtual DbSet<Log> log { get; set; }
        public virtual DbSet<CodeTable> CodeTable { get; set; }
    }
}
