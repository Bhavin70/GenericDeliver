﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    #region Mail sending class Date: 2/10/2017 by vinod thorat 

    public class SendMail
    {
        #region property define for the mail sending Date: 2/10/2017 by vinod thorat 

        String userName;
        String message;
        String fromTitle;
        String from;
        String bcc;
        String cc;
        String subject;
        String toTitle;
        String to;
        String host;
        bool enableSsl;
        string password;
        Int32 port;
        string attachements;
        string bodyContent;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 senderId;
        TblPersonTO personTO;
        bool isUpdatePerson;
        string weighmentSlip;
        //Aniket [22-03-2019]
        bool isWeighmentSlipAttach;
        string invoiceNumber;
        bool isInvoiceAttach;

        Boolean isSendEmailForInvoice;
        Boolean isSendEmailForWeighment;
        Int32 invoiceId;

        #endregion

        #region Constructor for the mail sending Date: 2/10/2017 by vinod thorat 

        public SendMail()
        {
        }
        #endregion

        #region All the properties of the send mail Date: 2/10/2017 by vinod thorat 

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Bcc
        {
            get { return bcc; }
            set { bcc = value; }
        }
        public string CC
        {
            get { return cc; }
            set { cc = value; }
        }
        public string Attachements
        {
            get { return attachements; }
            set { attachements = value; }
        }
        public String Message
        {
            get { return message; }
            set { message = value; }
        }
        public string FromTitle
        {
            get { return fromTitle; }
            set { fromTitle = value; }
        }
        public string From
        {
            get { return from; }
            set { from = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string ToTitle
        {
            get { return toTitle; }
            set { toTitle = value; }
        }
        public string To
        {
            get { return to; }
            set { to = value; }
        }
        public string Host
        {
            get { return host; }
            set { host = value; }
        }
        public bool EnableSsl
        {
            get { return enableSsl; }
            set { enableSsl = value; }
        }
        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        public Int32 Port
        {
            get { return port; }
            set { port = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        public string BodyContent
        {
            get { return bodyContent; }
            set { bodyContent = value; }
        }

        public Int32 SenderId
        {
            get { return senderId; }
            set { senderId = value; }
        }

        public bool IsUpdatePerson { get => isUpdatePerson; set => isUpdatePerson = value; }
        public TblPersonTO PersonInfo { get => personTO; set => personTO = value; }
        public string WeighmentSlip { get => weighmentSlip; set => weighmentSlip = value; }
        public bool IsWeighmentSlipAttach { get => isWeighmentSlipAttach; set => isWeighmentSlipAttach = value; }
        public bool IsInvoiceAttach { get => isInvoiceAttach; set => isInvoiceAttach = value; }
        public string InvoiceNumber { get => invoiceNumber; set => invoiceNumber = value; }

        public Boolean IsSendEmailForInvoice { get => isSendEmailForInvoice; set => isSendEmailForInvoice = value; }

        public Boolean IsSendEmailForWeighment { get => isSendEmailForWeighment; set => isSendEmailForWeighment = value; }

        public Int32 InvoiceId { get => invoiceId; set => invoiceId = value; }

        #endregion



    }

    #endregion
}
