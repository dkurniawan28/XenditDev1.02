using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Function
{
    public class InsertDB
    {
        PaymentGatewayContext entities = new PaymentGatewayContext();

        internal int InsertLog( string request = "", string respon = "", string url = "", DateTime? DatetimeNow = null)
        {

            RequestLog requestlog_ = new RequestLog();
            requestlog_.Request = request;
            requestlog_.Response = respon;
            requestlog_.Url = url;
            requestlog_.TimeStamp = DatetimeNow;
            entities.RequestLog.Add(requestlog_);
            entities.SaveChanges();
            int IdRequestLogs = requestlog_.IdRequestLog;
            return IdRequestLogs;
        }


        internal int InsertDetailLogTransaksi( int IdLogRequest = 0, int IdTransaksiPayment = 0, string Description = "", DateTime? DatetimeNow = null)
        {
            DetailLogTransaksi requestlog_ = new DetailLogTransaksi();
            requestlog_.IdLogRequest = IdLogRequest;
            requestlog_.IdTransaksiPayment = IdTransaksiPayment;
            requestlog_.Description = Description;
            requestlog_.Timestamp = DatetimeNow;
            entities.DetailLogTransaksi.Add(requestlog_);
            entities.SaveChanges();
            int IdDetailLogTransaksis = requestlog_.IdDetailLogTransaksi;
            return IdDetailLogTransaksis;
        }


        internal int InsertDetailLQr(int IdTransaksiPayment = 0, string IdXendit = "", decimal Amount = 0,string Description="",string QrString="",string CallbackUrl="",string Type="",string Status="", DateTime? Created = null, DateTime? Updated = null,string MetaData="")
        {
            DetailQr requestlog_ = new DetailQr();
            requestlog_.IdTransaksiPayment = IdTransaksiPayment;
            requestlog_.IdXendit = IdXendit;
            requestlog_.Amount = Amount;
            requestlog_.Description = Description;
            requestlog_.QrString = QrString;
            requestlog_.CallbackUrl = CallbackUrl;
            requestlog_.Type = Type;
            requestlog_.Status = Status;
            requestlog_.Created = Created;
            requestlog_.Updated = Updated;
            requestlog_.MetaData = MetaData;
            entities.DetailQr.Add(requestlog_);
            entities.SaveChanges();
            int IdQr = requestlog_.IdQr;
            return IdQr;
        }

        internal int InsertTransaksiPayment( string IdXendit = "", string external_id = "", string user_id = "", int IdMerchan=0,string merchant_name="",string merchant_profile_picture_url="",decimal amount=0,string payer_email="",string description="",DateTime? expiry_date=null,string invoice_url="",string status="",int should_exclude_credit_card=0,int should_send_email=0,DateTime? created=null,DateTime? updated=null,string currency="",DateTime? Timestamp=null)
        {
            TransaksiPayment requestlog_ = new TransaksiPayment();
            requestlog_.IdXendit = IdXendit;
            requestlog_.ExternalId = external_id;
            requestlog_.UserId = user_id;
            requestlog_.IdMerchan = IdMerchan;
            requestlog_.MerchantName = merchant_name;
            requestlog_.MerchantProfilePictureUrl = merchant_profile_picture_url;
            requestlog_.Amount = amount;
            requestlog_.PayerEmail = payer_email;
            requestlog_.Description = description;
            requestlog_.ExpiryDate = expiry_date;
            requestlog_.InvoiceUrl = invoice_url;
            requestlog_.Status = status;
            requestlog_.ShouldExcludeCreditCard = should_exclude_credit_card;
            requestlog_.ShouldSendEmail = should_send_email;
            requestlog_.Created = created;
            requestlog_.Updated = updated;
            requestlog_.Currency = currency;
            requestlog_.Timestamp = Timestamp;
            entities.TransaksiPayment.Add(requestlog_);
            entities.SaveChanges();
            int IdTransaksiPayments = requestlog_.IdTransaksiPayment;
            return IdTransaksiPayments;
        }

        internal int InsertDetailBank( string bank_code = "", string collection_type = "", string bank_account_number = "",decimal transfer_amount=0,string bank_branch="",string account_holder_name="",int identity_amount=0,int status=0,int IdTransaksiPayment=0)
        {
            DetailBank requestlog_ = new DetailBank();
            requestlog_.BankCode = bank_code;
            requestlog_.CollectionType = collection_type;
            requestlog_.BankAccountNumber = bank_account_number;
            requestlog_.TransferAmount = transfer_amount;
            requestlog_.BankBranch = bank_branch;
            requestlog_.AccountHolderName = account_holder_name;
            requestlog_.IdentityAmount = identity_amount;
            requestlog_.Status = status;
            requestlog_.IdTransaksiPayment = IdTransaksiPayment;
            entities.DetailBank.Add(requestlog_);
            entities.SaveChanges();
            int IdDetailBanks = requestlog_.IdDetailBank;
            return IdDetailBanks;
        }

        internal int InsertDetailOutlet( string retail_outlet_name = "", string payment_code = "", decimal transfer_amount = 0, string merchant_name = "", int status = 0,int IdTransaksiPayment=0)
        {
            DetailOutlet requestlog_ = new DetailOutlet();
            requestlog_.RetailOutletName = retail_outlet_name;
            requestlog_.PaymentCode = payment_code;
            requestlog_.TransferAmount = transfer_amount;
            requestlog_.TransferAmount = transfer_amount;
            requestlog_.MerchantName = merchant_name;
            requestlog_.Status = status;
            requestlog_.IdTransaksiPayment = IdTransaksiPayment;
            entities.DetailOutlet.Add(requestlog_);
            entities.SaveChanges();
            int IdDetailOutlets = requestlog_.IdDetailOutlet;
            return IdDetailOutlets;
        }

       

        internal int InsertDetailWallet(string ewallet_type="", int status = 0, int IdTransaksiPayment = 0)
        {
            DetailEwallet requestlog_ = new DetailEwallet();
            requestlog_.EwalletType = ewallet_type;
            requestlog_.Status = status;
            requestlog_.IdTransaksiPayment = IdTransaksiPayment;
            entities.DetailEwallet.Add(requestlog_);
            entities.SaveChanges();
            int IdDetailEwallets = requestlog_.IdDetailEwallet;
            return IdDetailEwallets;
        }


        internal int InsertDetailCC(int IdTransaksiPayment = 0,string IdXendit = "", string status = "", decimal authorized_amount = 0, 
            decimal capture_amount = 0, string currency = "", string credit_card_token_id = "", string business_id = "", string merchant_id = "", 
            string merchant_reference_code = "", string external_id = "", string eci = "", string charge_type = "", string masked_card_number = "",
            string card_brand = "", string card_type = "", string xid = "", string cavv = "", string descriptor="",string authorization_id="",
            string bank_reconciliation_id="",string issuing_bank_name="",string client_id="",string cvn_code="",string approval_code="", DateTime? created = null,string metadata="")
        {
            DetailCc requestlog_ = new DetailCc();
            requestlog_.IdTransaksiPayment = IdTransaksiPayment;
            requestlog_.IdXendit = IdXendit;
            requestlog_.Status = status;
            requestlog_.AuthorizedAmount = authorized_amount;
            requestlog_.CaptureAmount = capture_amount;
            requestlog_.Currency = currency;
            requestlog_.CreditCardTokenId = credit_card_token_id;
            requestlog_.BusinessId = business_id;
            requestlog_.MerchantId = merchant_id;
            requestlog_.MerchantReferenceCode = merchant_reference_code;
            requestlog_.ExternalId = external_id;
            requestlog_.Eci = eci;
            requestlog_.ChargeType = charge_type;
            requestlog_.MaskedCardNumber = masked_card_number;
            requestlog_.CardBrand = card_brand;
            requestlog_.CardType = card_type;
            requestlog_.Xid = xid;
            requestlog_.Cavv = cavv;
            requestlog_.Descriptor = descriptor;
            requestlog_.AuthorizationId = authorization_id;
            requestlog_.BankReconciliationId = bank_reconciliation_id;
            requestlog_.IssuingBankName = issuing_bank_name;
            requestlog_.ClientId = client_id;
            requestlog_.CvnCode = cvn_code;
            requestlog_.ApprovalCode = approval_code;
            requestlog_.Created = created;
            requestlog_.Metadata = metadata;
           
            entities.DetailCc.Add(requestlog_);
            entities.SaveChanges();
            int IdDetailCC = requestlog_.IdDetailCc;
            return IdDetailCC;
        }

    }
}
