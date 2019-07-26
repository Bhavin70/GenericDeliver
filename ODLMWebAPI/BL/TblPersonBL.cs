using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
 
namespace ODLMWebAPI.BL
{     
    public class TblPersonBL : ITblPersonBL
    {
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly ITblPersonAddrDtlDAO _iTblPersonAddrDtlDAO;
        private readonly ITblOrgPersonDtlsDAO _iTblOrgPersonDtlsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblPersonBL(IConnectionString iConnectionString, ITblOrgPersonDtlsDAO iTblOrgPersonDtlsDAO, ITblPersonAddrDtlDAO iTblPersonAddrDtlDAO, ITblPersonDAO iTblPersonDAO, ITblAddressDAO iTblAddressDAO)
        {
            _iTblPersonDAO = iTblPersonDAO;
            _iTblAddressDAO = iTblAddressDAO;
            _iTblPersonAddrDtlDAO = iTblPersonAddrDtlDAO;
            _iTblOrgPersonDtlsDAO = iTblOrgPersonDtlsDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblPersonTO> SelectAllTblPersonList()
        {
            return _iTblPersonDAO.SelectAllTblPerson();
        }

        public List<TblPersonTO> selectPersonsForOffline()
        {

            return _iTblPersonDAO.selectPersonsForOffline();
        }

        public List<DropDownTO> selectPersonsDropdownForOffline()
        {

            return _iTblPersonDAO.selectPersonDropdownListOffline();
        }

        public TblPersonTO SelectTblPersonTO(Int32 idPerson)
        {
            return _iTblPersonDAO.SelectTblPerson(idPerson);
        }

        public List<TblPersonTO> SelectAllPersonListByOrganization(int organizationId)
        {
            return _iTblPersonDAO.SelectAllTblPersonByOrganization(organizationId);
        }
        //Aniket
        public List<TblPersonTOEmail> SelectAllPersonListByOrganizationForEmail(int organizationId)
        {
            List<TblPersonTOEmail> list1= _iTblPersonDAO.SelectblOrganizationForEmail(organizationId);
            List<TblPersonTOEmail> list2 = _iTblPersonDAO.SelectAllTblPersonOrganizationForEmail(organizationId);
            if (list1 != null && list1.Count > 0)
            {
                list2.AddRange(list1);
            }
            return list2;
        }
        /// <summary>
        /// Sudhir[20-March-2018] Added for Get Person List On OrganizationId also in tblOrgPersonDtls.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public List<TblPersonTO> SelectAllPersonListByOrganizationV2(int organizationId, Int32 personTypeId)
        {
            return _iTblPersonDAO.SelectAllPersonByOrganizations(organizationId, personTypeId);
        }

        /// <summary>
        /// Sudhir[21-June-2018]
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDropDownListOnPersonId(Int32 idPerson)
        {
            return _iTblPersonDAO.SelectDropDownListOnPersonId(idPerson);
        }

        public TblPersonTO GetPersonOnUserId(int userId)
        {
            int PersonId = _iTblPersonDAO.GetPersonIdOnUserId(userId);
            
            return SelectTblPersonTO(PersonId);
        }

        /// <summary>
        /// Sudhir[23-APR-2018] Added for GetPersonList Based on OrgType
        /// </summary>
        /// <param name="tblPersonTO"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectPersonBasedOnOrgType(Int32 OrgType)
        {
            List<DropDownTO> list = _iTblPersonDAO.SelectPersonsBasedOnOrgType(OrgType);
            if (list != null)
            {
                list = list.OrderBy(ele => ele.Text).ToList();
                return list;
            }
            else
                return null;
        }

        public List<TblPersonTO> SelectAllTblPersonByRoleType(Int32 roleTypeId)
        {
            return _iTblPersonDAO.SelectAllTblPersonByRoleType(roleTypeId);
        }

        #endregion

        #region Insertion

        public ResultMessage AddNewPerson(TblPersonTO tblPersonTO)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                result = InsertTblPerson(tblPersonTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While AddNewPerson");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ResultMessage AddNewPersonWithAddressDetails(TblPersonTO tblPersonTO, TblAddressTO tblAddressTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Insert into tblPerson
                result = InsertTblPerson(tblPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Inserting New Person");
                    return resultMessage;
                }

                tblAddressTO.CreatedOn = tblPersonTO.CreatedOn;
                tblAddressTO.CreatedBy = tblPersonTO.CreatedBy;
                //Insert in to tblAddress 
                result = _iTblAddressDAO.InsertTblAddress(tblAddressTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Inserting Into TblAddress While adding  New Person");
                    return resultMessage;
                }

                if (result == 1)
                {
                    //Here Add Person And Address Mapping

                    TblPersonAddrDtlTO tblPersonAddrDtlTO = new TblPersonAddrDtlTO();
                    tblPersonAddrDtlTO.AddressId = tblAddressTO.IdAddr;
                    tblPersonAddrDtlTO.AddressTypeId = tblAddressTO.AddrTypeId;
                    tblPersonAddrDtlTO.PersonId = tblPersonTO.IdPerson;
                    tblPersonAddrDtlTO.CreatedBy = tblPersonTO.CreatedBy;
                    tblPersonAddrDtlTO.CreatedOn = tblPersonTO.CreatedOn;
                    result = _iTblPersonAddrDtlDAO.InsertTblPersonAddrDtl(tblPersonAddrDtlTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While Inserting into TblPersonAddrDtlTO While adding  New Person");
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour("Error into AddNewPersonWithAddressDetails");
                return resultMessage;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public int InsertTblPerson(TblPersonTO tblPersonTO)
        {
            return _iTblPersonDAO.InsertTblPerson(tblPersonTO);
        }

        public int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonDAO.InsertTblPerson(tblPersonTO, conn, tran);
        }

        /// <summary>
        /// Sudhir[20-MARCH-2018] Added for Add Person based on OrganizationId and PersonTypeId.
        /// </summary>
        /// <param name="tblPersonTO"></param>
        /// <param name="organizationId"></param>
        /// <param name="personTypeId"></param>
        /// <returns></returns>
        public ResultMessage SaveNewPersonAgainstOrganization(TblPersonTO tblPersonTO, Int32 organizationId, Int32 personTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = InsertTblPerson(tblPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AddNewPersonWithOrgId");
                    return resultMessage;
                }

                TblOrgPersonDtlsTO tblOrgPersonDtlsTO = new TblOrgPersonDtlsTO();
                tblOrgPersonDtlsTO.PersonId = tblPersonTO.IdPerson;
                tblOrgPersonDtlsTO.OrganizationId = organizationId;
                tblOrgPersonDtlsTO.IsActive = 1;
                tblOrgPersonDtlsTO.CreatedBy = tblPersonTO.CreatedBy;
                tblOrgPersonDtlsTO.CreatedOn = tblPersonTO.CreatedOn;
                tblOrgPersonDtlsTO.PersonTypeId = personTypeId;

                result = _iTblOrgPersonDtlsDAO.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AddNewPersonWithOrgId");
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Tag = tblPersonTO; //Added Sudhir for sending result back.
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error While Adding New Person with organizationId");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


        }
        #endregion

        #region Updation
        public int UpdateTblPerson(TblPersonTO tblPersonTO)
        {
            return _iTblPersonDAO.UpdateTblPerson(tblPersonTO);
        }

        public int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonDAO.UpdateTblPerson(tblPersonTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPerson(Int32 idPerson)
        {
            return _iTblPersonDAO.DeleteTblPerson(idPerson);
        }

        public int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonDAO.DeleteTblPerson(idPerson, conn, tran);
        }

        /// <summary>
        /// Tejaswini [13-11-2018] Added for Getting Birthday or anniversory today
        /// </summary>
        /// <param name="BirthdayAlertTO"></param>
        /// <returns></returns>
        public List<BirthdayAlertTO> SelectAllPersonBirthday(DateTime Today, Int32 UpcomingDays, Int32 IsBirthday)
        {
            List<BirthdayAlertTO> list = _iTblPersonDAO.SelectAllTblPersonByBirthdayAnniversory(Today, UpcomingDays, IsBirthday);
            return list;
        }
        #endregion

    }
}
