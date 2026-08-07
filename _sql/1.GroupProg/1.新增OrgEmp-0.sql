--新增 OrgEmp, 注意 p.bemp_year 輸入正確年度!!
insert into dbo.OrgEmp(
	Id,Idno,EmpNo,Name,DeptId,
	DeptNo,LevelNo,BankName,
	BankAccount,WorkStatus,ProjectId)
SELECT --distinct
	Id=e.Employee_Id, 
	Idno=e.EMPLOYEE_IDC_NO,
	EmpNo=e.Employee_No, 
	Name=e.Employee_Cname,
	DeptId=e.Department_Id,
	DeptNo=e.DEPARTMENT_CODE,
	--職級
	LevelNo=case LEFT(e.Level_Cname, 1) when '一' then 1 when '二' then 2 when '三' then 3 else 9 end,
	BankName=e.EMPLOYEE_BANK_CNAME1,
	BankAccount=e.EMPLOYEE_BACC1_NO,
	WorkStatus=e.Employee_Work_Status,
	ProjectId=p.bemp_id_proj
  from [192.168.246.26].[05200169].[dbo].vwZZ_Employee e
  left join dbo.OrgEmp e2 on e.Employee_id=e2.Id 
  --join [192.168.236.170].[FN_EDEN].[dbo].FNBEmp p on p.bemp_year = 114 and e.Employee_No = p.bemp_empno  	
  OUTER APPLY (
    SELECT TOP 1 *
    FROM [192.168.236.170].[FN_EDEN].[dbo].FNBEmp p
    WHERE p.bemp_year = 115 
      AND p.bemp_empno = e.Employee_No
    ORDER BY p.bemp_id_proj DESC
) p
  where 1=1
  and e.Company_Id='1'
  and e2.Id is null		--找不到 OrgEmp 者 for 新增
  order by e.Employee_Id


--更新 OrgEmp.LevelNo, DeptId, DeptNo(cross apply 會簡化語法, 但較不易理解!!)
--select HrLevelNo=case LEFT(e.Level_Cname, 1) when '一' then 1 when '二' then 2 when '三' then 3 else 9 end, e2.*
update e2 set LevelNo = case LEFT(e.Level_Cname, 1) when '一' then 1 when '二' then 2 when '三' then 3 else 9 end
from dbo.OrgEmp e2
join [192.168.246.26].[05200169].[dbo].vwZZ_Employee e on e2.EmpNo=e.Employee_No
where e2.LevelNo != case LEFT(e.Level_Cname, 1) when '一' then 1 when '二' then 2 when '三' then 3 else 9 end


--更新 OrgEmp.DeptId, DeptNo
--select e2.EmpNo, e2.Name, e2.DeptId, e2.DeptNo, e.Department_Id, e.Department_Code
update e2 set DeptId=e.Department_Id, DeptNo=e.Department_Code
from dbo.OrgEmp e2
join [192.168.246.26].[05200169].[dbo].vwZZ_Employee e on e2.EmpNo=e.Employee_No
where e2.DeptId != e.Department_Id


--更新 OrgEmp.BankAccount, WorkStatus
--select e2.BankAccount, e.EMPLOYEE_BACC1_NO
update e2 set 
	BankAccount = e.EMPLOYEE_BACC1_NO,
	WorkStatus = e.Employee_Work_Status
from dbo.OrgEmp e2
join [192.168.246.26].[05200169].[dbo].vwZZ_Employee e on e2.EmpNo=e.Employee_No
where 1=1
and (e2.BankAccount != e.EMPLOYEE_BACC1_NO or e2.WorkStatus != e.Employee_Work_Status)


--新增 OrgEmpDept
insert into dbo.OrgEmpDept (EmpId, DeptId, StartDate)
SELECT 
	ex.EMPLOYEE_ID, ex.DEPARTMENT_ID_AF, ex.EFFECTIVE_DATE
FROM [192.168.246.26].[05200169].[dbo].[vwZZ_EDEN_DEPT_VARIATION] ex
left join dbo.OrgEmpDept ed on ex.DEPARTMENT_ID_AF=ed.DeptId and ex.EMPLOYEE_ID=ed.EmpId and ex.EFFECTIVE_DATE=ed.StartDate
where 1=1
and ex.VARIATION_STATUS=1
and ed.EmpId is null
