using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblGroupItemTO
    {
        #region Declarations
        Int32 idGroupItem;
        Int32 groupId;
        Int32 prodItemId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        String   prodItemDesc;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 materialId;

        #endregion

        #region Constructor
        public TblGroupItemTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGroupItem
        {
            get { return idGroupItem; }
            set { idGroupItem = value; }
        }
        public Int32 GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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

        public string ProdItemDesc { get => prodItemDesc; set => prodItemDesc = value; }
        public int ProdCatId { get => prodCatId; set => prodCatId = value; }
        public int ProdSpecId { get => prodSpecId; set => prodSpecId = value; }
        public int MaterialId { get => materialId; set => materialId = value; }
        #endregion
    }
}
