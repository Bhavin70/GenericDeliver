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

namespace ODLMWebAPI.BL
{
    public class DimMstDeptBL : IDimMstDeptBL
    {
        #region Selection

        public List<DimMstDeptTO> SelectAllDimMstDeptList()
        {
            return DimMstDeptDAO.SelectAllDimMstDept();
        }

        public DimMstDeptTO SelectDimMstDeptTO(Int32 idDept)
        {
            return DimMstDeptDAO.SelectDimMstDept(idDept);
        }

        // Vaibhav [15-Sep-2017] added to fill department drop down.
        public List<DropDownTO> SelectDivisionDropDownList(Int32 DeptTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = DimMstDeptDAO.SelectAllDepartmentMasterForDropDown(DeptTypeId);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDepartmentForDropDown");
                return null;
            }
        }

        // Vaibhav [19-Sep-2017] added to fill department drop down.
        public List<DropDownTO> SelectDepartmentDropDownListByDivision(Int32 DivisionId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = DimMstDeptDAO.SelectDepartmentDropDownListByDivision(DivisionId);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectDepartmentDropDownListByDivision");
                return null;
            }
        }

        // Vaibhav [19-Sep-2017] added to get BOM department TO
        public DropDownTO SelectBOMDepartmentTO()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO dropDownTO = DimMstDeptDAO.SelectBOMDepartmentTO();
                if (dropDownTO != null)
                    return dropDownTO;
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectBOMDepartmentTO");
                return null;
            }
        }
   public List<DimMstDeptTO> SelectAllDimMstDeptList(SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDeptDAO.SelectAllDimMstDept(conn, tran);
        }
     
        // Vaibhav [25-Sep-2017] added to fill sub department drop down.
        public List<DropDownTO> SelectSubDepartmentDropDownListByDepartment(Int32 DepartmentId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> list = DimMstDeptDAO.SelectSubDepartmentDropDownListByDepartment(DepartmentId);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectSubDepartmentDropDownListByDepartment");
                return null;
            }
        }

        #endregion

        #region Insertion
        public int InsertDimMstDept(DimMstDeptTO dimMstDeptTO)
        {
            return DimMstDeptDAO.InsertDimMstDept(dimMstDeptTO);
        }

        public int InsertDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDeptDAO.InsertDimMstDept(dimMstDeptTO, conn, tran);
        }

        // Vaibhav [16-Sep-2017] Save department
        public ResultMessage SaveDepartment(DimMstDeptTO dimMstDeptTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();

                // Select current Identity
                SqlCommand cmdSelect = new SqlCommand();
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.Connection = conn;
                cmdSelect.CommandText = "SELECT Max(idDept) + 1 FROM dimMstDept ";

                int id = Convert.ToInt32(cmdSelect.ExecuteScalar());

                cmdSelect.Dispose();

                // Bind Identity to department name
                dimMstDeptTO.DeptCode = dimMstDeptTO.DeptDisplayName + " " + id;

                // Set default isVisible true 
                dimMstDeptTO.IsVisible = 1;

                // Set default org unit id to 19
                dimMstDeptTO.OrgUnitId = 19;

                result = InsertDimMstDept(dimMstDeptTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While SaveDepartment");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveDepartment");
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO)
        {
            return DimMstDeptDAO.UpdateDimMstDept(dimMstDeptTO);
        }

        public int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDeptDAO.UpdateDimMstDept(dimMstDeptTO, conn, tran);
        }

        // Vaibhav [16-Sep-2017] Update department
        public ResultMessage UpdateDepartment(DimMstDeptTO dimMstDeptTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                int result = 0;

                conn.Open();
                tran = conn.BeginTransaction();
                // Select Dept Code
                DimMstDeptTO dimMstDeptto = DimMstDeptDAO.SelectDimMstDept(dimMstDeptTO.IdDept, conn, tran);

                if (dimMstDeptto != null)
                {
                    String deptCode = dimMstDeptto.DeptCode;
                    // Bind Identity to department name
                    dimMstDeptTO.DeptCode = dimMstDeptTO.DeptDisplayName + " " + deptCode.Split(' ').Last();
                }

                if (dimMstDeptTO.IsVisible == 0)
                {


                    List<TblOrgStructureTO> orgainizationStructureList = TblOrgStructureBL.SelectAllOrgStructureList(conn, tran);
                    List<DimMstDeptTO> departmentList = SelectAllDimMstDeptList(conn, tran);

                    for (int i = 0; i < departmentList.Count; i++)
                    {
                        if (dimMstDeptTO.IdDept == departmentList[i].ParentDeptId)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateDepartment");
                            tran.Rollback();
                            return resultMessage;
                        }


                    }

                    for (int i = 0; i < orgainizationStructureList.Count; i++)
                    {
                        if (dimMstDeptTO.IdDept == orgainizationStructureList[i].DeptId)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateDepartment");
                            tran.Rollback();
                            return resultMessage;
                        }


                    }


                    result = UpdateDimMstDept(dimMstDeptTO, conn, tran);
                }
                else
                {
                     result = UpdateDimMstDept(dimMstDeptTO, conn, tran);
                }


                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateDepartment");
                    return resultMessage;
                }

                tran.Commit();

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateDepartment");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        #endregion


        #region Deletion
        public int DeleteDimMstDept(Int32 idDept)
        {
            return DimMstDeptDAO.DeleteDimMstDept(idDept);
        }

        public int DeleteDimMstDept(Int32 idDept, SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDeptDAO.DeleteDimMstDept(idDept, conn, tran);
        }

        #endregion

    }
}
