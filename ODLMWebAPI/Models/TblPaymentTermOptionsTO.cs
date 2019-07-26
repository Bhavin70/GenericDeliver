using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblPaymentTermOptionsTO
    {
        #region Declarations
        Int32 idPaymentTermOption;
        Int32 paymentTermId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String paymentTermOption;
        Int32 isSelected;
        //Aniket [10-7-2019]
        Int32 isDescriptive;
        String paymentTermsDescription; // to carry description from PaymentTermRelation table and pass to GUI
        #endregion

        #region Constructor
        public TblPaymentTermOptionsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPaymentTermOption
        {
            get { return idPaymentTermOption; }
            set { idPaymentTermOption = value; }
        }
        public Int32 PaymentTermId
        {
            get { return paymentTermId; }
            set { paymentTermId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public String PaymentTermOption
        {
            get { return paymentTermOption; }
            set { paymentTermOption = value; }
        }

        public int IsSelected { get => isSelected; set => isSelected = value; }
        public int IsDescriptive { get => isDescriptive; set => isDescriptive = value; }
        public string PaymentTermsDescription { get => paymentTermsDescription; set => paymentTermsDescription = value; }
        #endregion
    }
}
