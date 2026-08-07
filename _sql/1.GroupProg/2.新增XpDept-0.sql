--·s¼W XpDept
insert into dbo.XpDept (Id,Name,CorpId,DeptNo,MgrId,LevelNo,UpDeptId,Status)
select 
	Id=d.department_id,
	Name=d.department_cname,
	CorpId=d.Company_Id,
	DeptNo=d.department_code,
	MgrId=d.DEPARTMENT_LEADER_ID,
	LevelNo=case 
        WHEN d.DEPT_LEVEL_LEVEL in (10,20,25,30) THEN 1
        WHEN d.DEPT_LEVEL_LEVEL in (40) THEN 2
        WHEN d.DEPT_LEVEL_LEVEL in (50) THEN 3
        else 9 end,
	UpDeptId=d.part_DEPARTMENT_ID,
	Status=1 - d.DEPARTMENT_STATUS	--1/0¤¬Âà
from [192.168.246.26].[05200169].[dbo].vwZZ_department d
left join dbo.XpDept d2 on d.department_id=d2.Id
where 1=1
and d.Company_id=1
and d2.Id is null


-- update XpDept.MgrId
--select d2.MgrId, d.DEPARTMENT_LEADER_ID
update d2 set 
	d2.MgrId = d.DEPARTMENT_LEADER_ID
from dbo.XpDept d2
join [192.168.246.26].[05200169].[dbo].vwZZ_department d on d.Company_Id=1 and d.department_id=d2.Id
where d2.CorpId=1
and d2.MgrId != d.DEPARTMENT_LEADER_ID
