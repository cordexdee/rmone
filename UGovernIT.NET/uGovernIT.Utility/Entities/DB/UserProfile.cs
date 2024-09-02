using System;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.AspNetUsers)]
    [JsonObject(MemberSerialization.OptOut)]
    public class UserProfile : DBBaseEntity, IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

        public string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTimeOffset? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }

        public string EmployeeId { get; set; }
        [Column(DatabaseObjects.Columns.DepartmentLookup)]
        [JsonProperty(DatabaseObjects.Columns.DepartmentLookup)]
        public string Department { get; set; }
        public int HourlyRate { get; set; }
        [Column(DatabaseObjects.Columns.LocationLookup)]
        public string Location { get; set; }
        [Column(DatabaseObjects.Columns.ManagerID)]
        [JsonProperty(DatabaseObjects.Columns.ManagerID)]
        public string ManagerID { get; set; }

        [NotMapped]
        public UserLookupValue Manager1 { get; private set; }
        public string MobilePhone { get; private set; }
        public string JobProfile { get; set; }

        public bool IsIT { get; set; }
        public bool IsConsultant { get; set; }
        public bool IsManager { get; set; }
        public bool IsServiceAccount { get; set; }
        public int LocationId { get; private set; }
        public int DepartmentId { get; set; }
        public bool Enabled { get; set; }
        [Column(DatabaseObjects.Columns.UserSkillLookup)]
        public string Skills { get; set; }
        public string UserCertificateLookup { get; set; }
        [Column(DatabaseObjects.Columns.FunctionalAreaLookup)]
        public int? FunctionalArea { get; set; }
        [Column(DatabaseObjects.Columns.BudgetIdLookup)]
        public int? BudgetCategory { get; set; }
        public string DeskLocation { get; set; }
        public DateTime UGITStartDate { get; set; }
        public DateTime UGITEndDate { get; set; }
        public bool EnablePasswordExpiration { get; set; }
        public DateTime PasswordExpiryDate { get; set; }

        public bool DisableWorkflowNotifications { get; set; }
        //property containing user image url
        public string Picture { get; set; }
        public string NotificationEmail { get; set; }

        public double? ApproveLevelAmount { get; set; }

        //new entries for "out of office calender" start
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
        public bool EnableOutofOffice { get; set; }

        // public string DelegateUserOnLeave { get; private set; }
        public string DelegateUserOnLeave { get; set; }//Show delegate task to dropdown value in profile.
        public string DelegateUserFor { get; set; }
        public DateTime? WorkingHoursEnd { get; set; }
        public DateTime? WorkingHoursStart { get; set; }

        // public List<Core.UserProfileRole> Groups { get; set; }

        public bool isRole { get; set; }
        [NotMapped]
        public string AccountId { get; set; }
        [NotMapped]
        public string LoginName { get; set; }
        [Column(DatabaseObjects.Columns.UserRoleIdLookup)]
        public string UserRoleId { get; set; }
        public long JobTitleLookup { get; set; }
        public string GlobalRoleId { get; set; }
        public bool? IsShowDefaultAdminPage { get; set; }
        public int IsDefaultAdmin { get; set; }
        public string History { get; set; }
        public string EmployeeType { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        //[NotMapped]
        //public string ParentUserId { get; set; } //clam code 
        public string Resume { get; set; }
        public long StudioLookup { get; set; }
        [NotMapped]
        public string Division { get; set; }

        public UserProfile()
        {
            MobilePhone = string.Empty;
            Department = string.Empty;
            Location = string.Empty;
            JobProfile = string.Empty;
            HourlyRate = 0;
            LocationId = 0;
            //Skills = new List<LookupValue>();
            Name = string.Empty;
            Email = string.Empty;
            NotificationEmail = string.Empty;
            UGITStartDate = DateTime.MinValue.AddYears(1753);
            UGITEndDate = DateTime.MinValue.AddYears(1753);
            LeaveFromDate = DateTime.MinValue.AddYears(1753);
            LeaveToDate = DateTime.MinValue.AddYears(1753);
            PasswordExpiryDate = DateTime.MinValue.AddYears(1753);
            isRole = false;
            Enabled = false;
            IsConsultant = false;
            IsManager = false;
            IsServiceAccount = false;
            ApproveLevelAmount = 0;
            //property containing user image url           
            Picture = @"/Content/Images/userNew.png";
            //claim code 
            //ParentUserId = "";
        }

        public UserProfile(UserInfo user)
        {
            Id = user.ID;
            MobilePhone = user.PhoneNumber;
            Department = string.Empty;
            Location = string.Empty;
            JobProfile = string.Empty;
            HourlyRate = 0;
            LocationId = 0;
            //Skills = new List<LookupValue>();
            Name = user.UserName;
            Email = user.Email;
            NotificationEmail = string.Empty;
            //property containing user image url           
            Picture = @"/Content/Images/userNew.png";
            //StoreBase<UserProfile> store = new StoreBase<UserProfile>(DatabaseObjects.Tables.UserProfile);
            //UserProfile userProfile = store.Load("Where ID = '" + user.ID + "'").FirstOrDefault();
            //if (userProfile != null)
            //{
            //    Department = userProfile.Department;
            //    Location = userProfile.Location;
            //    JobProfile = userProfile.JobProfile;
            //}
        }
        /// <summary>
        /// Load user. if SPContext.Current Exist  then load from cache otherwise load from list.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oWeb"></param>
        /// <returns></returns>
    }
}
