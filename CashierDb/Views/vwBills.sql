CREATE VIEW [dbo].[vwBills]
	AS 
SELECT b.Number
     , b.Created
	 , bs.[Id] StatusId
	 , bs.[Name] StatusName
	 , COUNT(i.Barcode) ItemsCount
	 , SUM(i.Quantity * i.Price) BillSum
FROM [Bill] b 
  JOIN [BillStatus] bs ON b.Status = bs.Id
  JOIN [Item] i ON i.BillNumber = b.Number
GROUP BY b.Number
     , b.Created
	 , bs.[Id]
	 , bs.[Name];
