using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Reflection;

namespace uGovernIT.Manager.Core
{
        public class ExternalProjectMapping
        {

        }



    [DataContract]
    public class AuthorizationToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public int expires_in { get; set; }
        [DataMember]
        public string refresh_token { get; set; }
        [DataMember]
        public int created_at { get; set; }
    }



    [DataContract]
    public class TokenInfo
    {
        [DataMember]
        public int resource_owner_id { get; set; }
        [DataMember]
        public List<object> scopes { get; set; }
        [DataMember]
        public int expires_in_seconds { get; set; }
        [DataMember]
        public int created_at { get; set; }
        //public Application application { get; set; }
    }




    [DataContract]
    public class LoginToken
    {
        [DataMember]
        public string token { get; set; }
    }

    [DataContract]
    public class NewRootProject
    {
        [DataMember]
        public int company_id { get; set; }
        [DataMember]
        public NewProject project { get; set; }
    }

    [DataContract]
    public class NewProject
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public List<object> department_ids { get; set; }
        [DataMember]
        public string project_number { get; set; }
        [DataMember]
        public string estimated_start_date { get; set; }
        [DataMember]
        public string estimated_completion_date { get; set; }
        [DataMember]
        public string square_feet { get; set; }
        [DataMember]
        public string state_code { get; set; }
        [DataMember]
        public string country_code { get; set; }
        [DataMember]
        public string project_template_id { get; set; }
        [DataMember]
        public string office_id { get; set; }
        [DataMember]
        public string project_type_id { get; set; }
        [DataMember]
        public ProjectStage project_stage { get; set; }
        //[DataMember]
        //public Int32 project_stage_id { get; set; }
    }

    [DataContract]
    public class ProjectStage
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public bool is_bidding_stage { get; set; }
    }



    [DataContract]
    public class CreatedNewProject
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string logo_url { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string display_name { get; set; }
        [DataMember]
        public string project_number { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string estimated_start_date { get; set; }
        [DataMember]
        public string estimated_completion_date { get; set; }
        [DataMember]
        public bool active { get; set; }
        [DataMember]
        public string stage { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public List<object> departments { get; set; }
    }



    [DataContract]
    public class Office
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }


    [DataContract]
    public class ProjectTemplates
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }


    [DataContract]
    public class ProjectType
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class ShowProject
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string logo_url { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string display_name { get; set; }
        [DataMember]
        public string project_number { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string state_code { get; set; }
        [DataMember]
        public string country_code { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool active { get; set; }
        [DataMember]
        public string stage { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public string square_feet { get; set; }
        [DataMember]
        public Office office { get; set; }
        [DataMember]
        public List<object> departments { get; set; }
        [DataMember]
        public string actual_start_date { get; set; }
        [DataMember]
        public string projected_finish_date { get; set; }
    }

    //[DataContract]
    //public class SubcontractorList
    //{
    //    [DataMember]
    //    public int id { get; set; }
    //    [DataMember]
    //    public string title { get; set; }
    //    [DataMember]
    //    public string number { get; set; }
    //    [DataMember]
    //    public string status { get; set; }
    //    [DataMember]
    //    public string description { get; set; }
    //    [DataMember]
    //    public bool executed { get; set; }
    //    [DataMember]
    //    public string created_at { get; set; }
    //    [DataMember]
    //    public bool @private { get; set; }
    //}




    //[DataContract]
    //public class SubcontractorList
    //{
    //    [DataMember]
    //    public int id { get; set; }
    //    [DataMember]
    //    public string first_name { get; set; }
    //    [DataMember]
    //    public string last_name { get; set; }
    //    [DataMember]
    //    public string job_title { get; set; }
    //    [DataMember]
    //    public string address { get; set; }
    //    [DataMember]
    //    public string city { get; set; }
    //    [DataMember]
    //    public string zip { get; set; }
    //    [DataMember]
    //    public string business_phone { get; set; }
    //    [DataMember]
    //    public string mobile_phone { get; set; }
    //    [DataMember]
    //    public string fax_number { get; set; }
    //    [DataMember]
    //    public string email_address { get; set; }
    //    [DataMember]
    //    public bool is_active { get; set; }
    //    [DataMember]
    //    public Vendor vendor { get; set; }
    //    [DataMember]
    //    public string last_login_at { get; set; }
    //}

    //[DataContract]
    //public class Vendor
    //{
    //    [DataMember]
    //    public int id { get; set; }
    //    [DataMember]
    //    public string name { get; set; }
    //}



    [DataContract]
    public class ProjectCommitments
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public bool executed { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool? @private { get; set; }
        [DataMember]
        public Vendor vendor { get; set; }

    }




    [DataContract]
    public class ShowCommitments
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string accounting_method { get; set; }
        [DataMember]
        public bool allow_comments { get; set; }
        [DataMember]
        public bool allow_markups { get; set; }
        [DataMember]
        public bool allow_payment_applications { get; set; }
        [DataMember]
        public bool allow_payments { get; set; }
        [DataMember]
        public bool allow_redistributions { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool display_materials_retainage { get; set; }
        [DataMember]
        public bool display_stored_materials { get; set; }
        [DataMember]
        public bool display_work_retainage { get; set; }
        [DataMember]
        public bool erp_rejected { get; set; }
        [DataMember]
        public bool erp_reviewable { get; set; }
        [DataMember]
        public string exclusions { get; set; }
        [DataMember]
        public bool executed { get; set; }
        [DataMember]
        public string grand_total { get; set; }
        [DataMember]
        public string inclusions { get; set; }
        [DataMember]
        public string line_items_extended_total { get; set; }
        [DataMember]
        public string line_items_total { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string total_payments { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
        [DataMember]
        public List<ChangeOrderPackage> change_order_packages { get; set; }
        [DataMember]
        public CreatedBy created_by { get; set; }
        [DataMember]
        public List<LineItem> line_items { get; set; }
        [DataMember]
        public List<PotentialChangeOrder> potential_change_orders { get; set; }
        [DataMember]
        public List<object> payments_issued { get; set; }
        [DataMember]
        public Vendor vendor { get; set; }
        [DataMember]
        public string approved_change_orders { get; set; }

    }

    [DataContract]
    public class ChangeOrderPackage
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class CreatedBy
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string login { get; set; }
    }

    [DataContract]
    public class CostCode
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string biller { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public string sortable_code { get; set; }
        [DataMember]
        public string full_code { get; set; }
    }

    [DataContract]
    public class LineItemType
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string base_type { get; set; }
    }

    [DataContract]
    public class LineItem
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string quantity { get; set; }
        [DataMember]
        public string uom { get; set; }
        [DataMember]
        public string total_amount { get; set; }
        [DataMember]
        public int cost_code_id { get; set; }
        [DataMember]
        public string unit_cost { get; set; }
        [DataMember]
        public CostCode cost_code { get; set; }
        [DataMember]
        public LineItemType line_item_type { get; set; }
    }

    [DataContract]
    public class PotentialChangeOrder
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class Vendor
    {
        [DataMember]
        public string abbreviated_name { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string state_code { get; set; }

        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string business_phone { get; set; }
        [DataMember]
        public string fax_number { get; set; }
        [DataMember]
        public string email_address { get; set; }
        [DataMember]
        public bool is_active { get; set; }
        [DataMember]
        public bool authorized_bidder { get; set; }
        [DataMember]
        public bool prequalified { get; set; }
        [DataMember]
        public List<int> project_ids { get; set; }
    }


    [DataContract]
    public class CompanyVendor
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string state_code { get; set; }

        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string business_phone { get; set; }
        [DataMember]
        public string fax_number { get; set; }

        [DataMember]
        public bool is_active { get; set; }

        [DataMember]
        public string country_code { get; set; }
        [DataMember]
        public List<int> project_ids { get; set; }
    }


    //===============================================

    [DataContract]
    public class Contractor
    {
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public bool authorized_bidder { get; set; }
        [DataMember]
        public string business_phone { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string country_code { get; set; }
        [DataMember]
        public string fax_number { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool is_active { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public bool non_union_prevailing_wage { get; set; }
        [DataMember]
        public bool prequalified { get; set; }
        [DataMember]
        public string state_code { get; set; }
        [DataMember]
        public bool union_member { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
        [DataMember]
        public List<int> project_ids { get; set; }
    }



    [DataContract]
    public class PrimeContact
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string accounting_method { get; set; }
        [DataMember]
        public bool allow_comments { get; set; }
        [DataMember]
        public bool allow_markups { get; set; }
        [DataMember]
        public bool allow_payment_applications { get; set; }
        [DataMember]
        public bool allow_payments { get; set; }
        [DataMember]
        public bool allow_redistributions { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool display_materials_retainage { get; set; }
        [DataMember]
        public bool display_stored_materials { get; set; }
        [DataMember]
        public bool display_work_retainage { get; set; }
        [DataMember]
        public bool erp_rejected { get; set; }
        [DataMember]
        public bool erp_reviewable { get; set; }
        [DataMember]
        public string exclusions { get; set; }
        [DataMember]
        public bool executed { get; set; }
        [DataMember]
        public string grand_total { get; set; }
        [DataMember]
        public string inclusions { get; set; }
        [DataMember]
        public string line_items_extended_total { get; set; }
        [DataMember]
        public string line_items_total { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public string retainage_percent { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string total_payments { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
        [DataMember]
        public List<ChangeOrderPackage> change_order_packages { get; set; }
        [DataMember]
        public Contractor contractor { get; set; }
        [DataMember]
        public CreatedBy created_by { get; set; }
        [DataMember]
        public List<LineItem> line_items { get; set; }
        [DataMember]
        public List<PotentialChangeOrder> potential_change_orders { get; set; }
        [DataMember]
        public List<object> payments_received { get; set; }
        [DataMember]
        public Vendor vendor { get; set; }
        [DataMember]
        public Architect architect { get; set; }
    }

    [DataContract]
    public class Architect
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }


    [DataContract]
    public class PotentialChangeOrderByProject
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string accounting_method { get; set; }
        [DataMember]
        public int change_order_change_reason_id { get; set; }
        [DataMember]
        public int change_order_request_id { get; set; }
        [DataMember]
        public int contract_id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool field_change { get; set; }
        [DataMember]
        public string grand_total { get; set; }
        [DataMember]
        public bool hidden { get; set; }
        [DataMember]
        public bool invoiced { get; set; }
        [DataMember]
        public string line_items_extended_total { get; set; }
        [DataMember]
        public string line_items_total { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public bool paid { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public int revision { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public bool @void { get; set; }
        //public Creator creator { get; set; }
        [DataMember]
        public List<LineItem> line_items { get; set; }
        [DataMember]
        public Vendor vendor { get; set; }
        [DataMember]
        public string reference { get; set; }
    }

    [DataContract]
    public class Creator
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string login { get; set; }
    }

    [DataContract]
    public class ShowChangeOrderRequests
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int change_order_package_id { get; set; }
        [DataMember]
        public int contract_id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string grand_total { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public int revision { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public Creator creator { get; set; }
    }


    [DataContract]
    public class ShowChangeOrderPackage
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int contract_id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool erp_rejected { get; set; }
        [DataMember]
        public bool erp_reviewable { get; set; }
        [DataMember]
        public bool executed { get; set; }
        [DataMember]
        public string grand_total { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public int revision { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public Creator creator { get; set; }
        [DataMember]
        public List<LineItem> line_items { get; set; }
    }



    //=====================submittal log

    //[DataContract]
    //public class ReceivedFrom
    //{
    //    [DataMember]
    //    public string name { get; set; }
    //    [DataMember]
    //    public string login { get; set; }
    //    [DataMember]
    //    public int login_id { get; set; }
    //}

    [DataContract]
    public class LoginInformation
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string login { get; set; }
    }


    [DataContract]
    public class Attachment
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string filename { get; set; }
    }


    [DataContract]
    public class Status
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class SpecificationSection
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string label { get; set; }
    }

    // [DataContract]
    //public class Type
    //{
    //    [DataMember]
    //    public string id { get; set; }
    //    [DataMember]
    //    public string name { get; set; }
    //}

    [DataContract]
    public class ResponsibleContractor
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class ReceivedFrom
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class Response
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class Approver
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string approver_type { get; set; }
        [DataMember]
        public int response_id { get; set; }
        [DataMember]
        public string sent_date { get; set; }
        [DataMember]
        public string returned_date { get; set; }
        [DataMember]
        public User user { get; set; }
    }

    [DataContract]
    public class BallInCourt
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class submittals
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string revision { get; set; }
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public SpecificationSection specification_section { get; set; }
        [DataMember]
        public int attachments_count { get; set; }
        [DataMember]
        public string due_date { get; set; }
        [DataMember]
        public string distributed_at { get; set; }
        [DataMember]
        public int created_by_id { get; set; }
        [DataMember]
        public string issue_date { get; set; }
        [DataMember]
        public string received_date { get; set; }
        //[DataMember]
        //public ProcoreType type { get; set; }
        [DataMember]
        public ResponsibleContractor responsible_contractor { get; set; }
        [DataMember]
        public string submit_by { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public ReceivedFrom received_from { get; set; }
        [DataMember]
        public List<Approver> approvers { get; set; }
        [DataMember]
        public List<BallInCourt> ball_in_court { get; set; }
    }



    //=======================

    [DataContract]
    public class User
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class SubmittalManager
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class ShowSubmittal
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string revision { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public List<Approver> approvers { get; set; }
        [DataMember]
        public int attachments_count { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string distributed_at { get; set; }
        [DataMember]
        public string due_date { get; set; }
        [DataMember]
        public string issue_date { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public string received_date { get; set; }
        [DataMember]
        public string submit_by { get; set; }
        [DataMember]
        public object type { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public object actual_delivery_date { get; set; }
        [DataMember]
        public object confirmed_delivery_date { get; set; }
        [DataMember]
        public bool current_revision { get; set; }
        [DataMember]
        public string custom_textarea_1 { get; set; }
        [DataMember]
        public string custom_textfield_1 { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public object design_team_review_time { get; set; }
        [DataMember]
        public object internal_review_time { get; set; }
        [DataMember]
        public object lead_time { get; set; }
        [DataMember]
        public object required_on_site_date { get; set; }
        [DataMember]
        public List<BallInCourt> ball_in_court { get; set; }
        [DataMember]
        public CreatedBy created_by { get; set; }
        [DataMember]
        public object location { get; set; }
        [DataMember]
        public ReceivedFrom received_from { get; set; }
        [DataMember]
        public ResponsibleContractor responsible_contractor { get; set; }
        [DataMember]
        public object specification_section { get; set; }
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public SubmittalManager submittal_manager { get; set; }
        [DataMember]
        public SubmittalPackage submittal_package { get; set; }
        [DataMember]
        public List<object> distribution_members { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
        [DataMember]
        public CostCode cost_code { get; set; }
        [DataMember]
        public object scheduled_task { get; set; }
    }

    [DataContract]
    public class SubmittalPackage
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int attachments_count { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string title { get; set; }
    }



    //=======User


    //public class Vendor
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //}

    [DataContract]
    public class ProjectUser
    {
        public object this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(ProjectUser);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(ProjectUser);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }

        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string business_phone { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string country_code { get; set; }
        [DataMember]
        public string email_address { get; set; }
        [DataMember]
        public string email_signature { get; set; }
        [DataMember]
        public string employee_id { get; set; }
        [DataMember]
        public string fax_number { get; set; }
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string initials { get; set; }
        [DataMember]
        public bool is_active { get; set; }
        [DataMember]
        public bool is_employee { get; set; }
        [DataMember]
        public string job_title { get; set; }
        [DataMember]
        public string last_name { get; set; }
        [DataMember]
        public string mobile_phone { get; set; }
        [DataMember]
        public string notes { get; set; }
        [DataMember]
        public string state_code { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public Vendor vendor { get; set; }

        [DataMember]
        public string name { get; set; }
    }


    //============ project role
    [DataContract]
    public class ProjectRole
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string role { get; set; }
        [DataMember]
        public int? contact_id { get; set; }
    }

    //============= Meeting Minutes

    [DataContract]
    public class Meeting
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string mode { get; set; }
        [DataMember]
        public string meeting_date { get; set; }
        [DataMember]
        public int parent_id { get; set; }
    }

    [DataContract]
    public class MeettingCategory
    {
        [DataMember]
        public string group_title { get; set; }
        [DataMember]
        public List<Meeting> meetings { get; set; }
    }



    [DataContract]
    public class Attendee
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public object status { get; set; }
        [DataMember]
        public LoginInformation login_information { get; set; }
    }

    [DataContract]
    public class MeetingCategory2
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
    }

    [DataContract]
    public class MeetingTopic
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string created_on { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public string due_date { get; set; }
        [DataMember]
        public string priority { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public object minutes { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public MeetingCategory2 meeting_category { get; set; }
        [DataMember]
        public List<Assignment> assignments { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
    }

    [DataContract]
    public class Assignment
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string login { get; set; }
    }

    [DataContract]
    public class MeetingCategory
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public int? position { get; set; }
        [DataMember]
        public List<MeetingTopic> meeting_topic { get; set; }
    }

    [DataContract]
    public class ShowMeeting
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string meeting_date { get; set; }
        [DataMember]
        public bool occurred { get; set; }
        [DataMember]
        public string start_time { get; set; }
        [DataMember]
        public string finish_time { get; set; }
        [DataMember]
        public bool is_private { get; set; }
        [DataMember]
        public bool is_draft { get; set; }
        [DataMember]
        public string mode { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string conclusion { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
        // public List<object> attachments { get; set; }
        [DataMember]
        public List<Attendee> attendees { get; set; }
        [DataMember]
        public List<MeetingCategory> meeting_categories { get; set; }
    }



    //=======================
    // drawing..

    [DataContract]
    public class DrawingSet
    {
        [DataMember]
        public int drawing_revisions_count { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string date { get; set; }
    }

    [DataContract]
    public class DrawingDiscipline
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public object position { get; set; }
    }

    [DataContract]
    public class DrawingArea
    {
        [DataMember]
        public int drawings_count { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<DrawingSet> drawing_sets { get; set; }
        [DataMember]
        public List<DrawingDiscipline> drawing_disciplines { get; set; }
    }


    [DataContract]
    public class CurrentRevision
    {
        [DataMember]
        public bool floorplan { get; set; }
        [DataMember]
        public bool has_drawing_sketches { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int pdf_size { get; set; }
        [DataMember]
        public string pdf_url { get; set; }
        [DataMember]
        public int png_size { get; set; }
        [DataMember]
        public string png_url { get; set; }
        [DataMember]
        public string revision_number { get; set; }
        [DataMember]
        public string thumbnail_url { get; set; }
        [DataMember]
        public string updated_at { get; set; }
    }

    [DataContract]
    public class Drawing
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string discipline { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public bool obsolete { get; set; }
        [DataMember]
        public CurrentRevision current_revision { get; set; }
    }

    [DataContract]
    public class Discipline
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public object position { get; set; }
    }

    [DataContract]
    public class DrawingRevision
    {
        [DataMember]
        public DrawingSet drawing_set { get; set; }
        [DataMember]
        public DrawingArea drawing_area { get; set; }
        [DataMember]
        public int? drawing_id { get; set; }
        [DataMember]
        public bool floorplan { get; set; }
        [DataMember]
        public bool has_drawing_sketches { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public int pdf_size { get; set; }
        [DataMember]
        public string pdf_url { get; set; }
        [DataMember]
        public int png_size { get; set; }
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public int width { get; set; }
        [DataMember]
        public string png_url { get; set; }
        [DataMember]
        public string revision_number { get; set; }
        [DataMember]
        public string thumbnail_url { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public int drawing_sketches_count { get; set; }
        [DataMember]
        public bool has_public_markup_layer_elements { get; set; }
        [DataMember]
        public string received_date { get; set; }
        [DataMember]
        public string drawing_date { get; set; }
        [DataMember]
        public object activity_stream_last_viewed_at { get; set; }
        [DataMember]
        public Discipline discipline { get; set; }
    }


    //========================RIFs

    [DataContract]
    public class CostImpact
    {
        [DataMember]
        public object status { get; set; }
        [DataMember]
        public object value { get; set; }
    }

    [DataContract]
    public class ScheduleImpact
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public object value { get; set; }
    }

    //public class BallInCourt
    //{
    //    public int id { get; set; }
    //    public string login { get; set; }
    //    public string name { get; set; }
    //}
    //[DataContract]
    //public class ResponsibleContractor
    //{
    //    [DataMember]
    //    public int id { get; set; }
    //    [DataMember]
    //    public string name { get; set; }
    //}

    [DataContract]
    public class Assignee
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    //public class Errors
    //{
    //}
    //[DataContract]
    //public class Question
    //{
    //    [DataMember]
    //    public int id { get; set; }
    //    [DataMember]
    //    public string body { get; set; }
    //    //public Errors errors { get; set; }
    //}

    [DataContract]
    public class RFIList
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string subject { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string due_date { get; set; }
        [DataMember]
        public string initiated_at { get; set; }
        [DataMember]
        public string time_resolved { get; set; }
        [DataMember]
        public object location_id { get; set; }
        [DataMember]
        public CostImpact cost_impact { get; set; }
        [DataMember]
        public ScheduleImpact schedule_impact { get; set; }
        [DataMember]
        public BallInCourt ball_in_court { get; set; }
        [DataMember]
        public ResponsibleContractor responsible_contractor { get; set; }
        [DataMember]
        public Assignee assignee { get; set; }
        [DataMember]
        public List<Question> questions { get; set; }
        [DataMember]
        public string created_at { get; set; }
    }



    //public class CreatedBy
    //{
    //    public string name { get; set; }
    //    public string login { get; set; }
    //    public int id { get; set; }
    //}
    [DataContract]
    public class Answer
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool official { get; set; }
        [DataMember]
        public string answer_date { get; set; }
        [DataMember]
        public string plain_text_body { get; set; }
        [DataMember]
        public string rich_text_body { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public List<Attachment> attachments { get; set; }
    }

    [DataContract]
    public class Question
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string question_date { get; set; }
        [DataMember]
        public string plain_text_body { get; set; }
        [DataMember]
        public string rich_text_body { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public List<Answer> answers { get; set; }
        [DataMember]
        public List<Attachment2> attachments { get; set; }
    }

    [DataContract]
    public class Attachment2
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string filename { get; set; }
        [DataMember]
        public string url { get; set; }
    }

    //public class Assignee
    //{
    //    public string name { get; set; }
    //    public string login { get; set; }
    //    public int id { get; set; }
    //}

    //public class ResponsibleContractor
    //{
    //    public string name { get; set; }
    //    public int id { get; set; }
    //}

    //public class ReceivedFrom
    //{
    //    public string name { get; set; }
    //    public string login { get; set; }
    //    public int id { get; set; }
    //}

    //public class BallInCourt
    //{
    //    public string name { get; set; }
    //    public string login { get; set; }
    //    public int id { get; set; }
    //}

    [DataContract]
    public class DistributionList
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string name { get; set; }
    }



    [DataContract]
    public class ShowRFI
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string subject { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string due_date { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public List<object> drawing_ids { get; set; }
        [DataMember]
        public bool draft { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public string drawing_number { get; set; }
        [DataMember]
        public bool accepted { get; set; }
        [DataMember]
        public string time_resolved { get; set; }
        [DataMember]
        public CreatedBy created_by { get; set; }
        [DataMember]
        public ScheduleImpact schedule_impact { get; set; }
        [DataMember]
        public List<Question> questions { get; set; }
        [DataMember]
        public Assignee assignee { get; set; }
        [DataMember]
        public List<DistributionList> distribution_list { get; set; }
        [DataMember]
        public ResponsibleContractor responsible_contractor { get; set; }
        [DataMember]
        public ReceivedFrom received_from { get; set; }
        [DataMember]
        public BallInCourt ball_in_court { get; set; }
        [DataMember]
        public string initiated_at { get; set; }
        [DataMember]
        public string reference { get; set; }
        [DataMember]
        public CustomTextfield1 custom_textfield_1 { get; set; }
        [DataMember]
        public CustomTextfield2 custom_textfield_2 { get; set; }
    }

    [DataContract]
    public class CustomTextfield1
    {
        [DataMember]
        public string label { get; set; }
        [DataMember]
        public string value { get; set; }
    }

    [DataContract]
    public class CustomTextfield2
    {
        [DataMember]
        public string label { get; set; }
        [DataMember]
        public string value { get; set; }
    }

    [DataContract]
    public class ChangeOrderRequests
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string updated_at { get; set; }
    }



    //public class CostCode
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string code { get; set; }
    //    public string full_code { get; set; }
    //}

    [DataContract]
    public class Division
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string full_code { get; set; }
    }

    //public class LineItemType
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //}
    [DataContract]
    public class BudgetLineItem
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string original_budget_amount { get; set; }
        [DataMember]
        public string approved_budget_changes { get; set; }
        [DataMember]
        public string revised_budget { get; set; }
        [DataMember]
        public string pending_budget_changes { get; set; }
        [DataMember]
        public string projected_budget { get; set; }
        [DataMember]
        public string committed_costs { get; set; }
        [DataMember]
        public string direct_costs { get; set; }
        [DataMember]
        public string pending_cost_changes { get; set; }
        [DataMember]
        public string projected_costs { get; set; }
        [DataMember]
        public string budget_forecast { get; set; }
        [DataMember]
        public string estimated_cost_at_completion { get; set; }
        [DataMember]
        public string projected_over_under { get; set; }
        [DataMember]
        public CostCode cost_code { get; set; }
        [DataMember]
        public Division division { get; set; }
        [DataMember]
        public LineItemType line_item_type { get; set; }
    }


    [DataContract]
    public class Trade
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }


    [DataContract]
    public class PunchItemType
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }

    [DataContract]
    public class PunchItem
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public object deleted_at { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string due_date { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int position { get; set; }
        [DataMember]
        public object priority { get; set; }
        [DataMember]
        public bool @private { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public bool has_resolved_responses { get; set; }
        [DataMember]
        public bool has_unresolved_responses { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public object location { get; set; }
        [DataMember]
        public Trade trade { get; set; }
        [DataMember]
        public CreatedBy created_by { get; set; }
        [DataMember]
        public PunchItemType punch_item_type { get; set; }
        [DataMember]
        public List<object> assignments { get; set; }
        [DataMember]
        public List<object> assignees { get; set; }
    }


    [DataContract]
    public class ChangeOrderPackages
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public string reviewed_at { get; set; }
    }

    [DataContract]
    public class PermissionTemplates
    {
        [DataMember]
        public PermissionTemplatesUser user { get; set; }
    }

    [DataContract]
    public class PermissionTemplatesUser
    {
        [DataMember]
        public int permission_template_id { get; set; }
    }

    [DataContract]
    public class CompanyUser
    {
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string business_phone { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string country_code { get; set; }
        [DataMember]
        public string email_address { get; set; }
        [DataMember]
        public string email_signature { get; set; }
        [DataMember]
        public string employee_id { get; set; }
        [DataMember]
        public string fax_number { get; set; }
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string initials { get; set; }
        [DataMember]
        public bool is_active { get; set; }
        [DataMember]
        public bool is_employee { get; set; }
        [DataMember]
        public string job_title { get; set; }
        [DataMember]
        public string last_name { get; set; }
        [DataMember]
        public string mobile_phone { get; set; }
        [DataMember]
        public string notes { get; set; }
        [DataMember]
        public string state_code { get; set; }
        [DataMember]
        public string zip { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public string updated_at { get; set; }
        [DataMember]
        public Vendor vendor { get; set; }
        [DataMember]
        public string avatar { get; set; }
        [DataMember]
        public int? business_phone_extension { get; set; }
        [DataMember]
        public string last_login_at { get; set; }
    }

}
