namespace AlphaERP.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class User
    {
       
        [StringLength(8)]
        public string UserID { get; set; }

        [StringLength(8)]
        public string UserPWD { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public short? GroupID { get; set; }

        public short? Add { get; set; }

        public short? Upd { get; set; }

        public short? Del { get; set; }

        public bool? HideMenu { get; set; }

        public bool? StoppedUser { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string UserServer { get; set; }

        public bool? UserInternet { get; set; }

        public bool? OneSession { get; set; }

        public bool? IsLogedIn { get; set; }

        public bool? IsLocked { get; set; }

        public bool? ChangeDataGridViewDesign { get; set; }

    }
}
