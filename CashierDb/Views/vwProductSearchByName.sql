CREATE VIEW [dbo].[vwProductSearchByName]
	AS SELECT * FROM Product WHERE [Name] LIKE '%вес%';
