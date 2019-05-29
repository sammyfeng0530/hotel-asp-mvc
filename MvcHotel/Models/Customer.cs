//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Book = new HashSet<Book>();
        }

        //[Required]
        //[StringLength(20)]
        [Display(Name = "姓名")]
        public string Cname { get; set; }
        [Display(Name = "性别")]
        public bool Csex { get; set; }

        [Key]
        //[StringLength(11)]
        //[Required(ErrorMessage = "请输入电话号码")]
        [Display(Name = "电话号码")]
        //[Remote("CheckCtel", "Home", ErrorMessage = "用户名已经存在")]
        public string Ctel { get; set; }

        //[StringLength(20)]
        //[Required(ErrorMessage = "请输入密码")]
        [Display(Name = "密码")]
        public string Cpwd { get; set; }
        [Display(Name = "头像")]
        public string Cpic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Book { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Customer()
        //{
        //    this.Book = new HashSet<Book>();
        //}

        //public string Cname { get; set; }
        //[UIHint("_Csex")]
        //public bool Csex { get; set; }
        ////[Remote("CheckCtel", "Home", ErrorMessage = "用户名已经存在")]
        //public string Ctel { get; set; }
        //[Required(ErrorMessage = "请输入密码")]
        //public string Cpwd { get; set; }
        //public string Cpic { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Book> Book { get; set; }
    }
}