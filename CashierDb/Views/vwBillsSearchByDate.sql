CREATE VIEW [dbo].[vwBillsSearchByStatus]
	AS SELECT * FROM [vwBills] WHERE StatusId = 2;
