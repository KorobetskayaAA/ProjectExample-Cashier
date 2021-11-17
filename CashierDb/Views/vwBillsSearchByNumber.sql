CREATE VIEW [dbo].[vwBillsSearchByNumber]
	AS SELECT * FROM [dbo].[vwBills] WHERE Number BETWEEN 2 AND 3;
